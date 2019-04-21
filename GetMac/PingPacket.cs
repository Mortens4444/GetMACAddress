/*using System;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;

namespace GetMac
{
    public sealed class PingPacket : IDisposable
    {
        public delegate void PingReplyArrivedEventHandler(object sender, PingReplyArrivedEventArgs e);

        public bool ShowMessages { get; set; }
        public event PingReplyArrivedEventHandler PingReplyArrived;

        Ping ping;

        public PingPacket(byte[] ipAddress, int timeout = 10, PingReplyArrivedEventHandler PingReplyArrivedHandler = null)
            : this(ByteArrayToIpAddress(ipAddress), timeout, PingReplyArrivedHandler)
        { }

        public PingPacket(string ipAddress, int timeout = 10, PingReplyArrivedEventHandler PingReplyArrivedHandler = null)
        {
            if (PingReplyArrivedHandler != null)
            {
                PingReplyArrived += PingReplyArrivedHandler;
            }
            Initialize(ipAddress, timeout);
        }

        private static string ByteArrayToIpAddress(byte[] ipAddress)
        {
            var ip = new StringBuilder();
            for (var i = 0; i < ipAddress.Length; i++)
            {
                ip.Append(ipAddress[i].ToString());
                if (i < ipAddress.Length - 1)
                {
                    ip.Append('.');
                }
            }
            return ip.ToString();
        }

        private void Initialize(string IP_Address, int timeout)
        {
            try
            {
                ping = new Ping();
                ping.PingCompleted += PingResult;
                ping.SendAsync(IP_Address, timeout, new byte[] { 65, 65, 65, 65 }, null);
            }
            catch
            {
                Dispose();
            }
        }

        ~PingPacket()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        int cleanup;
        void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref cleanup, 1) != 0)
            {
                return;
            }

            if (ping != null)
            {
                ((IDisposable)ping).Dispose();
            }
        }

        private void OnPingResultArrived(PingReplyArrivedEventArgs e)
        {
            var handler = PingReplyArrived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void PingResult(object sender, PingCompletedEventArgs e)
        {
            OnPingResultArrived(new PingReplyArrivedEventArgs(e.Reply, ShowMessages));
            ((IDisposable)ping).Dispose();
        }
    }
}
*/