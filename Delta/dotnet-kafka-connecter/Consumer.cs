using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Net;

public class Consumer 
{
    public string Topic = "testtopic";
    public string BoostrapServers = "kafka:9092";
    public void Start() 
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = BoostrapServers,
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
        {
            consumer.Subscribe(Topic);
            
            var consumeResult = consumer.Consume(5000); //Consume for 5 seconds
            if (consumeResult != null) {
                Console.WriteLine($"Consumer recieved: {consumeResult.Message}");
            }
        }
    }
}
