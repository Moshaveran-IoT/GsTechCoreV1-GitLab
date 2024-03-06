using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Domain.Commands;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessCameraPayloadCommandHandler(IListenerService listener, IRepository<CameraBroker> cameraRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<ProcessCameraPayloadCommand, IResult<ProcessCameraPayloadCommandResponse>>
{
    public async Task<IResult<ProcessCameraPayloadCommandResponse>> Handle(ProcessCameraPayloadCommand request, CancellationToken cancellationToken)
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
        return processResult.WithValue(new ProcessCameraPayloadCommandResponse());
    }
}