using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.AutoMapper
{
    public static class Extensions
    {
        public static IServiceCollection AddAutomapper(this IServiceCollection _this, Profile profile)
        {
            var configAutoMapper = new MapperConfiguration(c =>
            {
                c.AddProfile(profile);
            });
            // configAutoMapper.AssertConfigurationIsValid();
            var mapper = configAutoMapper.CreateMapper();
            _this.AddSingleton(mapper);

            return _this;
        }
    }
}
