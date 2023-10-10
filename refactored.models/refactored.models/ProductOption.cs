using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace refactored.models
{
    public class ProductOption
    {

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {


        }

    }

}
