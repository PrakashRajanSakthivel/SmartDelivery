using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Contracts
{
    public record PaymentResult(
    bool Succeeded,
    string Error = null);
}
