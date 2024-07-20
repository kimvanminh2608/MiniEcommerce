using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Commons.Exceptions.ValidationException;

namespace Ordering.Application.Commons.Behaviours
{
    //check validation request
    public class ValidationBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResult.Where(e => e.Errors.Any())
                                      .SelectMany(e => e.Errors)
                                      .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
