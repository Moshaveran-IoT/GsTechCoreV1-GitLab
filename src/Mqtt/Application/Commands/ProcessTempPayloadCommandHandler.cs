using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.TemperatureBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessTempPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessTempPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessTempPayloadCommandHandler(IListenerService listener, IRepository<Broker> TempRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);
        }, args, TempRepo);
        return processResult.WithValue(new Response());
    }
}