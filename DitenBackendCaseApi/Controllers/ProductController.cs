using BusinessLayer.ValidationRules;
using DataAccessLayer.Repository;
using EntityLayer.DtoModel;
using EntityLayer.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DitenBackendCaseApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Products>>> SearchProductName(string name)
        {
            try
            {
                var result = await productRepository.SearchProductName(name);

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("Bu isimde bir ürün bulunamamıştır.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanında veri aranırken hata oluştu");
            }
        }


        //[HttpGet("{searchStock}")]
        //public async Task<ActionResult<IEnumerable<Products>>> SearchStock(int stock)
        //{
        //    try
        //    {
        //        var result = await productRepository.SearchStock(stock);

        //        if (result.Any())
        //        {
        //            return Ok(result);
        //        }
        //        return NotFound("Bu isimde bir ürün bulunamamıştır.");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanında veri aranırken hata oluştu");
        //    }
        //}


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                return Ok(await productRepository.GetProducts());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanından veri alınırken hata oluştu");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Products>> GetProductsById(int id)
        {
            try
            {
                var result = await productRepository.GetProductsById(id);

                if (result == null)
                {
                    return NotFound($"Id = {id} olan ürün bulunamamıştır.");
                }

                return Ok(new ProductResponse()
                {
                    ProductName = result.ProductName,
                    Stock = result.Stock,
                    DeleteStatus = result.DeleteStatus
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanından veri alınırken hata oluştu");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Products>> AddProduct(Products products)
        {
            try
            {
                if (products == null)
                    return BadRequest("Error");

                products.UpdatedDate = DateTime.Now;

                ProductValidation pv = new ProductValidation();
                ValidationResult validationResult = pv.Validate(products);
                if (validationResult.IsValid)
                {
                    var addProduct = await productRepository.AddProducts(products);

                    return CreatedAtAction(nameof(GetProductsById),
                        new
                        {
                            id = addProduct.ProductId
                        }, addProduct);
                }
                else
                {
                    return BadRequest("Error: Kesinlikle productName, createdUser, updatedUser bilgileri girilmelidir.");
                }
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanına ürün eklerken hata oluştu");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<Products>> UpdateProduct(int id, Products products)
        {
            try
            {
                if (id != products.ProductId)
                    return BadRequest("Product Id eşleşmiyor ya da lütfen productId bilgisini giriniz");

                var updateProduct = await productRepository.GetProductsById(id);

                if (updateProduct == null)
                {
                    return NotFound($"Id = {id} olan ürün bulunamamıştır.");
                }

                ProductValidation pv = new ProductValidation();
                ValidationResult validationResult = pv.Validate(products);
                if (validationResult.IsValid)
                {
                    return await productRepository.UpdateProducts(products);
                }
                else
                {
                    return BadRequest("Error: Kesinlikle productName, createdUser, updatedUser bilgileri girilmelidir.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanında ürün güncellemesi yaparken hata oluştu");
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Products>> DeleteProduct(int id)
        {
            try
            {
                var deleteProduct = await productRepository.GetProductsById(id);

                if (deleteProduct == null)
                {
                    return NotFound($"Id = {id} olan ürün bulunamamıştır.");
                }

                await productRepository.DeleteProducts(id);

                return Ok(new DeleteResponse()
                {
                    DeleteStatus = true
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanından ürün silinirken hata oluştu");
            }
        }
    }
}
