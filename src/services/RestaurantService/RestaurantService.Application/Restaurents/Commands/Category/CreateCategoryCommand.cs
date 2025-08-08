using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.Category
{
    public record CreateCategoryCommand(CreateCategoryRequest CreateCategoryRequest) : IRequest<Guid>;
} 