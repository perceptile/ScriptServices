using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Nancy.Hosting.Wcf;

namespace ScriptServices.hosting
{
    static internal class AnonymousServiceHost
    {
        public static void Create()
        {
            var settings = new ConfigSettings();

            var anonHost = new WebServiceHost(new NancyWcfGenericService(new AnonymousBootstrapper(settings)), new Uri(settings.HostUri));
            anonHost.AddServiceEndpoint(typeof (NancyWcfGenericService), new WebHttpBinding(), string.Empty);
            anonHost.Open();
        }
    }


}