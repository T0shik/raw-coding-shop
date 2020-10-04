using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RawCoding.S3;
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
            [FromServices] CreateProduct createProduct,
            [FromServices] S3Client s3Client)
        {
            var product = new Product
            {
                Name = form.Name,
                Description = form.Description,
            };

            var results = await Task.WhenAll(UploadFiles());

            product.Images.AddRange(results.Select((path, index) => new Image
            {
                Index = index,
                Path = path,
            }));

            return await createProduct.Do(product);

            IEnumerable<Task<string>> UploadFiles()
            {
                var index = 0;
                foreach (var image in form.Images)
                {
                    var fileName = $"{DateTime.Now.Ticks}_{index++}{Path.GetExtension(image.FileName)}";
                    //todo, resolve content type mimi, or do it from file name
                    yield return s3Client.SavePublicFile($"images/{fileName}", ContentType.Jpg, image.OpenReadStream());
                }
            }
        }

        public class TempForm
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public IEnumerable<IFormFile> Images { get; set; }
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