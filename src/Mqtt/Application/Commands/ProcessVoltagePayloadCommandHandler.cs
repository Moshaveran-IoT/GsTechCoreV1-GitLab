using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.VoltageBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessVoltagePayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessVoltagePayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessVoltagePayloadCommandHandler(IListenerService listener, IRepository<Broker> VoltageRepo)
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
        }, args, VoltageRepo);
        return processResult.WithValue(new Response());
    }
}