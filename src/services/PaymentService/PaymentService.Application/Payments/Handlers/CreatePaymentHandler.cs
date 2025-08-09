using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.Commands;
using PaymentService.Application.Payments.Queries;
using PaymentService.Application.Services;
using PaymentService.Domain.Entites;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Application.Payments.Handlers
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IPaymentUnitOfWork _unitOfWork;
        private readonly IExternalPaymentService _externalPaymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePaymentHandler> _logger;

        public CreatePaymentHandler(
            IPaymentUnitOfWork unitOfWork,
            IExternalPaymentService externalPaymentService,
            IMapper mapper,
            ILogger<CreatePaymentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _externalPaymentService = externalPaymentService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating payment for amount {Amount} {Currency}", 
                request.Request.Amount, request.Request.Currency);

            try
            {
                // Call external payment provider (Mocky) to create payment intent
                var externalResponse = await _externalPaymentService.CreatePaymentIntentAsync(
                    request.Request.Amount, 
                    request.Request.Currency, 
                    cancellationToken);

                var now = DateTime.UtcNow;

                // Validate and normalize currency (must be 3 chars max)
                var currency = string.IsNullOrEmpty(request.Request.Currency) ? "USD" : 
                              request.Request.Currency.Length > 3 ? "USD" : 
                              request.Request.Currency.ToUpper();

                // Create payment record
                var payment = new PaymentService.Domain.Entites.Payment
                {
                    PaymentId = Guid.NewGuid(),
                    PaymentIntentId = externalResponse.Id,
                    OrderId = request.Request.OrderId,
                    UserId = request.Request.UserId,
                    Amount = request.Request.Amount,
                    Currency = currency,
                    Status = PaymentStatus.RequiresPaymentMethod,
                    ClientSecret = externalResponse.ClientSecret,
                    Description = request.Request.Description,
                    Metadata = request.Request.Metadata,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _unitOfWork.Payments.AddAsync(payment);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Payment created with external ID {ExternalPaymentIntentId} and internal ID {PaymentId}", 
                    externalResponse.Id, payment.PaymentId);

                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create payment intent for amount {Amount} {Currency}", 
                    request.Request.Amount, request.Request.Currency);
                throw;
            }
        }


    }
}