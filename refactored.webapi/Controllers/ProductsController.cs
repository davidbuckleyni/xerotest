using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reafactored.services;
using reafactored.services.InterFace;
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

        [HttpGet]
        public IActionResult GetProductWithOptions(Guid id)

        {
            var productswithOptions = _productInterface.GetProductWithOptions(id).ToList();

            return Ok(productswithOptions);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            _logger.Info($"Entering into GetAllProducts in {nameof(ProductsController)}");
            var products = _productInterface.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }

            _logger.Info($"Exiting into GetAllProducts in {nameof(ProductsController)}");
        }


        [HttpGet]
        [Route("get/{id:Guid}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _productInterface.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                return Ok(product);
            }
            
        }

        [HttpPost("Create")]
        public IActionResult Create(Product product)
        {           
            var updateResult = _productInterface.Save(product);
            if (updateResult.Success)
            {
                return Ok(updateResult.Id);
            }else
            {
                return BadRequest(updateResult.ErrorMessage);
            }

        }


        [HttpPut("Update")]
        public IActionResult Update( Product product)
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
        [Route("get/{name}")]
        public  List<Product> GetProductsByName(string name)
        {            
            return _productInterface.GetProductsByName(name);
        }
    }

}

