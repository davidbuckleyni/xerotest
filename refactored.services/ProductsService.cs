using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using reafactored.services.InterFace;
using refactored.dal;
using refactored.models;
using refactored.services.InterFace;
using Microsoft.AspNetCore.Http.Features;
using refactor_this.Models;
using log4net;
using log4net.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Data.Entity;

namespace reafactored.services
{
    public class ProductsService : IProductInterface
    {
        public new List<Product> Items { get; set; }


        IMyConnection _myConnection;
        RefactoredDBContext _dbcontext;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductsService));

        public ProductsService(IMyConnection myConnection, RefactoredDBContext dBContext)
        {
            _myConnection = myConnection;
            _dbcontext = dBContext;
        }



        /// <summary>Gets all products.</summary>
        /// <returns>
        ///   Returns a List of products
        /// </returns>
        public List<Product> GetAllProducts()
        {
            string methodEnter = "Enterting GetAllProducts";
            _logger.Info(methodEnter);

            try
            {
                Items = _dbcontext.Product.ToList();
                string methodExit = "Exiting GetAllProducts";
                _logger.Info(methodEnter);

            }
            catch (Exception ex)
            {
                string method = "GetAllProducts";
                string methodError = $"An Erorr has occoured in the {nameof(ProductsService)} class in method {method} ";
                _logger.Error(methodError, ex);

            }

            return Items;
        }


        /// <summary>Gets the name of the products by.</summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///  Returns a list of products by name
        /// </returns>
        public List<Product> GetProductsByName(string name)
        {
            List<Product> products = new List<Product>();
            _logger.Info($"Entering  GetProductsByName Method in  the {nameof(ProductsService)} class");

            try
            {
                products = _dbcontext.Product.Where(w => w.Name.Contains(name)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetProductsByName Method in  the {nameof(ProductsService)} class", ex);

            }
            return products;

        }



        /// <summary>Gets the product by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A Singular product by id
        /// </returns>
        public Product GetProductById(Guid id)
        {
            //make sure we only get an integer back
            if (Guid.TryParse(id.ToString(), out Guid guidValue))
            {
                id = guidValue;
            }
            else
            {
                id = Guid.Empty;
            }
            var product = _dbcontext.Product.Where(w => w.Id == id).FirstOrDefault();


            return product;
        }

        /// <summary>
        /// Saves the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        public UpdateResult CreateProductOption(ProductOption productpOption)
        {
            try
            {
                _logger.Info($"Entering  CreateProductOption Method in  the {nameof(ProductsService)} class");

                _dbcontext.Add(productpOption);
                _dbcontext.SaveChanges();
                return new UpdateResult { Success = true, Id = productpOption.Id };

                _logger.Info($"Exiting CreateProductOption Method in  the {nameof(ProductsService)} class");
            }
            catch (Exception ex)
            {

                _logger.Error($"Error Occoured CreateProductOption Method in  the {nameof(ProductsService)} class", ex);
                return new UpdateResult { Success = false, ErrorMessage = ex.StackTrace.ToString() };

            }
        }


        /// <summary>
        /// Saves the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        public UpdateResult CreateProduct(Product product)
        {
            try
            {
                _logger.Info($"Entering  Save Method in  the {nameof(ProductsService)} class");

                _dbcontext.Add(product);
                _dbcontext.SaveChanges();
                return new UpdateResult { Success = true, Id = product.Id };

                _logger.Info($"Exiting Save Method in  the {nameof(ProductsService)} class");
            }
            catch (Exception ex)
            {

                _logger.Error($"Error Occoured Save Method in  the {nameof(ProductsService)} class", ex);
                return new UpdateResult { Success = false, ErrorMessage = ex.StackTrace.ToString() };

            }
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="product">The product.</param>
        public UpdateResult Update(Product product)
        {

            try
            {
                _logger.Info($"Entering Update Method in  the {nameof(ProductsService)} class");


                var existing = _dbcontext.Product.Where(w => w.Id == product.Id).FirstOrDefault();
                if (product != null)
                {
                    _dbcontext.Entry(existing).CurrentValues.SetValues(product);
                    _dbcontext.SaveChanges();
                    return new UpdateResult { Success = true, Id = product.Id };
                }
                else
                {

                    _dbcontext.SaveChanges(true);
                    return new UpdateResult { Success = true, Id = product.Id };
                }
                _logger.Info($"Exiting Update Method in  the {nameof(ProductsService)} class");
            }
            catch (Exception ex)
            {
                string method = "Update Product";
                string methodError = $"An Erorr has occoured in the {nameof(ProductsService)} class in method {method} ";
                _logger.Error(methodError, ex);
                return new UpdateResult { Success = false, ErrorMessage = ex.InnerException.ToString() };


            }
        }

        public List<Product> GetAllProductsWithOptions()
        {
            var productsWithOptions = _dbcontext.Product
                            .Join(
                            _dbcontext.ProductOption,
                            product => product.Id, // the primary key field in the Product table
                            option => option.ProductId, // the field to join with in the ProductOption table
                            (product, option) => new
                            {
                                Product = product,
                                Option = option
                            }
                        )                        
                        .GroupBy(x => x.Product) // Group by product to create a collection of options for each product
                        .ToList()
                        .Select(group => new Product
                        {
                            Id = group.Key.Id,
                            Name = group.Key.Name,
                            Description = group.Key.Description,
                            DeliveryPrice = group.Key.DeliveryPrice,
                            Price = group.Key.Price,

                            // Other product properties you want to include
                            ProductOptions = group.Select(x => x.Option).ToList()
                        })
                    .ToList();
            return productsWithOptions;

        }

        public List<Product> GetProductOptionsByProductId(Guid id)
        {
            var productsWithOptions = _dbcontext.Product
                            .Join(
                            _dbcontext.ProductOption,
                            product => product.Id, // the primary key field in the Product table
                            option => option.ProductId, // the field to join with in the ProductOption table
                            (product, option) => new
                            {
                                Product = product,
                                Option = option
                            }
                        )
                        .Where(w => w.Product.Id == id && w.Option.ProductId == id)
                        .GroupBy(x => x.Product) // Group by product to create a collection of options for each product
                        .ToList()
                        .Select(group => new Product
                        {
                            Id = group.Key.Id,
                            Name = group.Key.Name,
                            Description = group.Key.Description,
                            DeliveryPrice = group.Key.DeliveryPrice,
                            Price = group.Key.Price,

                            // Other product properties you want to include
                            ProductOptions = group.Select(x => x.Option).ToList()
                        })
                    .ToList();
            return productsWithOptions;

        }

        /// <summary>
        /// Gets the product with options.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<Product> GetProductOptionsById(Guid id)
        {
            var productsWithOptions = _dbcontext.Product
                .Join(
                _dbcontext.ProductOption,
                product => product.Id, // the primary key field in the Product table
                option => option.ProductId, // the field to join with in the ProductOption table
                (product, option) => new
                {
                    Product = product,
                    Option = option
                }
            )
             .Where(w => w.Product.Id == id)
            .GroupBy(x => x.Product) // Group by product to create a collection of options for each product
            .ToList()
            .Select(group => new Product
            {
                Id = group.Key.Id,
                Name = group.Key.Name,
                Description = group.Key.Description,
                DeliveryPrice = group.Key.DeliveryPrice,
                Price = group.Key.Price,

                // Other product properties you want to include
                ProductOptions = group.Select(x => x.Option).ToList()
            })
        .ToList();

            return productsWithOptions;

        }

        /// <summary>
        /// Deletes the option.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns update result based on the success of the deletion</returns>
        public UpdateResult DeleteOption(Guid id)
        {
            var options = _dbcontext.ProductOption.Where(w => w.Id == id).FirstOrDefault();
            try
              {
                if (options != null)
                {
                    var productOption = _dbcontext.ProductOption.Remove(options);
                }else
                {
                    return new UpdateResult { Success = false,ErrorMessage="Data does not exist" };
                }
                _dbcontext.SaveChanges();
                return new UpdateResult { Success = true, SuccessMessage = "Record Deleted", Id = id };

            }
            catch (Exception ex)
            {
                return new UpdateResult { Success = false, ErrorMessage = ex.InnerException.ToString() };

            }
        }


        /// <summary>
        /// Deletes the product and options.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns update result based on the success of the deletion</returns>
        public UpdateResult DeleteProductWithOptions(Guid id)
        {
            var product = _dbcontext.Product.Include(p => p.ProductOptions) // Load the associated options
            .FirstOrDefault(p => p.Id == id);
            try
            {
                if (product != null)
                {
                    var options = _dbcontext.ProductOption.Where(w=>w.Id==id).ToList();
                    _dbcontext.Product.Remove(product);
                    _dbcontext.ProductOption.RemoveRange(options);
                }              
                else

                {
                    return new UpdateResult { Success = false, SuccessMessage = "No Data Exists", Id = id };
                }

                _dbcontext.SaveChanges();
                return new UpdateResult { Success = true, SuccessMessage = "Record Deleted", Id = id };

            }
            catch (Exception ex)
            {
                return new UpdateResult { Success = false, ErrorMessage = ex.InnerException.ToString() };

            }
        }
        /// <summary>
        /// Deletes the option.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns update result based on the success of the deletion</returns>
        public UpdateResult UpdateOption(ProductOption productoption)
        {
            try
            {
                var productOption = _dbcontext.ProductOption.Where(w => w.Id == productoption.Id).FirstOrDefault();
                if (productOption != null)
                {
                    _dbcontext.Entry(productOption).CurrentValues.SetValues(productoption);
                    _dbcontext.SaveChanges();

                    _dbcontext.SaveChanges();
                    return new UpdateResult { Success = true, SuccessMessage = "Record Deleted", Id = productoption.Id };

                }
                else
                {
                    return new UpdateResult { Success = false, ErrorMessage = "No Data", Id = productoption.Id };

                }
            }
            catch (Exception ex)
            {
                return new UpdateResult { Success = false, ErrorMessage = ex.InnerException.ToString() };

            }

        }

    }

}
