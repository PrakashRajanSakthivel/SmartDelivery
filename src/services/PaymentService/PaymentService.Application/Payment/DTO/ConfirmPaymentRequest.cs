using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Payment.DTO
{
    public record ConfirmPaymentRequest(string PaymentIntentId);
}
