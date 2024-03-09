using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessCanPayloadCommand(ProcessCameraPayloadDto Dto) : IRequest<IResult<ProcessCanPayloadCommandResponse>>;
public sealed record ProcessCanPayloadCommandResponse();