namespace OrderService.Application.Common
{
    public class OrderBusinessValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }

        public OrderBusinessValidationException(ValidationResult validationResult)
            : base($"Order business validation failed: {string.Join("; ", validationResult.Errors)}")
        {
            ValidationResult = validationResult;
        }

        public OrderBusinessValidationException(string message)
            : base(message)
        {
            ValidationResult = ValidationResult.Failure(message);
        }

        public OrderBusinessValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ValidationResult = ValidationResult.Failure(message);
        }
    }
}
