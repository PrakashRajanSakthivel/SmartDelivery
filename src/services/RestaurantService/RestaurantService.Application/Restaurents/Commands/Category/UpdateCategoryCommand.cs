using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.Category
{
    public record UpdateCategoryCommand(UpdateCategoryRequest UpdateCategoryRequest) : IRequest<bool>;
} 