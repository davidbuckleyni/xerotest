using refactored.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reafactored.services.InterFace
{
    public interface IProductInterface
    {
        public Product GetProductById(Guid id);
        public List<Product> GetAllProducts();
        List<Product> GetProductsByName(string name);


    }
}
