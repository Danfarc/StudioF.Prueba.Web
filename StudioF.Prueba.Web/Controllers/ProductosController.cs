using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudioF.Prueba.Web.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Resources;
using Amazon.Auth.AccessControlPolicy;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace StudioF.Prueba.Web.Controllers
{
    public class ProductosController : Controller
    {
        private const string resxFile = @".\Resourse.resx";
        private readonly ILogger<HomeController> _logger;
        private string BaseUrl = "https://qastudiof.myvtex.com/";

        public ProductosController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            ResourceManager rm = new ResourceManager("StudioF.Prueba.Web.GlobalResources.Resource", Assembly.GetExecutingAssembly());

            var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es-MX");
            var text = rm.GetString("Prueba");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es-");
            text = rm.GetString("Prueba");


            List<Product> products = new List<Product>();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string filtro, string consulta)
        {

            List<Product> products = new List<Product>();
            List<Category> category = new List<Category>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage ResProduct = await client.GetAsync($"api/catalog_system/pub/products/search/?{filtro}/{consulta}/");
                if (ResProduct.IsSuccessStatusCode)
                {
                    var oResponseProd = ResProduct.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<Product>>(oResponseProd);

                }

                HttpResponseMessage ResCategory = await client.GetAsync("api/catalog_system/pub/category/tree/3/");
                if (ResCategory.IsSuccessStatusCode)
                {
                    var oResponseCat = ResCategory.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<List<Category>>(oResponseCat);

                }

                return View(products);

            }
        }



    }
}
