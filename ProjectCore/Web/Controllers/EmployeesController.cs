using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class EmployeesController : Controller
    {

        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44374/api/")
        };

        public IActionResult Index()
        {
            return View("~/Views/Dashboard/Employee.cshtml");
        }

        public IActionResult LoadEmp()
        {
            IEnumerable<EmployeeVM> employeeVMs = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("employees");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<EmployeeVM>>();
                readTask.Wait();
                employeeVMs = readTask.Result;
            }
            else
            {
                employeeVMs = Enumerable.Empty<EmployeeVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(employeeVMs);

        }

        public IActionResult GetById(string Id)
        {
            EmployeeVM employeeVM = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("employees/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(employeeVM);
        }


        public IActionResult Delete(string id)
        {
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var result = client.DeleteAsync("employees/" + id).Result;
            return Json(result);
        }


    }
}