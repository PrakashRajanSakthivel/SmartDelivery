using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Contracts
{
    public record PaymentIntentResponse(
     string Id,               // "pi_mock123"
     string ClientSecret,     // "mock_secret_123"
     long Amount,             // 1999 (in cents)
     string Currency,         // "usd"
     string Status);          // "requires_payment_method"
}
