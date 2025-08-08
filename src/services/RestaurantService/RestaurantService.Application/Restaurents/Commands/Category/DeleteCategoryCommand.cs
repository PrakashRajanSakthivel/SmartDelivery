using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.Category
{
    public record DeleteCategoryCommand(Guid CategoryId, Guid RestaurantId) : IRequest<bool>;
} 