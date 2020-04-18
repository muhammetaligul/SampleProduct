using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SampleProduct.ORM.Models.DB;
using Microsoft.Extensions.Configuration;
namespace SampleProduct.UI.Controllers
{
    //[Authorize(Roles = "Administrator, PowerUser")]
    [Authorize]
    public class ProductsController : Controller
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        public ProductsController(IConfiguration iConfig)
        {
            configuration = iConfig;
            httpClient = new HttpClient();

        }

        public async Task<IActionResult> Index()
        {

            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Product");
            var result = await response.Content.ReadAsStringAsync();
            var sampleProductDBContext = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);

            return View(sampleProductDBContext);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(result);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Category/");
            var result = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,Price,Description")] Product product)
        {

            if (ModelState.IsValid)
            {
                string myjsondata = JsonConvert.SerializeObject(product, Formatting.Indented);
                var content = new StringContent(myjsondata, System.Text.Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync(configuration.GetValue<string>("apiUrl") + "api/Product/", content);
                var httpResult = await httpResponse.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Category/");
            var result = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var httpResponse = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + id.ToString());
            var httpResult = await httpResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(httpResult);
            if (product == null)
            {
                return NotFound();
            }

            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Category/");
            var result = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,Price,Description")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string myjsondata = JsonConvert.SerializeObject(product, Formatting.Indented);
                    var content = new StringContent(myjsondata, System.Text.Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PutAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + product.ProductId, content);
                    var httpResult = await httpResponse.Content.ReadAsStringAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Category/");
            var result = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(result);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var httpResponse = await httpClient.DeleteAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + id);
            var httpResult = await httpResponse.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(int id)
        {
            var response = await httpClient.GetAsync(configuration.GetValue<string>("apiUrl") + "api/Product/" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(result);
            return product != null;
        }
    }
}
