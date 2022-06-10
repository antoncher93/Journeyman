using Confluent.Kafka;
using MessageBroker.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MessageBroker.Kafka
{
    public sealed class MessageBus : IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IDictionary<string, string> _producerConfig;
        private readonly IDictionary<string, string> _consumerConfig;
        private IConsumer<Null, string> _consumer;

        public MessageBus() : this("localhost")
        {
                
        }

        public MessageBus(string host)
        {
            _producerConfig = new Dictionary<string, string>() { { "bootstrap.servers", host } };
            _consumerConfig = new Dictionary<string, string>()
            {
                { "group.id", "custom-group" },
                { "bootstrap.servers", host }
            };
            _producer = new ProducerBuilder<Null, string>(_producerConfig)
                .SetValueSerializer(new CustomerSerializer())
                .Build();
        }

        public async void SendMessageAsync(string topic, string message, CancellationToken token = default)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string>(){Value = message}, token);
        }

        public void SubscribeOnTopic<T>(string topic, Action<T> action, CancellationToken token = default) where T : class
        {
            var msgBus = new MessageBus();
            using (_consumer = new ConsumerBuilder<Null, string>(_consumerConfig)
                       .SetValueDeserializer(new CustomerSerializer())
                       .Build())
            {
                _consumer.Assign(new List<TopicPartition>(){new TopicPartition(topic, Partition.Any)});

                while (true && !token.IsCancellationRequested)
                {
                    var result = _consumer.Consume(10);
                    action?.Invoke(result.Message as T);
                }

                _consumer.Close();
            }
        }


        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
