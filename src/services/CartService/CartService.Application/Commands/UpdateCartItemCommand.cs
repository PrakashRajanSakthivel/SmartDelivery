using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Commands
{
    public class UpdateCartItemCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string MenuItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
