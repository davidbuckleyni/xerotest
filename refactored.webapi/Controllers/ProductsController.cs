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
        [Route("get/{id:int}")]
        public Product GetProductById(Guid id)
        {
            return _productInterface.GetProductById(id);
        }
        

        [HttpGet]
        [Route("get/{name}")]
        public  List<Product> GetProductsByName(string name)
        {
            return _productInterface.GetProductsByName(name);
        }
    }

}

