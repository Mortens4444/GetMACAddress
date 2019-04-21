using System;
using System.Net;

namespace GetMac
{
    public class HostNameProvider
    {
        public string GetLinuxLocalHostName()
        {
            var shellScriptExecutor = new ShellScriptExecutor();
            var unameResult = shellScriptExecutor.GetCommandResult(CommandStrings.UNAME);
            if (unameResult.HasSucceeded)
            {
                return unameResult.Output;
            }
            return String.Empty;
        }

        public string GetHostName(string ipAddress)
        {
            var operatingSystem = Environment.OSVersion;
            var platform = operatingSystem.Platform;
            if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
            {
                var hostInfo = Dns.GetHostEntry(ipAddress);
                return hostInfo.HostName;
            }

            if (!String.IsNullOrEmpty(ipAddress))
            {
                try
                {
                    IPHostEntry hostInfo;
                    try
                    {
                        if (Environment.OSVersion.Version.Major > 5) // Windows Vistától
                        {
                            hostInfo = Dns.GetHostEntry(ipAddress);
                        }
                        else
                        {
                            #pragma warning disable 618
                            hostInfo = Dns.GetHostByAddress(ipAddress);
                            #pragma warning restore 618
                        }
                    }
                    catch
                    {
                        hostInfo = Dns.GetHostEntry(ipAddress);
                    }
                    return hostInfo.HostName;
                }
                catch { }
            }
            return ipAddress;
        }
    }
}

