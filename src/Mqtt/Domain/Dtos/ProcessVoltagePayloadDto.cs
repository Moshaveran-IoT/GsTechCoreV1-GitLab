﻿namespace Moshaveran.GsTech.Mqtt.Domain.Dtos;

public sealed class ProcessVoltagePayloadDto(in byte[] payload, in string clientId, in string imei) : ProcessPayloadDto(payload, clientId, imei);