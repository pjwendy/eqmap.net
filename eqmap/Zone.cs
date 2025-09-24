using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eqmap
{
    public class Zone
    {
        EQGameClient gameClient;

        public Zone(EQGameClient gameClient) 
        { 
            this.gameClient = gameClient; 
        }
        
        public string CurrentZoneName => gameClient?.CurrentZone?.Name ?? "";
        public int CurrentZoneId => (int)(gameClient?.CurrentZone?.ZoneID ?? 0);
        
        // Add zone-specific methods as needed
        public void RequestZone(string zoneName)
        {
            // This would need to be implemented in EQGameClient if zone requests are supported
            // For now, zone changes happen through the server
        }
        
        public async Task<bool> MoveTo(float x, float y, float z)
        {
            if (gameClient == null)
            {
                return false;
            }
            return await gameClient.MoveTo(x, y, z);
        }
        
        public void MoveTo(double x, double y, double z)
        {
            // Synchronous wrapper for Lua convenience (Lua uses doubles)            
            Task.Run(async () => await MoveTo((float)x, (float)y, (float)z));
        }
        
        public void StopMovement()
        {
            if (gameClient != null)
            {
                gameClient.StopMovement();
            }
        }
    }
}
