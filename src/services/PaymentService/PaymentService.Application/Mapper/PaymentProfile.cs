using AutoMapper;
using PaymentService.Application.Payments.Queries;
using PaymentService.Domain.Entites;

namespace PaymentService.Application.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentService.Domain.Entites.Payment, PaymentDto>();
        }
    }
}