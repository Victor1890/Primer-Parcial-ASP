using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PrimerParcial.Models;
using Nancy.Json;
using System.IO;
using Newtonsoft.Json;

namespace PrimerParcial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutMe()
        {
            return View();
        }

        //[HttpPost]
        public IActionResult Sumadora(double num1, double num2)
        {
            if (num1 != 0 || num2 != 0)
            {
                double resultado = num1 + num2;
                ViewBag.Resultado = resultado;
                return View(ViewBag.Resultado);
            }
            return View(null);
        }

        public IActionResult Cedula()
        {
            return View();
        }

        private dynamic JsonBool(string data)
        {
            var o = JObject.Parse(data);
            var result = o["Ok"];
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Result(string data)
        {
            API api = new API();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://173.249.49.169:88/api/test/");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (data != "" || data != null)
                {
                    var response = await httpClient.GetAsync("consulta/" + data);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        if (JsonBool(result) != false)
                        {
                            api = JsonConvert.DeserializeObject<API>(result);
                            return View(api);
                        }
                    }
                }
                return View(null);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
