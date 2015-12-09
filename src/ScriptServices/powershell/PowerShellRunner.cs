using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScriptServices
{
    public class PowerShellRunner
    {
        public static ScriptResult Execute(string scriptPath, IDictionary<string,string> scriptArguments)
        {
            string workingDirectory = Path.GetTempPath();

            var launcherFile = Path.Combine(workingDirectory, "Bootstrap." + Guid.NewGuid().ToString() + ".ps1");

            // generate a script that will invoke our script with all the required parameters
            using (var writer = new StreamWriter(launcherFile))
            {
                writer.Write(string.Format(@". '{0}' ", scriptPath));

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
                commandArguments.AppendFormat("-File \"{0}\"", launcherFile);


                var posh = new PowerShellScriptExecutor(commandArguments.ToString());
                var res = posh.Execute();

                return res;
            }
            finally
            {
                System.IO.File.Delete(launcherFile);
            }
        }
    }
}