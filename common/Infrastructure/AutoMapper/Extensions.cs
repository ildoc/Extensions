using AutoMapper;
using Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.AutoMapper
{
    public static class Extensions
    {
        public static IServiceCollection AddAutomapper(this IServiceCollection _this, Profile profile)
        {
            return _this.AddAutomapper(new List<Profile> { profile });
        }

        public static IServiceCollection AddAutomapper(this IServiceCollection _this, IEnumerable<Profile> profiles)
        {
            var configAutoMapper = new MapperConfiguration(c =>
            {
                profiles.Each(p => c.AddProfile(p));
            });
            // configAutoMapper.AssertConfigurationIsValid();
            var mapper = configAutoMapper.CreateMapper();
            _this.AddSingleton(mapper);

            return _this;
        }
    }
}
