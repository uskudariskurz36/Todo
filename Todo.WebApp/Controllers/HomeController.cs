using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using Todo.WebApp.Models;

namespace Todo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            RestClient client = new RestClient("http://localhost:5086");
            RestRequest request = new RestRequest("/Todo/List", Method.Get);

            List<Models.Todo> todos = client.Get<List<Models.Todo>>(request);

            return View(todos);
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