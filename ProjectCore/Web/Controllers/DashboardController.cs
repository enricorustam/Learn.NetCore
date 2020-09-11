using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using API.ViewModels;
using System.Net.Http;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44374/api/")
        };
        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable)
            {
                if (HttpContext.Session.GetString("lvl") == "Admin")
                {
                    return View();
                }
                return Redirect("/profile");
                
            }
            //if (HttpContext.Session.GetString("lvl") == "Sales")
            //{
            //    return View();
            //}
            return RedirectToAction("Login","Auth");
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult LoadPie()
        {
            IEnumerable<PieChartVM> departments = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("charts/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                departments = readTask.Result;
            }
            else
            {
                departments = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(departments);

        }

        public IActionResult LoadBar()
        {
            IEnumerable<PieChartVM> bar = null;
            //var token = HttpContext.Session.GetString("token");
            //client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("charts/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                bar = readTask.Result;
            }
            else
            {
                bar = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(bar);
        }
    }
}
