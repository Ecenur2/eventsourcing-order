using Newtonsoft.Json.Linq;

namespace Order.Domain.Extensions;

public static class StringExtensions
{
    public static T Deseriliaze<T>(this string value) where T:class
    {
        return ((JObject) Newtonsoft.Json.JsonConvert.DeserializeObject(value)).ToObject<T>();
    }
}