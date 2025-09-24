using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenEQ.Netcode.GameClient.Maps
{
    public static class NavReader
    {
        private static string GetBasePath()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(assemblyLocation);
            
            // First try relative to assembly location
            var relativePath = Path.Combine(assemblyDir, "GameClient", "Maps", "Nav");
            if (Directory.Exists(relativePath))
                return relativePath;
                
            // Fallback: try to find the Maps directory in the project structure
            var currentDir = assemblyDir;
            while (currentDir != null && !Directory.Exists(Path.Combine(currentDir, "GameClient", "Maps", "Nav")))
            {
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }
            
            if (currentDir != null)
                return Path.Combine(currentDir, "GameClient", "Maps", "Nav");
                
            // Final fallback - just use the relative path
            return relativePath;
        }
        
        private static readonly string BasePath = GetBasePath();

        public static byte[] ReadNavFile(string zoneName)
        {
            try
            {
                // Nav files are too large for embedding, use file system only
                var filePath = Path.Combine(BasePath, $"{zoneName}.nav");
                if (!File.Exists(filePath))
                {
                    return null;
                }
                
                return File.ReadAllBytes(filePath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> GetAvailableNavZones()
        {
            var zones = new List<string>();
            
            try
            {
                // Nav files use file system only (too large for embedding)
                if (Directory.Exists(BasePath))
                {
                    zones = Directory.GetFiles(BasePath, "*.nav")
                        .Select(Path.GetFileNameWithoutExtension)
                        .OrderBy(name => name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                // Return empty list on error
            }

            return zones;
        }

        public static bool NavExists(string zoneName)
        {
            try
            {
                // Nav files use file system only (too large for embedding)
                var filePath = Path.Combine(BasePath, $"{zoneName}.nav");
                return File.Exists(filePath);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}