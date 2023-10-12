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

        /// <summary>
        /// Gets all products with options.
        /// </summary>
        /// <returns>Returns all product allong with their options</returns>
        [HttpGet("GetAllProductsWithOptions")]
        public List<Product> GetAllProductsWithOptions()
        {
            return _productInterface.GetAllProductsWithOptions();
        }

        /// <summary>Gets the product options by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// returns a list of product options via id
        /// </returns>
        [HttpGet(("GetProductOptionsById"))]
        public IActionResult GetProductOptionsById(Guid id)
        {
            var productswithOptions = _productInterface.GetProductOptionsById(id).ToList();
            if (productswithOptions.Count > 0)
            {
                return Ok(productswithOptions);

            }
            else
            {
                return new NoContentWithMessageResult("No Data Exists");
            }

        }
        /// <summary>
        /// Gets the product options by product identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> returns a list of products with their options</returns>
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

        /// <summary>
        /// Deletes the product with options.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>returns a non content with messages to say if record has been deleted</returns>
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

        /// <summary>
        /// Deletes the product option.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> returns ok if product deleted other wise bad request</returns>
        [HttpDelete("DeleteProductOption")]
        public IActionResult DeleteProductOption(Guid id)
        {
            var products = _productInterface.DeleteOption(id);
            if (products.Success == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest(products.ErrorMessage);

            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns> Retuns all products if not no content</returns>
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            _logger.Info($"Entering into GetAllProducts in {nameof(ProductsController)}");
            var products = _productInterface.GetAllProducts();
            if (products != null)
            {
                return Ok(products);

            }
            else
            {
                return new NoContentWithMessageResult("No Data Exists");

            }

            _logger.Info($"Exiting into GetAllProducts in {nameof(ProductsController)}");
        }

        /// <summary>
        /// Gets the product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Returns a product by id</returns>
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

        /// <summary>
        /// Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>returns a guid for the new created product</returns>
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

        /// <summary>
        /// Creates the product option.
        /// </summary>
        /// <param name="productOption">The product option.</param>
        /// <returns>returns a guid if the create has been succesfull/returns>
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

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>returns a guid if the update succesfull</returns>
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


        /// <summary>
        /// [HttpGet]
        /// </summary>
        /// <param name="name"></param>
        /// <returns> A List of products by name</returns>
        [HttpGet]
        [Route("{name}")]
        public List<Product> GetProductsByName(string name)
        {
            return _productInterface.GetProductsByName(name);
        }



    }

}

