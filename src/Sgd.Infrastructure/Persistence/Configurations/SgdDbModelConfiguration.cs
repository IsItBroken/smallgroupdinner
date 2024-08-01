using Ardalis.SmartEnum;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Sgd.Domain.Common.ValueObjects;
using Sgd.Infrastructure.Persistence.Serializers;

namespace Sgd.Infrastructure.Persistence.Configurations;

public static class SgdDbModelConfiguration
{
    public static void ConfigureModel()
    {
        ConventionRegistry.Register(
            "RepositoryDefaults",
            new ConventionPack
            {
                new IgnoreIfNullConvention(true),
                new IgnoreExtraElementsConvention(true),
            },
            t => true
        );

        var pack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", pack, t => true);

        var assemblyTypes = typeof(SgdDbModelConfiguration)
            .Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(BsonClassMap).IsAssignableFrom(t));
        foreach (var type in assemblyTypes)
        {
            if (BsonClassMap.IsClassMapRegistered(type))
            {
                return;
            }

            var map = Activator.CreateInstance(type) as BsonClassMap;
            BsonClassMap.RegisterClassMap(map);
        }
    }

    public static void RegisterSmartEnumSerializers()
    {
        var assembly = typeof(ContactSystem).Assembly;
        var smartEnumBaseType = typeof(SmartEnum<,>);

        var smartEnumTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Select(t => new { Type = t, Base = t.BaseType })
            .Where(t =>
                t.Base != null
                && t.Base.IsGenericType
                && t.Base.GetGenericTypeDefinition() == smartEnumBaseType
            )
            .Select(t => new { EnumType = t.Type, ValueType = t.Base?.GetGenericArguments()[1] })
            .Where(t => t.ValueType == typeof(string))
            .ToList();

        foreach (var smartEnumType in smartEnumTypes)
        {
            if (smartEnumType.ValueType is null)
            {
                continue;
            }

            var serializerType = typeof(SmartEnumSerializer<,>).MakeGenericType(
                smartEnumType.EnumType,
                smartEnumType.ValueType
            );
            var serializer = (IBsonSerializer?)Activator.CreateInstance(serializerType);
            if (serializer is null)
            {
                continue;
            }

            BsonSerializer.RegisterSerializer(smartEnumType.EnumType, serializer);
        }
    }
}
