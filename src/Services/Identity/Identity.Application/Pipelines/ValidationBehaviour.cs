using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Identity.Application.Pipelines;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any()) 
        {
            ValidationContext<TRequest> context = new(request);

            ValidationResult[] validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            ValidationFailure[] validationFailures = validationResults.SelectMany(result => result.Errors).Where(failures => failures is not null).ToArray();

            if (validationFailures.Any())
            {
                throw new ValidationException(message: $"Error occured during {typeof(TRequest).FullName} validation", errors: validationFailures);
            }
        }

        return await next();
    }
}
