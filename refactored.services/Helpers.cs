using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;
using System.Data;
using refactored.services.InterFace;

namespace refactored.dal;
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

    public static  IMyConnection _hostingEnvironment;
        
        public static SqlConnection NewConnection(IMyConnection hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            string dbpath = _hostingEnvironment.GetDatabasePath();
           var connstr = ConnectionString.Replace("{DataDirectory}",dbpath);
            return new SqlConnection(connstr);
        }
    }
