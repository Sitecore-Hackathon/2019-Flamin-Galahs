using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.Pipelines;

namespace FlaminGalahs.Feature.FGSMS
{
    public class RegisterAPIRoutes
    {

        public void Process(PipelineArgs args)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;

            SetRoutes(config);
            SetSerializerSettings(config);
        }

        private void SetRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("TwilioListener", 
                "FGapi/listener/{action}", new {  controller = "TwilioListener" });
            
        }

        private void SetSerializerSettings(HttpConfiguration config)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            config.Formatters.JsonFormatter.SerializerSettings = settings;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.EnsureInitialized();
        }

    }
}
