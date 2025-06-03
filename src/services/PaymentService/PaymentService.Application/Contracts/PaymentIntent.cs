using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Contracts
{
    internal class PaymentIntent
    {
        public string Id { get; set; }
        public string ClientSecret { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }
}
