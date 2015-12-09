using System;
using System.IO;
using log4net;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

using LogManager = ScriptServices.LogManager;

namespace ScriptServices.hosting
{
    public class AnonymousBootstrapper : DefaultNancyBootstrapper
    {
        private readonly ConfigSettings _settings;

        public AnonymousBootstrapper(ConfigSettings settings)
        {
            _settings = settings;
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((nancyContext, exception) =>
            {
                container.Resolve<ILog>().Error("Unhandled and logged at the end of the pipeline", exception);
                return HttpStatusCode.InternalServerError;
            });
            base.RequestStartup(container, pipelines, context);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register(_settings);
            container.Register(LogManager.Logger);
            //container.Register<ConfigRepoSync>().AsSingleton();
            
            //if (_synchronise)
            //{
            //    container.Resolve<ConfigRepoSync>().Synchronise();
            //}
            //else
            //{
            //    container.Resolve<ConfigRepoSync>().Start();
            //}

            pipelines.OnError.AddItemToEndOfPipeline((nancyContext, exception) =>
            {              
                container.Resolve<ILog>().Error(string.Format("Unhandled and logged at the end of the pipeline"), exception);
                return HttpStatusCode.InternalServerError;
            });

            pipelines.AfterRequest += ctx => ctx.Response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

            base.ApplicationStartup(container, pipelines);
        }
    }
}