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
using ExpressStationSystem.Controllers;
using System.Diagnostics;

namespace ExpressStationSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            int sleepTime = 1800000;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Text();
            //moni();
            Thread thread = new Thread(new ParameterizedThreadStart(new MoneyController().ErrorPost));
            thread.IsBackground = true;
            thread.Start(sleepTime);

        }
        public void moni()
        {
            //Thread newlogin = new Thread(Simulation.Instance.AddNewLogin);
            //newlogin.IsBackground = true;
            //newlogin.Start();

            //Thread newaddress = new Thread(Simulation.Instance.AddNewAddress);
            //newaddress.IsBackground = true;
            //newaddress.Start();

            Thread newpath = new Thread(Simulation.Instance.NewPath);
            newpath.IsBackground = true;
            newpath.Start();

            //Thread newpackage = new Thread(new ParameterizedThreadStart(Simulation.Instance.OrderNewPackage));
            //newpackage.IsBackground = true;
            //newpackage.Start(3000);
        }
        public void Text()
        {
            DateTime date1 = new DateTime(2008, 6, 15, 21, 15, 07);
            DateTime date2 = new DateTime(2019, 6, 15, 21, 15, 07);
            List<int> list = new ManagerController().GetAllPackage(date1, date2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            new QueryController().GetAllInfoFast(date1,date2);
            //耗时巨大的代码

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);

            
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
