﻿using System;
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
            [FromForm] ProductForm form,
            [FromServices] CreateProduct createProduct,
            [FromServices] S3Client s3Client)
        {
            var product = new Product
            {
                Name = form.Name,
                Slug = form.Name.Replace(" ", "-").ToLower(),
                Description = form.Description,
                Series = form.Series,
                StockDescription = form.StockDescription
            };

            var results = await Task.WhenAll(UploadFiles(s3Client, form.Images));

            product.Images.AddRange(results.Select((path, index) => new Image
            {
                Index = index,
                Path = path,
            }));

            return await createProduct.Do(product);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct(
            [FromForm] ProductForm form,
            [FromServices] GetProduct getProduct,
            [FromServices] UpdateProduct updateProduct,
            [FromServices] S3Client s3Client)
        {
            var product = getProduct.Do(form.Id);
            product.Description = form.Description;
            product.Series = form.Series;
            product.StockDescription = form.StockDescription;

            if (form.Images != null && form.Images.Any())
            {
                product.Images = new List<Image>();
                var results = await Task.WhenAll(UploadFiles(s3Client, form.Images));

                product.Images.AddRange(results.Select((path, index) => new Image
                {
                    Index = index,
                    Path = path,
                }));
            }

            await updateProduct.Do(product);
            return Ok();
        }

        public class ProductForm
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Series { get; set; }
            public string StockDescription { get; set; }
            public IEnumerable<IFormFile> Images { get; set; }
        }

        private static IEnumerable<Task<string>> UploadFiles(S3Client s3Client, IEnumerable<IFormFile> files)
        {
            var index = 0;
            foreach (var image in files)
            {
                var fileName = $"{DateTime.Now.Ticks}_{index++}{Path.GetExtension(image.FileName)}";
                yield return s3Client.SavePublicFile($"images/{fileName}", image.OpenReadStream());
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, [FromServices] DeleteProduct deleteProduct) =>
            Ok(await deleteProduct.Do(id));
    }
}