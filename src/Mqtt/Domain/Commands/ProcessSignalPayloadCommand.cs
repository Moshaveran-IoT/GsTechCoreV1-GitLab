﻿using MediatR;

using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Results;

namespace Moshaveran.GsTech.Mqtt.Domain.Commands;

public sealed record ProcessSignalPayloadCommand(ProcessObdPayloadDto Dto) : IRequest<IResult<ProcessSignalPayloadCommandResponse>>;
public sealed record ProcessSignalPayloadCommandResponse();