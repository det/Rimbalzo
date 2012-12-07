using System;
using System.Net;
using System.Net.Sockets;

namespace Shared
{
	public abstract class Connecter
	{
		public abstract void ConnectFailed(string message);
		public abstract void Connected(TcpClient client);
		
		public TcpClient connection;

        public Connecter(string hostname, int port)
        {			
            try {
                connection = new TcpClient();
                connection.BeginConnect(hostname, port, HandleConnected, null);
            }
            catch (Exception e) {
                ConnectFailed(e.ToString());
            }
        }
		
		private void HandleConnected(IAsyncResult res)
        {
            try {
                connection.EndConnect(res);
            }
            catch (Exception e) {
                ConnectFailed(e.ToString());
                return;
            }

			Connected(connection);
        }

	}
}

