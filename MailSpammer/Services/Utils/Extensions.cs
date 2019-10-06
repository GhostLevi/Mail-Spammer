using System;
using System.Runtime.CompilerServices;
using System.Reactive.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Interface;

namespace Services.Utils
{
    public static class Extensions
    {
        public static IObservable<TSource> DoOnNext<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
        {
            return source.Do(onNext, (throwable) => { }, () => { });
        }

        public static IObservable<TSource> DoOnError<TSource>(this IObservable<TSource> source,
            Action<Exception> onError)
        {
            return source.Do((item) => { }, onError, () => { });
        }

        public static IObservable<TSource> DoOnComplete<TSource>(this IObservable<TSource> source, Action onCompleted)
        {
            return source.Do((item) => { }, (throwable) => { }, onCompleted);
        }


        public static IServiceCollection ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationRoot configuration,
            string sectionName) where T : class, new()
        {
            services.Configure<T>(configuration.GetSection(sectionName));
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var options = provider.GetService<IOptionsMonitor<T>>();
                IOptionsWriter writer = new OptionsWriter(configuration);
                return new WritableOptions<T>(sectionName, writer, options);
            });
            return services;
        }
    }
}