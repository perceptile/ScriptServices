using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptServices
{
    public class PowerShellScriptExecutor
    {
        const string PowerShellExe = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";

        public PowerShellScriptExecutor(string arguments)
        {
            this.Arguments = arguments;
        }

        public string Arguments { get; set; }

        public ScriptResult Execute()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = PowerShellExe,
                Arguments = this.Arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();

            process.WaitForExit();

            // TODO: Sort out proper output & error stream handling
            string results = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(errors))
            {
                return new ScriptResult { Success = false, Output = errors };
            }
            
            return new ScriptResult { Success = true, Output = results };
        }
    }

    
}
