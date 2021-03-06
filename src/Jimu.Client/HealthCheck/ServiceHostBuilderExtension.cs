﻿using Autofac;
using Jimu.Client.HealthCheck;

namespace Jimu.Client
{
    public static partial class ServiceHostBuilderExtension
    {
        public static IServiceHostClientBuilder UseServerHealthCheck(this IServiceHostClientBuilder serviceHostBuilder, int intervalMinute)
        {
            serviceHostBuilder.RegisterService(container =>
            {
                container.RegisterType<QuartzHealthCheck>().As<IHealthCheck>().WithParameter("intervalMinute", intervalMinute).SingleInstance();
            });
            serviceHostBuilder.AddInitializer(container =>
            {
                var healthCheck = container.Resolve<IHealthCheck>();
                var logger = container.Resolve<ILogger>();
                logger.Info($"[config]use server health check, checked job interval: {intervalMinute} min");
                healthCheck.RunAsync();
            });
            return serviceHostBuilder;
        }
    }
}
