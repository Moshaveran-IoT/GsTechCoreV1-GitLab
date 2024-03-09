using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.CameraBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessCameraPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessCameraPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessCameraPayloadCommandHandler(IListenerService listener, IRepository<Broker> cameraRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save((broker, payloadMessage) =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.Value = $"data:image/png;base64,{payloadMessage}";
            return IResult.Success(broker).ToAsync();
        }, args, cameraRepo);
        return processResult.WithValue(new Response());
    }
}