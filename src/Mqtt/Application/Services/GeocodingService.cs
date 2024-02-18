using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Mqtt.Domain.Services;

namespace Moshaveran.GsTech.Mqtt.Application.Services;

public sealed class GeocodingService : IGeocodingService
{
    public Task<IResult<GeoCode?>> Forward(string address)
        => Result.Create<GeoCode?>(new GeoCode(0, 0), true).ToAsync();

    public Task<IResult<string?>> Reverse(double latitude, double longitude)
        => Result.Create<string?>(string.Empty, true).ToAsync();
}