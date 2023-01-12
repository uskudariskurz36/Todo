using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Todo.WebApp.Filters;
using Todo.WebApp.Managers;
using Todo.WebApp.Models;

namespace Todo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ITodoService _todoService;

        public HomeController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [AuthFilter]
        public IActionResult Index()
        {
            //RestRequest request = new RestRequest("/Todo/List", Method.Get);

            //List<Models.Todo> todos = client.Get<List<Models.Todo>>(request);

            RestResponse<List<Models.Todo>> response = _todoService.List();

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [AuthFilter]
        public IActionResult Create()
        {
            return View();
        }

        [AuthFilter]
        [HttpPost]
        public IActionResult Create(TodoCreate model)
        {
            if (ModelState.IsValid)
            {
                //RestRequest request = new RestRequest("/Todo/Create", Method.Post);
                //request.AddJsonBody(model);

                //RestResponse<Models.Todo> response = client.ExecutePost<Models.Todo>(request);
                ////Models.Todo todo = client.Post<Models.Todo>(request);

                //Models.Todo todo = response.Data;

                RestResponse<Models.Todo> response = _todoService.Create(model);
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError("", "Servis erişim hatası.");
                    //ModelState.AddModelError("", response.ErrorMessage);

                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [AuthFilter]
        public IActionResult Edit(int id)
        {
            //RestRequest request = new RestRequest($"/Todo/GetById/{id}", Method.Get);
            //RestResponse<Models.Todo> response = client.ExecuteGet<Models.Todo>(request);

            RestResponse<Models.Todo> response = _todoService.GetById(id);
            if (response.IsSuccessful == false)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(response.Data);
        }

        [AuthFilter]
        [HttpPost]
        public IActionResult Edit(int id, Models.Todo model)
        {
            if (ModelState.IsValid)
            {
                //RestRequest request = new RestRequest($"/Todo/Edit/{id}", Method.Put);
                //request.AddJsonBody(model);

                //RestResponse<Models.Todo> response = client.ExecutePut<Models.Todo>(request);

                RestResponse<Models.Todo> response = _todoService.Update(id, model);
                if (response.IsSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme yapılamadı(servis hatası).");
                }
            }

            return View(model);
        }

        [AuthFilter]
        public IActionResult Delete(int id)
        {
            //RestRequest request = new RestRequest($"/Todo/Remove/{id}", Method.Delete);
            //RestResponse response = client.Execute(request);

            RestResponse response =_todoService.Delete(id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["result"] = "Kayıt bulunamadı.";
            }
            else
            {
                TempData["result"] = "Kayıt silindi.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                // gelen modeldeki bilgiler ile aPI ye istek yapılır ve token alınmaya çalışılır.
                RestResponse<string> response = _todoService.Authenticate(model);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", "Kullanıcı adı ya da şifre hatalı.");
                }
                else
                {
                    if (response.IsSuccessful)
                    {
                        string token = response.Data;
                        HttpContext.Session.SetString("token", token);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "API Servisi hatası.");
                    }
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}