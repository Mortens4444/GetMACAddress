using System;
using System.Windows.Forms;

namespace GetMac
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: {0} ipAddress", Application.ProductName);
                return;
            }

            var ipAddress = args[0];
            Console.WriteLine("IP address: {0}", ipAddress);

            try
            {
                var hostNameProvider = new HostNameProvider();
                var hostname = hostNameProvider.GetHostName(ipAddress);
                Console.WriteLine("Hostname: {0}", hostname);

#if __MonoCS__
                var linuxMacAddressProvider = new LinuxMacAddressProvider();
                var macAddress = linuxMacAddressProvider.GetMacAddress(ipAddress);
#else

                var windowsMacAddressProvider = new WindowsMacAddressProvider();
                var macAddress = windowsMacAddressProvider.GetMacAddress(ipAddress);
#endif
                if (String.IsNullOrEmpty(macAddress))
                {
                    throw new Exception("MAC address could not be retrieved.");
                }

                Console.WriteLine("MAC address: {0}", macAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Console.ReadKey();
        }
    }
}
