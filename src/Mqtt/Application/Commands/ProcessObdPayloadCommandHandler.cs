using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.ObdBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessObdPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessObdPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessObdPayloadCommandHandler(IListenerService listener, IRepository<Broker> cameraRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save((_, payloadMessage) =>
        {
            var broker = new ObdBroker
            {
                Imei = args.Imei,
                CreatedOn = DateTime.Now,
                Value = payloadMessage
            };
            return IResult.Success(broker).ToAsync();
        }, args, cameraRepo);
        return processResult.WithValue(new Response());
    }
}