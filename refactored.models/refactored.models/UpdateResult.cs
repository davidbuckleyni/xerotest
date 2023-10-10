using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactored.models
{
    public class UpdateResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public Guid Id { get; set; }
    }
}
