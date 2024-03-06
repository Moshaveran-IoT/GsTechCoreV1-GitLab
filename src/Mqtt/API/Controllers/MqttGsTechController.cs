﻿using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Services;
using Moshaveran.GsTech.Mqtt.Domain.Commands;
using Moshaveran.Mqtt.Domain.Services;

using MQTTnet.AspNetCore.AttributeRouting;

using IResult = Moshaveran.Library.Results.IResult;

namespace Moshaveran.GsTech.Mqtt.API.Controllers;

[MqttController]
[MqttRoute("Gs")]
public sealed class MqttGsTechController(GsTechMqttService service, IMediator mediator) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public Task CAN(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessCanPayload, "CAN", IMEI, token);

    [MqttRoute("{IMEI}/General")]
    public Task General(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessGeneralPayload, "General", IMEI, token);

    [MqttRoute("{IMEI}/GeneralPlus")]
    public Task GeneralPlus(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessGeneralPlusPayload, "General Plus", IMEI, token);

    [MqttRoute("{IMEI}/GPS")]
    public Task GPS(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessGpsPayload, "GPS", IMEI, token);

    [MqttRoute("{IMEI}/Image")]
    public Task ProcessCameraPayload(string IMEI, CancellationToken token = default)
        //=> this.ProcessServiceMethod(service.ProcessCameraPayload, "Camera", IMEI, token);
        => mediator.Send(new ProcessCameraPayloadCommand(new(this.Message.Payload, this.MqttContext.ClientId, IMEI)), token);

    [MqttRoute("{IMEI}/OBD")]
    public Task OBD(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessObdPayload, "OBD", IMEI, token);

    [MqttRoute("{IMEI}/Signal")]
    public Task Signal(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessSignalPayload, "Signal", IMEI, token);

    [MqttRoute("{IMEI}/Temp")]
    public Task Temp(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessTemperaturePayload, "Temp", IMEI, token);

    [MqttRoute("{IMEI}/TPMS")]
    public Task TPMS(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessTpmsPayload, "TPMS", IMEI, token);

    [MqttRoute("{IMEI}/Voltage")]
    public Task Voltage(string IMEI, CancellationToken token = default)
        => this.ProcessServiceMethod(service.ProcessVoltagePayload, "Voltage", IMEI, token);

    private async Task ProcessServiceMethod(Func<ProcessPayloadArgs, Task<IResult>> method, string _, string imei, CancellationToken token = default)
    {
        var result = await method(new(this.Message.Payload, this.MqttContext.ClientId, imei, token));
        if (result.IsSucceed)
        {
            await this.Ok();
        }
        else
        {
            await this.BadMessage();
        }
    }
}