using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenEQ.Netcode.GameClient.Maps
{
    public class MapLine
    {
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int FromZ { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }
        public int ToZ { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }

    public class MapData
    {
        public string ZoneName { get; set; }
        public List<MapLine> Lines { get; set; } = new List<MapLine>();
        public int MaxX { get; set; } = int.MinValue;
        public int MaxY { get; set; } = int.MinValue;
        public int MinX { get; set; } = int.MaxValue;
        public int MinY { get; set; } = int.MaxValue;
    }

    public static class MapReader
    {
        private static string GetBasePath()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(assemblyLocation);
            
            // First try relative to assembly location
            var relativePath = Path.Combine(assemblyDir, "GameClient", "Maps", "2D");
            if (Directory.Exists(relativePath))
                return relativePath;
                
            // Fallback: try to find the Maps directory in the project structure
            var currentDir = assemblyDir;
            while (currentDir != null && !Directory.Exists(Path.Combine(currentDir, "GameClient", "Maps", "2D")))
            {
                currentDir = Directory.GetParent(currentDir)?.FullName;
            }
            
            if (currentDir != null)
                return Path.Combine(currentDir, "GameClient", "Maps", "2D");
                
            // Final fallback - just use the relative path
            return relativePath;
        }
        
        private static readonly string BasePath = GetBasePath();

        public static MapData ReadMapFile(string zoneName)
        {
            var mapData = new MapData { ZoneName = zoneName };
            
            try
            {
                // All map files use file system to allow user customization
                var filePath = Path.Combine(BasePath, $"{zoneName}.txt");
                if (!File.Exists(filePath))
                {
                    return null;
                }
                
                var lines = File.ReadAllLines(filePath);
                
                foreach (var line in lines)
                {
                    var mapLine = ParseMapLine(line);
                    if (mapLine != null)
                    {
                        mapData.Lines.Add(mapLine);
                        
                        // Calculate bounds like the original EQMap
                        mapData.MaxX = Math.Max(mapData.MaxX, Math.Max(mapLine.FromX, mapLine.ToX));
                        mapData.MaxY = Math.Max(mapData.MaxY, Math.Max(mapLine.FromY, mapLine.ToY));
                        mapData.MinX = Math.Min(mapData.MinX, Math.Min(mapLine.FromX, mapLine.ToX));
                        mapData.MinY = Math.Min(mapData.MinY, Math.Min(mapLine.FromY, mapLine.ToY));
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return mapData;
        }

        public static List<string> GetAvailableZones()
        {
            var zones = new List<string>();
            
            try
            {
                // All map files use file system to allow user customization
                if (Directory.Exists(BasePath))
                {
                    zones = Directory.GetFiles(BasePath, "*.txt")
                        .Select(Path.GetFileNameWithoutExtension)
                        .Where(name => !name.Contains("_")) // Filter out variant files like zone_1.txt
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

        public static List<string> GetZoneVariants(string zoneName)
        {
            var variants = new List<string>();
            
            try
            {
                // All map files use file system to allow user customization
                if (Directory.Exists(BasePath))
                {
                    var pattern = $"{zoneName}_*.txt";
                    variants = Directory.GetFiles(BasePath, pattern)
                        .Select(Path.GetFileNameWithoutExtension)
                        .OrderBy(name => name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                // Return empty list on error
            }

            return variants;
        }

        public static bool MapExists(string zoneName)
        {
            try
            {
                // All map files use file system to allow user customization
                var filePath = Path.Combine(BasePath, $"{zoneName}.txt");
                return File.Exists(filePath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static MapLine ParseMapLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("L "))
            {
                return null;
            }

            var parts = line.Substring(2).Split(',');
            if (parts.Length != 9)
            {
                return null;
            }

            try
            {
                return new MapLine
                {
                    FromX = Convert.ToInt32(parts[0].Split('.')[0].Trim()),
                    FromY = Convert.ToInt32(parts[1].Split('.')[0].Trim()),
                    FromZ = Convert.ToInt32(parts[2].Split('.')[0].Trim()),
                    ToX = Convert.ToInt32(parts[3].Split('.')[0].Trim()),
                    ToY = Convert.ToInt32(parts[4].Split('.')[0].Trim()),
                    ToZ = Convert.ToInt32(parts[5].Split('.')[0].Trim()),
                    Red = byte.Parse(parts[6].Trim()),
                    Green = byte.Parse(parts[7].Trim()),
                    Blue = byte.Parse(parts[8].Trim())
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}