using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactored.models;
namespace refactor_this.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
        }

        public Products(string name)
        {
        }


    }


}
