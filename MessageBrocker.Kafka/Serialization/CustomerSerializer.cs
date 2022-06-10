using Confluent.Kafka;
using System;
using System.Text;

namespace MessageBroker.Kafka.Serialization
{
    class CustomerSerializer : ISerializer<string>, IDeserializer<string>
    {
        private readonly Encoding _encoding;

        public CustomerSerializer(Encoding encoding)
        {
            _encoding = encoding;   
        }

        public CustomerSerializer() : this(Encoding.UTF8)
        {
                
        }

        public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return _encoding.GetString(data);
        }

        public byte[] Serialize(string data, SerializationContext context)
        {
            return _encoding.GetBytes(data);
        }
    }
}
