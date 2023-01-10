using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Todo.WebApp.Models;

namespace Todo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        RestClient client = new RestClient("http://localhost:5086");

        public IActionResult Index()
        {
            RestRequest request = new RestRequest("/Todo/List", Method.Get);

            List<Models.Todo> todos = client.Get<List<Models.Todo>>(request);

            return View(todos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoCreate model)
        {
            if (ModelState.IsValid)
            {
                RestRequest request = new RestRequest("/Todo/Create", Method.Post);
                request.AddJsonBody(model);

                RestResponse<Models.Todo> response = client.ExecutePost<Models.Todo>(request);
                //Models.Todo todo = client.Post<Models.Todo>(request);

                Models.Todo todo = response.Data;

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

        public IActionResult Edit(int id)
        {
            RestRequest request = new RestRequest($"/Todo/GetById/{id}", Method.Get);
            RestResponse<Models.Todo> response = client.ExecuteGet<Models.Todo>(request);

            if (response.IsSuccessful == false)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(response.Data);
        }

        [HttpPost]
        public IActionResult Edit(int id, Models.Todo model)
        {
            if (ModelState.IsValid)
            {
                RestRequest request = new RestRequest($"/Todo/Edit/{id}", Method.Put);
                request.AddJsonBody(model);

                RestResponse<Models.Todo> response = client.ExecutePut<Models.Todo>(request);

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