using System;
using System.Diagnostics;

namespace GetMac
{
    public class ShellScriptExecutor
    {
        public CommndExecutionResult GetCommandResult(string command, string arguments = "")
        {
            var process = new Process();
            process.StartInfo.FileName = "awk";
            process.StartInfo.Arguments = String.Concat("'BEGIN{system(\"", command, "\")}'");
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            return new CommndExecutionResult(process.StandardError.ReadToEnd(), process.StandardOutput.ReadToEnd());
        }
    }
}

