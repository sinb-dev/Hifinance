using Confluent.Kafka;
using System.Net;

namespace dkc.lib;
public class Producer 
{
    HostConfig hostConfig;
    public Producer(HostConfig config) 
    {
        hostConfig = config;
    }

    public void StartMessages(string[] messages) 
    {
        var config = new ProducerConfig
        {
            BootstrapServers = hostConfig.Brokers
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            foreach (string message in messages) 
            {
                producer.Produce(hostConfig.Topic, new Message<Null, string> { Value=message});
            }
            producer.Flush(TimeSpan.FromSeconds(30));
        }
    }
    public void StartDelayedMessages(DelayedMessage[] messages) 
    {
        var config = new ProducerConfig
        {
            BootstrapServers = hostConfig.Brokers
        };
        int numProduced = 0;

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            foreach (DelayedMessage message in messages) 
            {
                Thread.Sleep(message.Delay);
                producer.Produce(hostConfig.Topic, new Message<Null, string> { Value=message.Message}, (deliveryReport) => {
                    if (deliveryReport.Error.Code != ErrorCode.NoError) {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else {
                            Console.WriteLine($"Produced event to topic {hostConfig.Topic} value = {message.Message}");
                            numProduced += 1;
                        }
                });
            }

            producer.Flush(TimeSpan.FromSeconds(10));
            Console.WriteLine($"{numProduced} messages were produced to topic {hostConfig.Topic}");
        }
    }

    public void StartInteractivePrompt() 
    {
        var config = new ProducerConfig
        {
            BootstrapServers = hostConfig.Brokers
        };
        int numProduced = 0;
        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            while (true) {  
                Console.Write(">");
                string? message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message) || message == "exit" || message == "quit" || message == "q") 
                {
                    break;
                }
                producer.Produce(hostConfig.Topic, new Message<Null, string> { Value=message}, (deliveryReport) => {
                    if (deliveryReport.Error.Code != ErrorCode.NoError) {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else {
                            Console.WriteLine($"Produced event to topic {hostConfig.Topic} value = {message}");
                            numProduced += 1;
                        }
                });
                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"{numProduced} messages were produced to topic {hostConfig.Topic}");
            }
        }
    }
}
