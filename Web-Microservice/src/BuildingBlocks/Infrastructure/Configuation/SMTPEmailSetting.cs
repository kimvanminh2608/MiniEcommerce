using Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuation
{
    public class SMTPEmailSetting : ISMTPEmailSetting
    {
        public string DisplayName { get; set; }
        public bool EnableVerification { get; set; }
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public bool UseSsl { get; set; }
        public int port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
