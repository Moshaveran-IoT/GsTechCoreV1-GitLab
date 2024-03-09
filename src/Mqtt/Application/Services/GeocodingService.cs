﻿using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Mqtt.Domain.Services;

namespace Moshaveran.GsTech.Mqtt.Application.Services;

public sealed class GeocodingService : IGeocodingService
{
    public Task<IResult<Geocode?>> Forward(string address)
        => IResult.Success<Geocode?>(new Geocode(0, 0)).ToAsync();

    public Task<IResult<string?>> Reverse(double latitude, double longitude)
        => IResult.Success<string?>(string.Empty).ToAsync();
}