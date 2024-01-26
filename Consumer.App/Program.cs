using Confluent.Kafka;

var config = new ConsumerConfig
{
    GroupId = "consumers",
    BootstrapServers = "localhost:9092"
};

using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
{
    consumer.Subscribe("ExchangeCustomerData");
    while (true)
    {
        var CustomerDetails = consumer.Consume();
        Console.WriteLine(CustomerDetails.Message.Value);
    }
}