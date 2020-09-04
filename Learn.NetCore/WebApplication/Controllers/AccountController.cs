using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Learn.NetCore.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult index()
        {
            return View();
        }

        HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44312/api/")
        };

        [Route("validate")]
        public IActionResult Validate(UserVM userVM)
        {
            if (userVM.UserName == null)
            {
                var jsonUserVM = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonUserVM);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resTask = client.PostAsync("users/login/", byteContent);
                resTask.Wait();
                var result = resTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    if (data != "")
                    {
                        var json = JsonConvert.DeserializeObject(data).ToString();
                        var account = JsonConvert.DeserializeObject<UserVM>(json);
                        if (BCrypt.Net.BCrypt.Verify(userVM.Password, account.Password) && (account.Roles == "Admin" || account.Roles == "Sales"))
                        {
                            //HttpContext.Session.SetString("id", account.Id);
                            //HttpContext.Session.SetString("uname", account.UserName);
                            //HttpContext.Session.SetString("email", account.Email);
                            //HttpContext.Session.SetString("lvl", account.Roles);
                            if (account.Roles == "Admin")
                            {
                                return Json(new { status = true, msg = "Login Successfully !", acc = "Admin" });
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Login Successfully !", acc = "Sales" });
                            }
                        }
                        else
                        {
                            return Json(new { status = false, msg = "Invalid Username or Password!" });
                        }
                    }
                    else
                    {
                        return Json(new { status = false, msg = "Username Not Found!" });
                    }
                }
                else
                {
                    //return RedirectToAction("Login","Auth");
                    return Json(new { status = false, msg = "Username Not Found!" });
                }
            }
            else if (userVM.UserName != null)
            {
                var json = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync("users/register/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { status = true, code = result, msg = "Register Success! " });
                }
                else
                {
                    return Json(new { status = false, msg = "Something Wrong!" });
                }
            }
            return Redirect("/login");
        }
    }
}