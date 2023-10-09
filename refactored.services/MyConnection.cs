using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using refactored.services.InterFace;

namespace refactored.services
{
    public class MyConnection : IMyConnection
    {
        private readonly IWebHostEnvironment _environment;
        public MyConnection()
        {
        }
            public MyConnection(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetDatabasePath()
        {
            return _environment.ContentRootPath + @"\App_Data";
        }
    }
}
