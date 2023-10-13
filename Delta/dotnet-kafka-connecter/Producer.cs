using Confluent.Kafka;
using System.Net;

public class Producer 
{
    public string Topic = "testtopic";
    public string BoostrapServers = "kafka:9092";
    public void Start() 
    {
        var config = new ProducerConfig
        {
            BootstrapServers = BoostrapServers,
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            producer.Produce(Topic, new Message<Null, string> { Value="Oh hai there"});
        }
    }
}
