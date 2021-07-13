using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Exceptions
{
    public static class Extensions
    {
        public static IMvcBuilder AddControllersWithExceptionFilters(this IServiceCollection _this) =>
            _this.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
    }
}
