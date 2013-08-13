using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace SimpleMoney.Controllers
{
     public class HomeController : Controller
     {
          public ActionResult Index()
          {
               ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

               Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                  .SetXAxis(new XAxis
                  {
                       Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                  })
                  .SetSeries(new Series
                  {
                       Data = new Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })
                  });

               ViewBag.chart = chart;

               return View();
          }

          public ActionResult About()
          {
               ViewBag.Message = "Your app description page.";

               return View();
          }

          public ActionResult Contact()
          {
               ViewBag.Message = "Your contact page.";

               return View();
          }
     }
}
