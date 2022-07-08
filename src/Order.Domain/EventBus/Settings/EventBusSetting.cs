namespace Order.Domain.EventBus.Settings;

public class EventBusSetting
{
    public string MongoConnectionUrl { get; set; }
    
    public string BrokerList { get; set; }
    
    public int ConnectionTimeout { get; set; }

    public int ConsumerTimeout { get; set; }

    public int PersistentConsumerTimeout { get; set; }
}