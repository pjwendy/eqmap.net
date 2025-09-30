using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    /// <summary>
    /// Zone class that provides Lua-accessible interface to game client zone and position data
    /// This class is now a lightweight wrapper around EQGameClient
    /// </summary>
    public class Zone
    {
        private readonly EQGameClient gameClient;

        public Zone(EQGameClient gameClient)
        {
            this.gameClient = gameClient;
        }

        #region Zone Information

        /// <summary>
        /// Get the current zone name from game client (raw name)
        /// </summary>
        public string CurrentZoneName => gameClient?.CurrentZone?.Name ?? "";

        /// <summary>
        /// Get the current zone ID
        /// </summary>
        public int CurrentZoneId => (int)(gameClient?.ZoneId ?? 0);

        /// <summary>
        /// Get the proper zone short name using ZoneUtils mapping (delegate to GameClient)
        /// </summary>
        public string Name => gameClient?.ZoneName ?? "";

        /// <summary>
        /// Get the current zone ID (alias for CurrentZoneId for Lua convenience)
        /// </summary>
        public int Id => CurrentZoneId;

        #endregion

        #region Player Position Information

        /// <summary>
        /// Get player's current X coordinate (delegate to GameClient)
        /// </summary>
        public float X => gameClient?.X ?? 0;

        /// <summary>
        /// Get player's current Y coordinate (delegate to GameClient)
        /// </summary>
        public float Y => gameClient?.Y ?? 0;

        /// <summary>
        /// Get player's current Z coordinate (delegate to GameClient)
        /// </summary>
        public float Z => gameClient?.Z ?? 0;

        /// <summary>
        /// Get player's current heading (delegate to GameClient)
        /// </summary>
        public float Heading => gameClient?.Heading ?? 0;

        /// <summary>
        /// Get player's name (delegate to GameClient)
        /// </summary>
        public string PlayerName => gameClient?.PlayerName ?? "";

        /// <summary>
        /// Get all position information as a formatted string (delegate to GameClient)
        /// </summary>
        public string Position => gameClient?.Position ?? "(0.0, 0.0, 0.0)";

        /// <summary>
        /// Get full location information as a formatted string (delegate to GameClient)
        /// </summary>
        public string Location => gameClient?.Location ?? "Unknown (0.0, 0.0, 0.0)";

        /// <summary>
        /// Get distance between current position and target coordinates (delegate to GameClient)
        /// </summary>
        /// <param name="targetX">Target X coordinate</param>
        /// <param name="targetY">Target Y coordinate</param>
        /// <param name="targetZ">Target Z coordinate (optional, defaults to current Z)</param>
        /// <returns>3D distance if Z provided, 2D distance otherwise</returns>
        public double DistanceTo(double targetX, double targetY, double targetZ = double.NaN)
        {
            return gameClient?.DistanceTo(targetX, targetY, targetZ) ?? double.PositiveInfinity;
        }

        /// <summary>
        /// Check if player is within a certain distance of target coordinates (delegate to GameClient)
        /// </summary>
        /// <param name="targetX">Target X coordinate</param>
        /// <param name="targetY">Target Y coordinate</param>
        /// <param name="maxDistance">Maximum distance</param>
        /// <param name="targetZ">Target Z coordinate (optional)</param>
        /// <returns>True if within distance</returns>
        public bool IsWithinDistance(double targetX, double targetY, double maxDistance, double targetZ = double.NaN)
        {
            return gameClient?.IsWithinDistance(targetX, targetY, maxDistance, targetZ) ?? false;
        }

        #endregion

        #region Movement Methods (delegate to GameClient)

        /// <summary>
        /// Move to specified coordinates asynchronously
        /// </summary>
        public async Task<bool> MoveTo(float x, float y, float z)
        {
            if (gameClient == null) return false;
            return await gameClient.MoveTo(x, y, z);
        }

        /// <summary>
        /// Move to specified coordinates (synchronous wrapper for Lua)
        /// </summary>
        public void MoveTo(double x, double y, double z)
        {
            if (gameClient != null)
            {
                // Run fire-and-forget for Lua convenience
                Task.Run(async () => await gameClient.MoveTo((float)x, (float)y, (float)z));
            }
        }

        /// <summary>
        /// Stop current movement
        /// </summary>
        public void StopMovement()
        {
            gameClient?.StopMovement();
        }

        /// <summary>
        /// Check if character is currently moving
        /// </summary>
        public bool IsMoving => gameClient?.IsMoving ?? false;

        #endregion

        #region Sleep Methods (delegate to GameClient)

        /// <summary>
        /// Sleep for specified milliseconds (delegate to GameClient)
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to sleep</param>
        public void Sleep(double milliseconds)
        {
            gameClient?.Sleep(milliseconds);
        }

        /// <summary>
        /// Sleep for specified seconds (delegate to GameClient)
        /// </summary>
        /// <param name="seconds">Number of seconds to sleep</param>
        public void SleepSeconds(double seconds)
        {
            gameClient?.SleepSeconds(seconds);
        }

        #endregion

        #region Deprecated Methods (kept for backward compatibility)

        /// <summary>
        /// [DEPRECATED] Update player position - no longer needed as GameClient handles this automatically
        /// </summary>
        [Obsolete("This method is deprecated. Position updates are handled automatically by GameClient.")]
        public void UpdatePlayerPosition(float x, float y, float z, string playerName = "")
        {
            // This method is now a no-op since GameClient handles position tracking
            // Kept for backward compatibility only
        }

        /// <summary>
        /// [DEPRECATED] Request zone change - not currently supported
        /// </summary>
        [Obsolete("This method is deprecated. Zone changes are handled by the server.")]
        public void RequestZone(string zoneName)
        {
            // This method is now a no-op
            // Zone changes happen through the server, not client requests
        }

        #endregion
    }
}
