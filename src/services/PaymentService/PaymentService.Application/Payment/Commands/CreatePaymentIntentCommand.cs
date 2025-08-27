using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PaymentService.Application.Payment.DTO;

namespace PaymentService.Application.Payment.Commands
{
    public record CreatePaymentIntentCommand(CreatePaymentIntentRequest request) : IRequest<Guid>;
}
