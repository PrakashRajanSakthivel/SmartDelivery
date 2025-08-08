using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediatR;
using System.Threading.Tasks;

namespace CartService.Application.Commands
{
    public class ClearCartCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
