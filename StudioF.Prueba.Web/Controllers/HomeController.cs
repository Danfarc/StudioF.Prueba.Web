using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudioF.Prueba.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace StudioF.Prueba.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string BaseUrl = "https://qastudiof.myvtex.com/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            return View();
        }

        public async Task<IActionResult> API()
        {
            
            List<Product> products = new List<Product>();
            List<Category> category = new List<Category>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage ResProduct = await client.GetAsync("api/catalog_system/pub/products/search/");
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

                var query = from prod in products
                            join cat in category on int.Parse(prod.categoryId) equals cat.Id
                            select new 
                            { 
                                productId = prod.productId,
                                productName = prod.productName,
                                brand = prod.brand,
                                category = cat.Name
                            };

                return Ok(query);
            }
        }

        public async Task<IActionResult> API2(string NameProduct)
        {

            List<Product> products = new List<Product>();
            CategoryFind category = new CategoryFind();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage ResProduct = await client.GetAsync($"api/catalog_system/pub/products/search/{NameProduct}");
                if (ResProduct.IsSuccessStatusCode)
                {
                    var oResponseProd = ResProduct.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<Product>>(oResponseProd);

                }

                HttpResponseMessage ResCategory = await client.GetAsync($"api/catalog_system/pub/category/{products[0].categoryId}");
                if (ResCategory.IsSuccessStatusCode)
                {
                    var oResponseCat = ResCategory.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<CategoryFind>(oResponseCat);

                }

                //var query = from prod in products
                //            join cat in category on int.Parse(prod.categoryId) equals cat.id
                //            select new
                //            {
                //                productId = prod.productId,
                //                productName = prod.productName,
                //                brand = prod.brand
                //            };

                products[0].category = category.name;

                return Ok(products);
            }
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



        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        class Pet
        {
            public string Name { get; set; }
            public Person Owner { get; set; }
        }

    }
}
