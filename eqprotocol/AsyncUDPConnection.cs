﻿using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpenEQ.Netcode {
    public class AsyncUDPConnection {
        UdpClient client;
        string host;
        int port;

        public AsyncUDPConnection(string host, int port) {
            this.host = host;
            this.port = port;
            client = new UdpClient(host, port);
        }

        public async Task<byte[]> Receive() {
            while(true) {
                var result = await client.ReceiveAsync();
                // XXX: Add check that this is actually the right sender
                return result.Buffer;
            }
        }

        public async void Send(byte[] packet) {
            await client.SendAsync(packet, packet.Length);
        }
    }
}
