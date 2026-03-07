using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Contracts;

namespace PaymentService.Application.common
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponse> CreatePaymentIntentAsync(decimal amount, string currency);
        Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId);
    }
}
