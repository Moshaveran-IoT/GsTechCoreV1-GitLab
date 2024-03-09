using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;
public sealed record ProcessObdPayloadCommand(ProcessObdPayloadDto Dto) : IRequest<IResult<ProcessObdPayloadCommandResponse>>;
public sealed record ProcessObdPayloadCommandResponse();