using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Net.Sockets;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GetMac
{
    public class WindowsMacAddressProvider
    {
        [DllImport("Iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        const string IPV4_ADDRESS_FORMAT_IS_INCORRECT = "IPv4 address format is incorrect.";
        const int MAC_ADDRESS_LENGTH_IN_BYTES = 6;

        public string GetMacAddress(string ipOrHost)
        {
            if (String.IsNullOrEmpty(ipOrHost))
            {
                return String.Empty;
            }

            var ipAddress = IPAddress.Parse(ipOrHost);
            var mac = IpAddressToMacAddress(ipAddress);
            return PhysicalAddressToString(mac);
        }

        private static PhysicalAddress IpAddressToMacAddress(IPAddress ip)
        {
            // Check what family the IP is from <cref="http://msdn.microsoft.com/en-us/library/system.net.sockets.addressfamily.aspx"/>
            if (ip.AddressFamily != AddressFamily.InterNetwork)
            {
                throw new ArgumentException("The remote system only supports IPv4 addresses.");
            }

            var receiverIp = InetAddress(ip.ToString());
            var macByteArray = new byte[MAC_ADDRESS_LENGTH_IN_BYTES]; // 48 bit
            var length = (uint)macByteArray.Length;

            // Call the Win32 API SendArp <cref="http://msdn.microsoft.com/en-us/library/aa366358%28VS.85%29.aspx"/>
            var arpReply = SendARP(receiverIp, 0, macByteArray, ref length);
            if ((SystemErrorCodes)arpReply != SystemErrorCodes.SUCCESS)
            {
                switch ((SystemErrorCodes)arpReply)
                {
                    case SystemErrorCodes.ERROR_GEN_FAILURE:
                        throw new Exception("A device attached to the system is not functioning. This error is returned on Windows Server 2003 and earlier when an ARP reply to the SendARP request was not received. This error can occur if destination IPv4 address could not be reached because it is not on the same subnet or the destination computer is not operating.");
                    case SystemErrorCodes.ERROR_INVALID_PARAMETER:
                        throw new Exception("One of the parameters is invalid. This error is returned on Windows Server 2003 and earlier if either the pMacAddr or PhyAddrLen parameter is a NULL pointer.");
                    case SystemErrorCodes.ERROR_INVALID_USER_BUFFER:
                        throw new Exception("The supplied user buffer is not valid for the requested operation. This error is returned on Windows Server 2003 and earlier if the ULONG value pointed to by the PhyAddrLen parameter is zero.");
                    case SystemErrorCodes.ERROR_BAD_NET_NAME:
                        throw new Exception("The network name cannot be found. This error is returned on Windows Vista and later when an ARP reply to the SendARP request was not received. This error occurs if the destination IPv4 address could not be reached.");
                    case SystemErrorCodes.ERROR_BUFFER_OVERFLOW:
                        throw new Exception("The file name is too long. This error is returned on Windows Vista if the ULONG value pointed to by the PhyAddrLen parameter is less than 6, the size required to store a complete physical address.");
                    case SystemErrorCodes.ERROR_NOT_FOUND:
                        throw new Exception("Element not found. This error is returned on Windows Vista if the the SrcIp parameter does not specify a source IPv4 address on an interface on the local computer or the INADDR_ANY IP address (an IPv4 address of 0.0.0.0).");
                    default:
                        throw new Win32Exception(arpReply);
                }
            }           

            return new PhysicalAddress(macByteArray);
        }

        // 1.2.3.4 . 0x04030201
        private static uint InetAddress(string ipAddress)
        {
            var ipParts = ipAddress.Split('.');
            if (ipParts.Length != 4)
            {
                throw new ArgumentException(IPV4_ADDRESS_FORMAT_IS_INCORRECT, "ipAddress");
            }
            var ipPart1 = Convert.ToByte(ipParts[0]);
            var ipPart2 = Convert.ToByte(ipParts[1]);
            var ipPart3 = Convert.ToByte(ipParts[2]);
            var ipPart4 = Convert.ToByte(ipParts[3]);
            return (uint)(ipPart1 + ipPart2 * 256 + ipPart3 * 65536 + ipPart4 * 16777216);
        }

        private static string PhysicalAddressToString(PhysicalAddress mac, char? separator = ':')
        {
            var macString = mac.ToString();
            if (!separator.HasValue)
            {
                return macString.ToUpper();
            }
            var friendlyMac = new StringBuilder();
            for (var i = 0; i < macString.Length; i += 2)
            {
                if (i > 0)
                {
                    friendlyMac.Append(separator);
                }
                friendlyMac.Append(macString.Substring(i, 2).ToUpper());
            }

            return friendlyMac.ToString();
        }
    }
}
