﻿using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessGeneralPayloadCommand(ProcessGeneralPayloadDto Dto) : IRequest<IResult<ProcessGeneralPayloadCommandResponse>>;
public sealed record ProcessGeneralPayloadCommandResponse();