using System.Text;
using Confluent.Kafka;

namespace Order.Infrastructure.Extensions;

public static class KafkaExtensions
{
    public static Dictionary<string, string> ResolveHeaders(this Headers headers)
    {
        return headers.ToDictionary(x => x.Key, x => Encoding.ASCII.GetString(x.GetValueBytes()));
    }
}