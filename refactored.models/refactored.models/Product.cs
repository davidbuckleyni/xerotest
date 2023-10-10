using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace refactored.models
{
    
        public class Product
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public decimal Price { get; set; }

            public decimal DeliveryPrice { get; set; }

            public List<ProductOption> ProductOptions { get; set; }
        [JsonIgnore]
            public bool IsNew { get; }

            public Product()
            {
                Id = Guid.NewGuid();
                IsNew = true;
            }

        }
    }
