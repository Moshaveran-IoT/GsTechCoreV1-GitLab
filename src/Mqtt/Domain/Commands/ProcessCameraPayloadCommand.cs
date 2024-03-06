using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessCameraPayloadCommand(ProcessCameraPayloadDto Dto) : IRequest<IResult<ProcessCameraPayloadCommandResponse>>;
public sealed record ProcessCameraPayloadCommandResponse();