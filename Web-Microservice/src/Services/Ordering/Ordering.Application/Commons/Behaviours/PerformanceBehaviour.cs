using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Serilog;

namespace Ordering.Application.Commons.Behaviours
{
    //Check performace request
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMiliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMiliseconds < 500) return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("Application long request {name} ({elapsedMiliseconds}) milisecond {@request}", requestName, elapsedMiliseconds, request);
            return response;
        }
    }
}
