using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using ExpressStationSystem.Models;

namespace ExpressStationSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Thread newlogin = new Thread(Simulation.Instance.AddNewLogin);
            //newlogin.IsBackground = true;
            //newlogin.Start();

            //Thread newaddress = new Thread(Simulation.Instance.AddNewAddress);
            //newaddress.IsBackground = true;
            //newaddress.Start();

            //Thread newpackage = new Thread(Simulation.Instance.OrderNewPackage);
            //newpackage.IsBackground = true;
            //newpackage.Start();

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var res = HttpContext.Current.Response;
            var req = HttpContext.Current.Request;

            //自定义header时进行处理
            if (req.HttpMethod == "OPTIONS")
            {
                res.StatusCode = 200;
                res.End();
            }
        }
    }
}
