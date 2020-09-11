using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.IO;
using Web.Pdf;

namespace Web.Controllers
{
    public class DivisionsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44374/api/")
        };

        public IActionResult Index()
        {
            return View("~/Views/Dashboard/Division.cshtml");
        }

        public IActionResult LoadDiv()
        {
            IEnumerable<Division> divisions = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("divisions");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Division>>();
                readTask.Wait();
                divisions = readTask.Result;
            }
            else
            {
                divisions = Enumerable.Empty<Division>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(divisions);

        }

        public IActionResult GetById(int Id)
        {
            Division divisions = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("divisions/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                divisions = JsonConvert.DeserializeObject<Division>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(divisions);
        }

        public IActionResult InsertOrUpdate(Division divisions, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(divisions);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var token = HttpContext.Session.GetString("token");
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (divisions.Id == 0)
                {
                    var result = client.PostAsync("divisions", byteContent).Result;
                    return Json(result);
                }
                else if (divisions.Id == id)
                {
                    var result = client.PutAsync("divisions/" + id, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id)
        {
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var result = client.DeleteAsync("divisions/" + id).Result;
            return Json(result);
        }


        public async Task<IActionResult> Excel()
        {
            IEnumerable<Division> divisions = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("divisions");
            resTask.Wait();
            var result = resTask.Result;
            var readTask = result.Content.ReadAsAsync<List<Division>>();
            readTask.Wait();
            divisions = readTask.Result;
            //return Json(divisions);
            //List<Division> divisions = new List<Division>();
            //HttpResponseMessage resView = await client.GetAsync("divisions");
            //var resultView = resView.Content.ReadAsStringAsync().Result;
            //divisions = JsonConvert.DeserializeObject<List<Division>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("divisions");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Id";
                worksheet.Cell(currentRow, 3).Value = "Department Name";
                worksheet.Cell(currentRow, 4).Value = "Created Date";
                worksheet.Cell(currentRow, 5).Value = "Updated Date";

                foreach (var form in divisions)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = form.Id;
                    worksheet.Cell(currentRow, 3).Value = form.department.Name;
                    worksheet.Cell(currentRow, 4).Value = form.CreateData;
                    worksheet.Cell(currentRow, 5).Value = form.UpdateDate;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Forms_Data.xlsx");
                }
            }
        }

        public ActionResult ExportPdf()
        {
            DivisionPdf formPdf = new DivisionPdf();
            List<Division> divisions = new List<Division>();

            //var resTask = client.GetAsync("divisions");
            //resTask.Wait();
            //var result = resTask.Result;

            //var readTask = result.Content.ReadAsAsync<List<Division>>();
            //readTask.Wait();
            //forms = readTask.Result;


            //IEnumerable<Division> divisions = null;
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", token);
            var resTask = client.GetAsync("divisions");
            resTask.Wait();
            var result = resTask.Result;
            var readTask = result.Content.ReadAsAsync<List<Division>>();
            readTask.Wait();
            divisions = readTask.Result;

            byte[] abytes = formPdf.Prepare(divisions);
            return File(abytes, "application/pdf");
        }
    }
}