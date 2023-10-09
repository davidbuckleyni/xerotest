using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reafactored.services.InterFace;
using refactored.models;

namespace refactored.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductsController : ControllerBase
    {
        IProductInterface _productInterface;
        public ProductsController(IProductInterface productInterface)
        {
            _productInterface = productInterface;
        }

        [HttpGet]
        public List<Product> GetAllProducts()
        {
            return _productInterface.GetAllProducts(); 



        }


        [HttpGet]
        [Route("/Products/{id}")]
        public Product GetProductById(Guid id)
        {
            return _productInterface.GetProductById(id);
        }

        [HttpGet("/Products/{name2}")]
        [Route("/Products/{name3}")]
        public Product GetProductByName(string name)
        {
            return _productInterface.GetProductByName(name);
        }
    }

}

