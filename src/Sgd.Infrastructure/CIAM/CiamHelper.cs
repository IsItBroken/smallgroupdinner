using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Sgd.Infrastructure.CIAM;

public static class CiamHelper
{
    public static void ValidateRequiredConfigurationSet(
        this WebApplicationBuilder builder,
        string[] requiredConfig
    )
    {
        foreach (var key in requiredConfig)
        {
            var value = builder.Configuration.GetValue<string>(key);

            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Config variable missing: {key}.");
            }
        }
    }

    public static void ValidateRequiredConfigurationSecretSet(
        this WebApplicationBuilder builder,
        string[] requiredConfig
    )
    {
        foreach (var key in requiredConfig)
        {
            var value = builder.Configuration.GetValue<string>(key);

            if (string.IsNullOrEmpty(value) || value == "{{SET_IN_SECRET}}")
            {
                throw new Exception($"Config variable missing or set to default: {key}.");
            }
        }
    }
}
