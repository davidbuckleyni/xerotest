using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using reafactored.services;
using reafactored.services.InterFace;
using refactor_this.Models;
using refactored.models;

namespace refactored.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductsController : ControllerBase
    {
        IProductInterface _productInterface;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductsService));

        public ProductsController(IProductInterface productInterface)
        {
            _productInterface = productInterface;
        }

        [HttpGet("GetAllProductsWithOptions")]
        public List<Product> GetAllProductsWithOptions()
        {
            return _productInterface.GetAllProductsWithOptions();
        }
        [HttpGet(("GetProductWithOptions"))]  
        public IActionResult GetProductOptionsById(Guid id)
        {
            var productswithOptions = _productInterface.GetProductOptionsById(id).ToList();
            if(productswithOptions.Count > 0)
            {
                return Ok(productswithOptions);

            }
            else
            {
                return new NoContentWithMessageResult("No Data Exists");
            }
            
        }
        [HttpGet(("GetProductOptionsByProductId"))]
        public IActionResult GetProductOptionsByProductId(Guid id)
        {
            var productswithOptions = _productInterface.GetProductOptionsByProductId(id).ToList();
            if (productswithOptions.Count > 0)
            {
                return Ok(productswithOptions);

            }
            else
            {
                return new NoContentWithMessageResult("No Data Exists");
            }
        }

        [HttpDelete("DeleteProductWithOptions")]
        public IActionResult DeleteProductWithOptions(Guid id)
        {
            var products = _productInterface.DeleteProductWithOptions(id);
            if (products.Success == true)
            {
                return new NoContentWithMessageResult("Record with ID " + id + " has been successfully deleted.");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteProductOption")]
        public IActionResult DeleteProductOption(Guid id)
        {
            var products =_productInterface.DeleteOption(id);
            if(products.Success == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest(products.ErrorMessage);

            }
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            _logger.Info($"Entering into GetAllProducts in {nameof(ProductsController)}");
            var products = _productInterface.GetAllProducts();
            if (products.Count > 0)
            {
                return Ok(products);

            }
            else
            {
                return new NoContentWithMessageResult("No Data Exists");

            }

            _logger.Info($"Exiting into GetAllProducts in {nameof(ProductsController)}");
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _productInterface.GetProductById(id);
            if (product == null)
            {
                return new NoContentWithMessageResult("No Data Exists");

            }
            else
            {
                return Ok(product);
            }

        }

        [HttpPost("Create")]
        public IActionResult Create(Product product)
        {
            var updateResult = _productInterface.CreateProduct(product);
            if (updateResult.Success)
            {
                return Ok(updateResult.Id);
            }
            else
            {
                return BadRequest(updateResult.ErrorMessage);
            }

        }

        [HttpPost("CreateProductOption")]
        public IActionResult CreateProductOption(ProductOption productOption)
        {
            var updateResult = _productInterface.CreateProductOption(productOption);
            if (updateResult.Success)
            {
                return Ok(updateResult.Id);
            }
            else
            {
                return BadRequest(updateResult.ErrorMessage);
            }

        }


        [HttpPut("Update")]
        public IActionResult Update(Product product)
        {
            var updateResult = _productInterface.Update(product);
            if (updateResult.Success)
            {
                return Ok(updateResult.Id);
            }
            else
            {
                return BadRequest(updateResult.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("{name}")]
        public List<Product> GetProductsByName(string name)
        {
            return _productInterface.GetProductsByName(name);
        }

        
      
    }

}

