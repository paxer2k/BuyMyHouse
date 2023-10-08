using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppConfiguration
    {
        public string GridApiKey { get;  }
        public string MailSender { get;  }

        public AppConfiguration(IConfiguration configuration) 
        {
            GridApiKey = configuration["GridApiKey"] ?? "";
            MailSender = configuration["MailSender"] ?? "";
        }
    }
}
