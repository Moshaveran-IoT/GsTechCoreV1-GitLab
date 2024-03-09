using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessVoltagePayloadCommand(ProcessVoltagePayloadDto Dto) : IRequest<IResult<ProcessVoltagePayloadCommandResponse>>;
public sealed record ProcessVoltagePayloadCommandResponse();