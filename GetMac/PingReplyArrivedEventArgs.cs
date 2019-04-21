/*using System;
using System.Net.NetworkInformation;

namespace GetMac
{
    public class PingReplyArrivedEventArgs : EventArgs
    {
        public string Sender { get; private set; }

        public PingReply PingReply { get; private set; }

        public bool Success { get; private set; }

        public bool ShowStatusMessages { get; private set; }

        public string StatusMessage { get; private set; }

        public PingReplyArrivedEventArgs(PingReply pingReply, bool showStatusMessages = false)
        {
            PingReply = pingReply;
            ShowStatusMessages = showStatusMessages;

            if ((pingReply != null) && (pingReply.Address != null))
            {
                Sender = pingReply.Address.ToString();
                Success = pingReply.Status == IPStatus.Success;
                StatusMessage = pingReply.Status.GetIpStatusDescription();
            }
            else
            {
                Sender = null;
                Success = false;
                StatusMessage = IPStatus.Unknown.GetIpStatusDescription();
            }
        }

        public override string ToString()
        {
            return StatusMessage;
        }
    }
}
*/