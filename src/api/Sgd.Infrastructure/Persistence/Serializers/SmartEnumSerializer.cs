using Ardalis.SmartEnum;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Sgd.Infrastructure.Persistence.Serializers;

public class SmartEnumSerializer<TEnum, TValue> : SerializerBase<TEnum>
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        TEnum value
    )
    {
        context.Writer.WriteString(value.Value.ToString());
    }

    public override TEnum Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args
    )
    {
        var value = context.Reader.ReadString();
        return SmartEnum<TEnum, TValue>.FromValue(
            (TValue)Convert.ChangeType(value, typeof(TValue))
        );
    }
}
