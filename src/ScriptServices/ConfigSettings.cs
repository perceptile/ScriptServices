using System.Configuration;

namespace ScriptServices
{
    public class ConfigSettings
    {
        public string HostUri { get; set; }
        public string ScriptRepoRoot { get; set; }

        public ConfigSettings()
        {
            HostUri = ConfigurationManager.AppSettings["hostUri"];
            ScriptRepoRoot = ConfigurationManager.AppSettings["scriptRepoRoot"];
        }
    }
}
