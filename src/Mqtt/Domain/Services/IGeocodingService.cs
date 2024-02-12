using Moshaveran.Library.ApplicationServices;
using Moshaveran.Library.Results;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGeocodingService : IBusinessService
{
    Task<Result<GeoCode?>> Forward(string address);

    Task<Result<string?>> Reverse(double latitude, double longitude);

    Task<Result<string?>> Reverse(GeoCode code)
        => Reverse(code.Latitude, code.Longitude);
}

public readonly record struct GeoCode(double Latitude, double Longitude);