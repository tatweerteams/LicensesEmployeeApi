using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{

    public class HostBusOptions
    {
        public string RabbitMqAddress { get; set; }
        public string RabbitMqVirtualHost { get; set; }
        public string RabbitMqUserName { get; set; }
        public string RabbitMqPassword { get; set; }
        public string RabbitMqHostName { get; set; }
    }
}
