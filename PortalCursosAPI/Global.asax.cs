using Newtonsoft.Json;
using System.Globalization;
using System.Web.Http;

namespace PortalCursosAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Culture = CultureInfo.GetCultureInfo("pt-br"),
                DateFormatString = "dd/MM/yyyy"
            };
        }
    }
}
