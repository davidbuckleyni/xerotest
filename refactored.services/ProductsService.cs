using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Xml.Linq;
using reafactored.services.InterFace;
using refactored.dal;
using refactored.models;
using refactored.services.InterFace;
using Microsoft.AspNetCore.Http.Features;
using refactor_this.Models;

namespace reafactored.services
{
    public class ProductsService : IProductInterface
    {
        public new List<Product> Items { get; set; }

        public int MyProperty { get; set; }

        IMyConnection _myConnection;
        public ProductsService(IMyConnection myConnection)
        {
            _myConnection = myConnection;
        }

        

        /// <summary>Gets the product options.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///  Retruns a Product Options even if multiple
        /// </returns>
        public List<ProductOption> GetProductOptions(int id)
        {
            List<ProductOption> productOptions = new List<ProductOption>();
             var conn = Helpers.NewConnection(_myConnection);
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;
            while (rdr.Read())
            {
                ProductOption productOption = new ProductOption
                {
                    Id = Guid.Parse(rdr["Id"].ToString()),
                    Name = rdr["Name"].ToString(),
                    Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString(),
                };
                productOptions.Add(productOption);
            }
            return productOptions;
        }

        /// <summary>Gets the product by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A Singular product by id
        /// </returns>
        public Product GetProductById(Guid id)
        {
            var conn = Helpers.NewConnection(_myConnection);

            //make sure we only get an integer back
            if (Guid.TryParse(id.ToString(), out Guid guidValue))
            {
                id = guidValue;
            }
            else
            {
                id = Guid.Empty;
            }

            var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;
            Product product = new Product
            {

                Id = Guid.Parse(rdr["Id"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
            };

            return product;
        }


        /// <summary>Gets all products.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public List<Product> GetAllProducts()
        {
            Items = new List<Product>();
            var conn = Helpers.NewConnection(_myConnection);
            var cmd = new SqlCommand($"select id from product", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(GetProductById(id));
            }
            return Items;
        }


        /// <summary>Gets the name of the products by.</summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public List<Product> GetProductsByName(string name)
        {
            var conn = Helpers.NewConnection(_myConnection);

            //make sure we only get an integer back
            List<Product> products = new List<Product>();
            
            var cmd = new SqlCommand($"select * from product where name '%' + @paramValue + '%'\"", conn);

            // only use parameters here so that sql injection cannot happen
            cmd.Parameters.AddWithValue("@paramValue", name);

            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            while (rdr.Read())
            {
                Product product = new Product
                {
                    Id = Guid.Parse(rdr["Id"].ToString()),
                    Name = rdr["Name"].ToString(),
                    Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString(),
                    Price = rdr["Price"] == DBNull.Value ? 0m : decimal.Parse(rdr["Price"].ToString()),
                    DeliveryPrice = rdr["DeliveryPrice"] == DBNull.Value ? 0m : decimal.Parse(rdr["DeliveryPrice"].ToString()),

                };
                products.Add(product);
            }
            return products;
        }
    }
}
