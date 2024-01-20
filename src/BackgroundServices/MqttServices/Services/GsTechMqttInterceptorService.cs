using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.Protocol;
using MQTTnet.Server;

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Moshaveran.BackgroundServices.MqttServices.Services;

public sealed class GsTechMqttInterceptorService(ILogger<GsTechMqttInterceptorService> logger, IServiceScopeFactory scopeFactory) :
    IMqttServerConnectionValidator,
    IMqttServerSubscriptionInterceptor,
    IMqttServerClientConnectedHandler,
    IMqttServerClientDisconnectedHandler
{
    private static readonly string _newLine = Environment.NewLine;
    private readonly Collection<string> _connectedClientIds = [];

    private IMqttServer? _server;

    public void ConfigureMqttServer(IMqttServer mqtt)
    {
        this._server = CheckNull(mqtt);
        mqtt.ClientConnectedHandler = this;
        mqtt.ClientDisconnectedHandler = this;
    }

    public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs) => Task.Run(() =>
    {
        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - HandleClientConnectedAsync Handler Triggered");

        if (this._connectedClientIds.Count == 0)
        {
            this.SubscribeKiss();
        }

        var clientId = eventArgs.ClientId;
        this._connectedClientIds.Add(clientId);

        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - MQTT Client Connected:{_newLine} - ClientID = {clientId + _newLine}");
    });

    public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs) => Task.Run(() =>
    {
        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - HandleClientDisconnectedAsync Handler Triggered");

        var clientId = eventArgs.ClientId;
        _ = this._connectedClientIds.Remove(clientId);

        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - MQTT Client Disconnected:{_newLine} - ClientID = {clientId + _newLine}");
    });

    public async Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context)
    {
        _ = CheckNull(context);

        var processSubscription = true;
        var topic = context.TopicFilter.Topic;
        switch (true)
        {
            case bool when Regex.IsMatch(topic, @"Gs/(#IMEI#)/(Action|RF|Result)$".Replace("#IMEI#", context.ClientId)): break;
            case bool when Regex.IsMatch(topic, @"(AliveMessage)$"): break;
            default: processSubscription = false; break;
        }
        context.AcceptSubscription = processSubscription;
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public void SubscribeKiss() => Task.Run(async () =>
    {
        var msg = new MqttApplicationMessageBuilder().WithPayload($"MQTTnet hosted on GsTech has started up!").WithTopic("AliveMessage");

        while (this._connectedClientIds.Count > 0)
        {
            try
            {
                _ = await this._server.PublishAsync(msg.Build()).ConfigureAwait(false);
                _ = msg.WithPayload($"MQTTnet hosted on GsTech is still running at {DateTime.Now}!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            }
        }
    });

    public Task ValidateConnectionAsync(MqttConnectionValidatorContext context)
    {
        _ = CheckNull(context);

        context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
        if (context.ClientId.Length == 15 && context.Username.Equals("root") && context.Password.Equals(context.ClientId[10..]))
        {
            context.ReasonCode = MqttConnectReasonCode.Success;
        }
        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " + "ValidateConnectionAsync Handler Triggered");
        return Task.CompletedTask;
    }

    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
    private static T CheckNull<T>([NotNull][AllowNull] T o)
        where T : class => o ?? throw new NullReferenceException();
}