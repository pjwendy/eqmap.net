using System;
using System.Collections.Concurrent;
using OpenEQ.Netcode.GameClient.Maps;
using Microsoft.Extensions.Logging;

namespace OpenEQ.Netcode.GameClient.Navigation
{
    // Simple navigation data structure for now
    public class SimpleNavMesh
    {
        public string ZoneName { get; set; }
        public byte[] NavData { get; set; }
        public bool IsLoaded { get; set; }
    }

    public class NavigationManager
    {
        private readonly ILogger<NavigationManager> _logger;
        private readonly ConcurrentDictionary<string, SimpleNavMesh> _navMeshes;
        private string _currentZone;
        private SimpleNavMesh _currentNavMesh;

        public NavigationManager(ILogger<NavigationManager> logger)
        {
            _logger = logger;
            _navMeshes = new ConcurrentDictionary<string, SimpleNavMesh>();
        }

        public string CurrentZone => _currentZone;
        public bool HasNavMesh => _currentNavMesh?.IsLoaded == true;

        public bool LoadNavMeshForZone(string zoneName)
        {
            try
            {
                // Check if already loaded in cache
                if (_navMeshes.TryGetValue(zoneName, out var cachedMesh))
                {
                    _currentZone = zoneName;
                    _currentNavMesh = cachedMesh;
                    _logger.LogInformation("Using cached nav mesh for zone: {ZoneName}", zoneName);
                    return true;
                }

                // Load nav file
                var navData = NavReader.ReadNavFile(zoneName);
                if (navData == null)
                {
                    _logger.LogWarning("No nav file found for zone: {ZoneName}", zoneName);
                    return false;
                }

                // Create simple nav mesh container (for now, just store the raw data)
                var navMesh = new SimpleNavMesh
                {
                    ZoneName = zoneName,
                    NavData = navData,
                    IsLoaded = true
                };

                // Cache the nav mesh
                _navMeshes[zoneName] = navMesh;
                _currentZone = zoneName;
                _currentNavMesh = navMesh;

                _logger.LogInformation("Successfully loaded nav mesh for zone: {ZoneName}", zoneName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading nav mesh for zone: {ZoneName}", zoneName);
                return false;
            }
        }

        public SimpleNavMesh GetNavMesh()
        {
            return _currentNavMesh;
        }

        public void ClearCache()
        {
            _navMeshes.Clear();
            _currentNavMesh = null;
            _currentZone = null;
            _logger.LogInformation("Navigation cache cleared");
        }

        public void RemoveZoneFromCache(string zoneName)
        {
            if (_navMeshes.TryRemove(zoneName, out _))
            {
                if (_currentZone == zoneName)
                {
                    _currentNavMesh = null;
                    _currentZone = null;
                }
                _logger.LogInformation("Removed nav mesh for zone from cache: {ZoneName}", zoneName);
            }
        }
    }
}