using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;
public sealed record ProcessGpsPayloadCommand(ProcessGpsPayloadDto Dto) : IRequest<IResult<ProcessGpsPayloadCommandResponse>>;
public sealed record ProcessGpsPayloadCommandResponse();