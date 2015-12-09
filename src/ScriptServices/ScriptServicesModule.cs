using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Json;

namespace ScriptServices
{
    public class ScriptServicesModule : NancyModule
    {
        private readonly ConfigSettings _settings;
        private static Dictionary<string, IEnumerable<string>> _queryEndpoints;

        public ScriptServicesModule(ConfigSettings settings)
        {
            _settings = settings;
            JsonSettings.MaxJsonLength = 5000000;
            JsonSettings.RetainCasing = true;

            Get["/admin"] = _ =>
                Response.AsJson("Hello Admin!");

            Get["/script/^(?<route>.*)$"] = parameters =>
            {
                var subPath = ((string)parameters["route"]);
                var script = string.Format("{0}.ps1", Path.Combine(_settings.ScriptRepoRoot, subPath));
                if ( !File.Exists(script) )
                {
                    return HttpStatusCode.NotFound;
                }

                var scriptArgs = new Dictionary<string, string>();
                foreach (var q in Request.Query.Keys)
                {
                    scriptArgs.Add(q, Request.Query[q].Value);
                }

                var res = PowerShellRunner.Execute(script, scriptArgs);
                
                var retCode = HttpStatusCode.OK;
                if (!res.Success)
                {
                    return Response.AsJson(new { Error = res.Output }, HttpStatusCode.InternalServerError);
                }
                
                return Response.AsJson(res.Output);
            };
        }
    }
}