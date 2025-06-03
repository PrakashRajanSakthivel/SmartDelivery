using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Payment.DTO
{
    // Request DTOs
    public record CreatePaymentIntentRequest(decimal Amount, string Currency = "usd");
}
