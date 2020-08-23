using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Shop.Application.Admin.Products;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI.Controllers.Admin
{
    public class ProductsController : AdminBaseController
    {
        private readonly IWebHostEnvironment _env;

        public ProductsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IEnumerable<object> GetProducts([FromServices] GetProducts getProducts) =>
            getProducts.Do();

        [HttpGet("{id}")]
        public object GetProduct(int id, [FromServices] GetProduct getProduct) =>
            getProduct.Do(id);

        [HttpPost]
        public async Task<object> CreateProduct(
            [FromForm] TempForm form,
            [FromServices] CreateProduct createProduct)
        {
            var product = new Product
            {
                Name = form.Name,
                Description = form.Description,
            };
            await foreach (var image in SaveImages(form.Images))
            {
                product.Images.Add(image);
            }

            return await createProduct.Do(product);
        }

        public class TempForm
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public IEnumerable<IFormFile> Images { get; set; }
        }

        private async IAsyncEnumerable<Image> SaveImages(IEnumerable<IFormFile> images)
        {
            var index = 0;
            foreach (var image in images)
            {
                var fileName = $"{DateTime.Now.Ticks}_{index}{Path.GetExtension(image.FileName)}";
                var path = Path.Combine(_env.WebRootPath, "images", fileName);

                await using (var fileStream = System.IO.File.Create(path))
                {
                    await image.CopyToAsync(fileStream);
                }

                yield return new Image
                {
                    Path = fileName,
                    Index = index,
                };

                index++;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, [FromServices] DeleteProduct deleteProduct) =>
            Ok(await deleteProduct.Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct(
            [FromBody] UpdateProduct.Form request,
            [FromServices] UpdateProduct updateProduct) =>
            Ok(await updateProduct.Do(request));
    }
}