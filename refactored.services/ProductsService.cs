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





        public Product GetProductByName(string name)
        {
            var conn = Helpers.NewConnection(_myConnection);

            //make sure we only get an integer back
          

            var cmd = new SqlCommand($"select * from product where name = @paramValue", conn);
            cmd.Parameters.AddWithValue("@paramValue", name);

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

 
        
    }
}
