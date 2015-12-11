using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ScriptServices.powershell
{
    public class PowerShellRunner
    {
        public static ScriptResult Execute(string method, string scriptPath, IDictionary<string,string> scriptArguments)
        {
            string workingDirectory = Path.GetTempPath();

            var launcherScript = Path.Combine(workingDirectory, string.Format("SSLaunch-{0}.ps1.", Guid.NewGuid()));

            // generate a script that will invoke our script with all the required parameters
            using (var writer = new StreamWriter(launcherScript))
            {
                // provide a fingerprint so scripts know when they are running inside of ScriptServices, should they need to
                writer.WriteLine("$env:SCRIPTSERVICES_VERSION = '{0}'", Assembly.GetExecutingAssembly().GetName().Version);

                // mock-out logging functions that could pollute the response stream, this still
                // allows them to be used in scripts to help with interactive debugging etc.
                writer.WriteLine("function Write-Host {}");
                writer.WriteLine("function Write-Verbose {}");
                writer.WriteLine("function Write-Debug {}");

                // dot source the script so it is execute in the same scope as this launcher script
                writer.Write(string.Format(@". '{0}' ", scriptPath));

                // scripts should have a parameter to accept the HTTP request method, even if they do nothing with it
                writer.Write(string.Format("-httpVerb \"{0}\"", method));

                // construct the script parameters
                foreach (var key in scriptArguments.Keys)
                {
                    writer.Write(string.Format("-{0} \"{1}\"", key, scriptArguments[key]));
                }

                writer.Flush();
            }

            try
            {
                var commandArguments = new StringBuilder();
                commandArguments.Append("-NonInteractive ");
                commandArguments.Append("-NoLogo ");
                commandArguments.Append("-ExecutionPolicy Unrestricted ");
                commandArguments.Append("-NoProfile ");
                commandArguments.AppendFormat("-File \"{0}\"", launcherScript);


                var posh = new PowerShellScriptExecutor(commandArguments.ToString());
                var res = posh.Execute();

                return res;
            }
            finally
            {
                File.Delete(launcherScript);
            }
        }
    }
}