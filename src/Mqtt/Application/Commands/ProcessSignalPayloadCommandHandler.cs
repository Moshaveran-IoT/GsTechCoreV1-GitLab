using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.SignalBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessSignalPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessSignalPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessSignalPayloadCommandHandler(IListenerService listener, IRepository<Broker> SignalRepo)
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
        }, args, SignalRepo);
        return processResult.WithValue(new Response());
    }
}