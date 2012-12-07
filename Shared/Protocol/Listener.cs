using System;
using System.Net;
using System.Net.Sockets;

namespace Shared
{
	public abstract class Listener
	{		
		public abstract void AcceptFailed(string message);
		public abstract void Accepted(TcpClient client);
		
		public TcpListener listener;
		
		public Listener (IPAddress address, int port)
		{	
			listener = new TcpListener(address, port);
			listener.Start();
			listener.BeginAcceptTcpClient(HandleAccepted, null);
		}
		
		private void HandleAccepted(IAsyncResult res)
        {
            try {
                var client = listener.EndAcceptTcpClient(res);
				Accepted(client);
				listener.BeginAcceptTcpClient(HandleAccepted, null);
            }
            catch (Exception e) {
                Stop();
                AcceptFailed(e.ToString());
                return;
            }
        }
		
		public void Stop()
		{
			listener.Stop();
		}
		
	}
}

