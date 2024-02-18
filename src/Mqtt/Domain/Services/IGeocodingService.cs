using Moshaveran.Library;
using Moshaveran.Library.ApplicationServices;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGeocodingService : IBusinessService
{
    Task<IResult<GeoCode?>> Forward(string address);

    Task<IResult<string?>> Reverse(double latitude, double longitude);

    Task<IResult<string?>> Reverse(GeoCode code)
        => Reverse(code.Latitude, code.Longitude);
}

public readonly record struct GeoCode(double Latitude, double Longitude);