using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;
using Moshaveran.Mqtt.Domain.Services;

using System.Runtime.CompilerServices;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.GpsBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessGpsPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessGpsPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessGpsPayloadCommandHandler(IListenerService listener, IRepository<Broker> cameraRepo, IGeocodingService geocoding)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save(async (broker, _) =>
        {
            if (!isValidGps())
            {
                return IResult.Fail(broker);
            }
            var geocode = await geocoding.Reverse(broker.Latitude, broker.Longitude);
            if (geocode.IsFailure)
            {
                return geocode.WithValue(broker);
            }

            broker.Address = geocode.GetValue();
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            bool isValidGps()
                => broker.Latitude is >= Constants.SOUTH_POLE and <= Constants.NORTH_POLE && broker.Longitude is >= Constants.INTERNATIONAL_DATE_LINE_WEST and <= Constants.INTERNATIONAL_DATE_LINE_EAST && broker.Latitude.ToString().Length != 1 && broker.Longitude.ToString().Length != 1;
        }, args, cameraRepo);
        return processResult.WithValue(new Response());
    }
}