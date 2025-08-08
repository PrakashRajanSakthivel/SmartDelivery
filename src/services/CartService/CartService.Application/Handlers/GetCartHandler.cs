using CartService.Application.Commands;
using CartService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace CartService.Application.Handlers
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, CartDto?>
    {
        private readonly ICartUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCartHandler> _logger;

        public GetCartHandler(ICartUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCartHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CartDto?> Handle(GetCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting cart for user {UserId}", request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            if (cart == null)
            {
                _logger.LogInformation("No cart found for user {UserId}", request.UserId);
                return null;
            }

            var cartDto = _mapper.Map<CartDto>(cart);
            _logger.LogInformation("Retrieved cart {CartId} with {ItemCount} items for user {UserId}", 
                cart.Id, cart.Items.Count, request.UserId);

            return cartDto;
        }
    }
}
