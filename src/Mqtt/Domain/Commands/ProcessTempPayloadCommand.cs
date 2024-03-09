using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessTempPayloadCommand(ProcessTempPayloadDto Dto) : IRequest<IResult<ProcessTempPayloadCommandResponse>>;
public sealed record ProcessTempPayloadCommandResponse();