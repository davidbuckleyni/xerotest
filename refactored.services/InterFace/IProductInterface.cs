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
        public List<Product> GetProductOptionsById(Guid id);
        public List<Product> GetProductOptionsByProductId(Guid id);
        public UpdateResult Update(Product product);
         public UpdateResult UpdateOption(ProductOption productoption);
        public UpdateResult DeleteOption(Guid id);

        public UpdateResult DeleteProductWithOptions(Guid id);

        public UpdateResult CreateProductOption(ProductOption productpOption);

        public UpdateResult CreateProduct(Product product);


    }
}
