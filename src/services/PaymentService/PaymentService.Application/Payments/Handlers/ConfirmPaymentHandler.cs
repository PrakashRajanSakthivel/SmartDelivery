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
    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, PaymentDto>
    {
        private readonly IPaymentUnitOfWork _unitOfWork;
        private readonly IExternalPaymentService _externalPaymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<ConfirmPaymentHandler> _logger;

        public ConfirmPaymentHandler(
            IPaymentUnitOfWork unitOfWork,
            IExternalPaymentService externalPaymentService,
            IMapper mapper,
            ILogger<ConfirmPaymentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _externalPaymentService = externalPaymentService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentDto> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting payment confirmation for {PaymentIntentId}", 
                    request.Request.PaymentIntentId);

                // Get the payment from our database
                var payment = await _unitOfWork.Payments.GetByPaymentIntentIdAsync(request.Request.PaymentIntentId);
                if (payment == null)
                {
                    throw new InvalidOperationException($"Payment with intent ID {request.Request.PaymentIntentId} not found");
                }

                if (payment.Status == PaymentStatus.Succeeded)
                {
                    _logger.LogInformation("Payment {PaymentIntentId} is already confirmed", request.Request.PaymentIntentId);
                    return _mapper.Map<PaymentDto>(payment);
                }

                var now = DateTime.UtcNow;
                var previousStatus = payment.Status;

                // Update payment to processing
                payment.Status = PaymentStatus.Processing;
                payment.PaymentMethodId = request.Request.PaymentMethodId;
                payment.UpdatedAt = now;

                // Call external payment provider to confirm payment
                var externalResponse = await _externalPaymentService.ConfirmPaymentAsync(
                    request.Request.PaymentIntentId,
                    request.Request.PaymentMethodId,
                    cancellationToken);

                // Update payment based on external response
                var success = externalResponse.Status?.ToLower() == "succeeded";
                var finalStatus = success ? PaymentStatus.Succeeded : PaymentStatus.PaymentFailed;

                payment.Status = finalStatus;
                payment.UpdatedAt = now;
                
                if (success)
                {
                    payment.ProcessedAt = now;
                    payment.ErrorMessage = null;
                    payment.ErrorCode = null;
                }
                else
                {
                    payment.ErrorMessage = externalResponse.Error ?? "Payment failed";
                    payment.ErrorCode = externalResponse.Error ?? "unknown_error";
                }

                // Store provider response for debugging
                payment.ProviderResponse = System.Text.Json.JsonSerializer.Serialize(externalResponse);

                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Payment confirmation completed for {PaymentIntentId} with status {Status}", 
                    request.Request.PaymentIntentId, finalStatus);

                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during payment confirmation for {PaymentIntentId}", request.Request.PaymentIntentId);

                // Get payment again to update it
                var payment = await _unitOfWork.Payments.GetByPaymentIntentIdAsync(request.Request.PaymentIntentId);
                if (payment != null)
                {
                    payment.Status = PaymentStatus.PaymentFailed;
                    payment.ErrorMessage = "Internal error during payment processing";
                    payment.ErrorCode = "internal_error";
                    payment.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<PaymentDto>(payment);
                }

                throw;
            }
        }
    }
}