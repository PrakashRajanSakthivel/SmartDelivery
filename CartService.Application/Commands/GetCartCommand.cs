using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Commands
{
    public class GetCartCommand : IRequest<CartDto?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
