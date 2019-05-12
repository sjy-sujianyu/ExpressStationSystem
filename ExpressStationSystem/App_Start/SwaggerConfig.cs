using System.Web.Http;
using WebActivatorEx;
using ExpressStationSystem;
using Swashbuckle.Application;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ExpressStationSystem
{

    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.OperationFilter<TokenFilter>();
                    c.OperationFilter<UploadFilter>();
                    c.SingleApiVersion("v1", "Swagger");
                    c.IncludeXmlComments(GetXmlCommentsPath());

                })
                .EnableSwaggerUi(c => 
                {
                    c.DocumentTitle("ϵͳ�����ӿ�");
                        // ʹ������
                        c.InjectJavaScript(thisAssembly, "Swagger.Scripts.Swagger.swagger_lang.js");
                });
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/ExpressStationSystem.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }

}
