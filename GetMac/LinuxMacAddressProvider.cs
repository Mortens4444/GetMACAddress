using System;

namespace GetMac
{
    public class LinuxMacAddressProvider
    {
        public string GetMacAddress(string ipAddress)
        {
            Ping(ipAddress);

            var arpCommand = String.Format("{0} -a | grep {1}", CommandStrings.ARP, ipAddress);
            var shellScriptExecutor = new ShellScriptExecutor();
            var arpResult = shellScriptExecutor.GetCommandResult(arpCommand);
            if (arpResult.HasSucceeded)
            {
                var startIndex = arpResult.Output.IndexOf(" at ") + 4;
                var endIndex = arpResult.Output.IndexOf(' ', startIndex + 1);
                return arpResult.Output.Substring(startIndex, endIndex - startIndex).ToUpper().Replace(':', '-');
            }
            else
            {                
                Console.WriteLine(arpResult);
                return String.Empty;
            }
        }

        /*private static void PingBroadcast()
        {
            var shellScriptExecutor = new ShellScriptExecutor();
            var getBroadcastAddressCommand = String.Concat(CommandStrings.IFCONFIG, " | grep broadcast");
            var getBroadcastAddressCommandResult = shellScriptExecutor.GetCommandResult(getBroadcastAddressCommand);
            if (!getBroadcastAddressCommandResult.HasSucceeded)
            {
                throw new InvalidOperationException(getBroadcastAddressCommandResult.Error);
            }
            var outputLines = getBroadcastAddressCommandResult.Output.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var outputLine in outputLines)
            {
                var broadcastAddress = outputLine.Substring(outputLine.IndexOf("broadcast ") + 10);
                Ping(broadcastAddress);
            }
        }*/

        private static void Ping(string ipAddress)
        {
            var shellScriptExecutor = new ShellScriptExecutor();
            Console.WriteLine("Ping: {0}", ipAddress);            
            var pingCommand = String.Concat("ping -b -c 1 ", ipAddress);
            var pingResult = shellScriptExecutor.GetCommandResult(pingCommand);
            if (!pingResult.HasSucceeded)
            {
                throw new InvalidOperationException(pingResult.Error);
            }
            Console.WriteLine(pingResult);
        }
    }
}
