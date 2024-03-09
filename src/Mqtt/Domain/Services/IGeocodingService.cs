using Moshaveran.Library.ApplicationServices;
using Moshaveran.Library.Results;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGeocodingService : IBusinessService
{
    Task<IResult<Geocode?>> Forward(string address);

    Task<IResult<string?>> Reverse(double latitude, double longitude);

    Task<IResult<string?>> Reverse(Geocode code)
        => Reverse(code.Latitude, code.Longitude);
}

public readonly record struct Geocode(double Latitude, double Longitude);