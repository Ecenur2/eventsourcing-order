using Order.Domain.Constants;
using Order.Infrastructure.Helpers;

namespace Order.Api.Extensions;

public static class StartupExtensions
{
    public static void RegisterApplicationLifetimeMethods(this IHostApplicationLifetime lifetime, IApplicationBuilder app)
    {
        lifetime.ApplicationStarted.Register(() =>
        {
            Console.WriteLine("app starting!!");
            var kafkahelper = app.ApplicationServices.GetService<KafkaHelper>();

            kafkahelper.CreateTopic(EventConstants.OrderTopicName, 2, 1);
            kafkahelper.CreateTopic(EventConstants.EventTypeTopicName, 2, 1);
        });
    }
}