using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KardeslerDikimEvi.Helpers
{
    public static class Helper
    {
        public static readonly string cnnString = ConfigurationManager.ConnectionStrings["TerziDbConnectionString"].ConnectionString;
    }
}
