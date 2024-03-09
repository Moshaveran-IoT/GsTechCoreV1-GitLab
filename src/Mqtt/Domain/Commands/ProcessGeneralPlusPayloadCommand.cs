using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessGeneralPlusPayloadCommand(ProcessGeneralPlusPayloadDto Dto) : IRequest<IResult<ProcessGeneralPlusPayloadCommandResponse>>;
public sealed record ProcessGeneralPlusPayloadCommandResponse();