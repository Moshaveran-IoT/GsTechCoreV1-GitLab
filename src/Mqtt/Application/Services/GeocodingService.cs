using Moshaveran.Library.Results;
using Moshaveran.Mqtt.Domain.Services;

namespace Moshaveran.GsTech.Mqtt.Application.Services;

public sealed class GeocodingService : IGeocodingService
{
    public Task<Result<GeoCode?>> Forward(string address) => Task.FromResult(Result.Create<GeoCode?>(new GeoCode(0, 0), true));

    public Task<Result<string?>> Reverse(double latitude, double longitude) => Task.FromResult(Result.Create<string?>(string.Empty, true));
}