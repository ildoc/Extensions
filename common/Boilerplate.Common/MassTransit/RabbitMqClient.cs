using System;
using System.Threading.Tasks;
using MassTransit;

namespace Boilerplate.Common.MassTransit
{
    public interface IRabbitMqClient
    {
        Task<Response<TResult>> CreateAndGetResponse<TRequest, TResult>(Uri uri, object values, RequestTimeout requestTimeout = default)
            where TRequest : class
            where TResult : class;
    }

    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IBusControl _busControl;

        public RabbitMqClient(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task<Response<TResult>> CreateAndGetResponse<TRequest, TResult>(Uri uri, object values, RequestTimeout requestTimeout = default)
            where TRequest : class
            where TResult : class
        {
            var client = _busControl.CreateRequestClient<TRequest>(uri, requestTimeout);

            return client.GetResponse<TResult>(values);
        }
    }
}
