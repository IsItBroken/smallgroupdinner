namespace Sgd.Domain.Common.Services;

public class StateService
{
    public bool IsValidStateAbbreviation(string stateAbbreviation)
    {
        if (string.IsNullOrWhiteSpace(stateAbbreviation))
        {
            return false;
        }

        if (stateAbbreviation.Length != 2)
        {
            return false;
        }

        return _stateAbbreviations.Contains(stateAbbreviation);
    }

    private readonly string[] _stateAbbreviations = new string[]
    {
        "AL",
        "AK",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY"
    };
}
