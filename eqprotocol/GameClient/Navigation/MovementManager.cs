using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OpenEQ.Netcode.GameClient.Navigation
{
    public class MovementSpeed
    {
        public float BaseSpeed { get; set; } = 0.7f; // EQ base running speed
        public float SpeedMultiplier { get; set; } = 1.0f; // For buffs, debuffs, etc.
        
        public float EffectiveSpeed => BaseSpeed * SpeedMultiplier;
    }

    public class SimpleVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public SimpleVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class MovementState
    {
        public SimpleVector3 CurrentPosition { get; set; } = new SimpleVector3(0, 0, 0);
        public SimpleVector3 TargetPosition { get; set; }
        public List<SimpleVector3> Path { get; set; } = new List<SimpleVector3>();
        public int CurrentPathIndex { get; set; }
        public bool IsMoving { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
    }

    public class MovementManager
    {
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<MovementManager> _logger;
        private readonly MovementSpeed _movementSpeed;
        private readonly MovementState _movementState;
        private Timer _movementTimer;
        
        // Movement update intervals
        private const int MOVEMENT_UPDATE_INTERVAL_MS = 100; // Send updates every 100ms
        private const float WAYPOINT_THRESHOLD = 2.0f; // Distance to consider waypoint reached
        private const float DESTINATION_THRESHOLD = 0.5f; // Distance to consider destination reached

        public event Action<float, float, float, float> OnPositionUpdate; // x, y, z, heading
        private readonly Action<float, float, float, float> _sendPositionToServer;

        public MovementManager(NavigationManager navigationManager, ILogger<MovementManager> logger, Action<float, float, float, float> sendPositionToServer)
        {
            _navigationManager = navigationManager;
            _logger = logger;
            _movementSpeed = new MovementSpeed();
            _movementState = new MovementState();
            _sendPositionToServer = sendPositionToServer;
        }

        public MovementSpeed Speed => _movementSpeed;
        public MovementState State => _movementState;
        public bool IsMoving => _movementState.IsMoving;

        public void SetCurrentPosition(float x, float y, float z)
        {
            _movementState.CurrentPosition = new SimpleVector3(x, y, z);
            _movementState.LastUpdateTime = DateTime.UtcNow;
        }

        public async Task<bool> MoveTo(float x, float y, float z)
        {
            try
            {
                var targetPos = new SimpleVector3(x, y, z);
                var currentPos = _movementState.CurrentPosition;

                _logger.LogDebug("MoveTo requested: (X: {}, Y: {}, Z: {}) from ( CurX: {}, CurY: {}, CurZ: {})", x, y, z, currentPos.X, currentPos.Y, currentPos.Z);

                // Simple straight-line path for now (will be enhanced with proper nav mesh later)
                var path = await CreateSimplePathAsync(currentPos, targetPos);
                if (path == null || path.Count == 0)
                {
                    _logger.LogWarning("Could not create path to target position");
                    return false;
                }

                // Start movement
                _movementState.Path = path;
                _movementState.TargetPosition = targetPos;
                _movementState.CurrentPathIndex = 0;
                _movementState.IsMoving = true;

                StartMovementTimer();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MoveTo");
                return false;
            }
        }

        public void StopMovement()
        {
            _movementState.IsMoving = false;
            _movementState.Path.Clear();
            _movementState.CurrentPathIndex = 0;
            StopMovementTimer();
            _logger.LogInformation("Movement stopped");
        }

        private async Task<List<SimpleVector3>> CreateSimplePathAsync(SimpleVector3 start, SimpleVector3 end)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var path = new List<SimpleVector3>();
                    
                    // Create a straight-line path with smaller steps for smoother movement
                    var distance = CalculateDistance(start, end);
                    var steps = Math.Max(1, (int)(distance / 2.0f)); // Create waypoints every 2 units for smoother movement
                    
                    for (int i = 0; i <= steps; i++)
                    {
                        var t = steps > 0 ? (float)i / steps : 1.0f;
                        var waypoint = new SimpleVector3(
                            start.X + (end.X - start.X) * t,
                            start.Y + (end.Y - start.Y) * t,
                            start.Z + (end.Z - start.Z) * t
                        );
                        path.Add(waypoint);
                    }

                    _logger.LogInformation("Created simple path with {Count} waypoints", path.Count);
                    return path;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating simple path");
                    return null;
                }
            });
        }

        private void StartMovementTimer()
        {
            StopMovementTimer();
            _movementTimer = new Timer(UpdateMovement, null, 0, MOVEMENT_UPDATE_INTERVAL_MS);
        }

        private void StopMovementTimer()
        {
            _movementTimer?.Dispose();
            _movementTimer = null;
        }

        private void UpdateMovement(object state)
        {
            try
            {
                if (!_movementState.IsMoving || _movementState.Path.Count == 0)
                    return;

                var now = DateTime.UtcNow;
                var deltaTime = (float)(now - _movementState.LastUpdateTime).TotalSeconds;
                _movementState.LastUpdateTime = now;

                // Get current target waypoint
                if (_movementState.CurrentPathIndex >= _movementState.Path.Count)
                {
                    // Reached destination
                    StopMovement();
                    _logger.LogInformation("Reached destination");
                    return;
                }

                var currentWaypoint = _movementState.Path[_movementState.CurrentPathIndex];
                var currentPos = _movementState.CurrentPosition;

                // Calculate distance to current waypoint
                var distance = CalculateDistance(currentPos, currentWaypoint);

                // Check if we've reached the current waypoint
                if (distance <= WAYPOINT_THRESHOLD)
                {
                    _movementState.CurrentPathIndex++;
                    if (_movementState.CurrentPathIndex >= _movementState.Path.Count)
                    {
                        // Reached final destination
                        _movementState.CurrentPosition = _movementState.TargetPosition;
                        StopMovement();
                        return;
                    }
                    currentWaypoint = _movementState.Path[_movementState.CurrentPathIndex];
                    distance = CalculateDistance(currentPos, currentWaypoint);
                }

                // Calculate movement for this frame
                var speed = _movementSpeed.EffectiveSpeed;
                var maxMoveDistance = speed * deltaTime;

                SimpleVector3 newPosition;
                if (distance <= maxMoveDistance)
                {
                    // We can reach the waypoint this frame
                    newPosition = currentWaypoint;
                }
                else
                {
                    // Move toward the waypoint
                    var direction = new SimpleVector3(
                        (currentWaypoint.X - currentPos.X) / distance,
                        (currentWaypoint.Y - currentPos.Y) / distance,
                        (currentWaypoint.Z - currentPos.Z) / distance
                    );

                    newPosition = new SimpleVector3(
                        currentPos.X + direction.X * maxMoveDistance,
                        currentPos.Y + direction.Y * maxMoveDistance,
                        currentPos.Z + direction.Z * maxMoveDistance
                    );
                }

                // Update position
                _movementState.CurrentPosition = newPosition;

                // Calculate heading (facing direction)
                var heading = CalculateHeading(currentPos, currentWaypoint);

                // Send position update to server (server will broadcast to all clients including us)
                _sendPositionToServer?.Invoke(newPosition.X, newPosition.Y, newPosition.Z, heading);
                
                // Also fire local event for any listeners
                OnPositionUpdate?.Invoke(newPosition.X, newPosition.Y, newPosition.Z, heading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating movement");
                StopMovement();
            }
        }

        private static float CalculateDistance(SimpleVector3 pos1, SimpleVector3 pos2)
        {
            var dx = pos2.X - pos1.X;
            var dy = pos2.Y - pos1.Y;
            var dz = pos2.Z - pos1.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        private static float CalculateHeading(SimpleVector3 from, SimpleVector3 to)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;
            
            // Convert to EQ heading format (0-255, where 0 is North)
            var radians = Math.Atan2(-dx, dy); // EQ has inverted X axis
            var degrees = radians * 180.0 / Math.PI;
            if (degrees < 0) degrees += 360;
            
            return (float)(degrees * 256.0 / 360.0); // Convert to EQ heading format
        }

        public void Dispose()
        {
            StopMovement();
        }
    }
}