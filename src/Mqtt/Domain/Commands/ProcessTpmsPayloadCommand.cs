using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessTpmsPayloadCommand(ProcessTempPayloadDto Dto) : IRequest<IResult<ProcessTpmsPayloadCommandResponse>>;
public sealed record ProcessTpmsPayloadCommandResponse();