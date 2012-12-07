using System;
using System.Net;
using System.Net.Sockets;

namespace Shared
{    
    public abstract class Connection
    {
		public abstract void Disconnected(string message);
		public abstract void Received();
		
        public TcpClient connection;
		
		public byte[] Buffer;
		public int BufferSize;
 
		public Connection(TcpClient client)
		{
			Buffer = new byte[4096];
			BufferSize = 0;
			connection = client;
			Read();
		}

		public void Close()
        {
            try { connection.Close(); }
            catch {}
        }		
        
        private void Read()
        {
			if (BufferSize == Buffer.Length) return;
            
            try {
                connection.GetStream().BeginRead(Buffer, BufferSize, Buffer.Length - BufferSize, HandleRead, null);
            }
            catch (Exception e) {
                Close();
                Disconnected(e.ToString());
            }
        }
        
        private void HandleRead(IAsyncResult res)
        {
            try {
                int read = connection.GetStream().EndRead(res);
                if (read == 0) {
                    Close();
                    Disconnected("");
                    return;
                }
				BufferSize += read;
            }
            catch (Exception e) {
                Close();
                Disconnected(e.ToString());
                return;
            }
            
			Received();
			Read();

        }
        
        public void Write(Byte[] data)
        {
            try {
                connection.GetStream().BeginWrite(data, 0, data.Length, HandleWrite, null);
            }
            catch (Exception e) {
                Close();
                Disconnected(e.ToString());
            }
        }

        public void Write(String data)
        {
            var bytes = new System.Text.ASCIIEncoding().GetBytes(data);
            Write(bytes);
        }

        private void HandleWrite(IAsyncResult res)
        {
            try {
                connection.GetStream().EndWrite(res);
            }
            catch (Exception e) {
                Close();
                Disconnected(e.ToString());
                return;
            }            
        }

    }
}
