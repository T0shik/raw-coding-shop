using System;
using RawCoding.S3;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegisterService
    {
        public static IServiceCollection AddRawCodingS3Client(
            this IServiceCollection services,
            Func<S3StorageSettings> settingsFactory)
        {
            services.AddSingleton(settingsFactory());
            services.AddScoped<S3Client>();

            return services;
        }
    }
}