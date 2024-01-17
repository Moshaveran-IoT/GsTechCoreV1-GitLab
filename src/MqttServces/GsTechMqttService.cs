using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GsTechCoreV1.Application.Interfaces.Contexts;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace MqttServices;

public class GsTechMqttService :
    IMqttServerConnectionValidator,
    IMqttServerSubscriptionInterceptor,
    IMqttServerClientConnectedHandler,
    IMqttServerClientDisconnectedHandler
{
    #region MQTT Service & Server Configuration

    #region Variable Declarations

    public List<string> connectedClientIds = [];

    public IMqttServer Server;

    // Default Variable Initialization
    //private readonly AppSettings _appSettings;
    public List<Task> Tasks = [];

    private static readonly string _newLine = Environment.NewLine;
    private readonly ILogger<GsTechMqttService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    #endregion Variable Declarations

    public GsTechMqttService(ILogger<GsTechMqttService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public void ConfigureMqttServer(IMqttServer mqtt)
    {
        Server = mqtt;
        mqtt.ClientConnectedHandler = this;
        mqtt.ClientDisconnectedHandler = this;
    }

    public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
    {
        // Configure the MQTT Server options here
        options.WithoutDefaultEndpoint();
        options.WithDefaultEndpointPort(1885);
        options.WithConnectionValidator(this);
        options.WithSubscriptionInterceptor(this);
        // Enable Attribute Routing
        // By default, messages published to topics that don't match any routes are rejected.
        // Change this to true to allow those messages to be routed without hitting any controller actions.
        options.WithAttributeRouting(true);
    }

    #endregion MQTT Service & Server Configuration

    #region Validation & Interception

    public Task ValidateConnectionAsync(MqttConnectionValidatorContext context)
    {
        context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
        if (context.ClientId.Length == 15 && context.Username.Equals("root") && context.Password.Equals(context.ClientId[10..]))
        {
            context.ReasonCode = MqttConnectReasonCode.Success;
        }
        return Task.Run(() =>
        {
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " + "ValidateConnectionAsync Handler Triggered");
        });
    }

    //public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    //{
    //    return Task.Run(() =>
    //    {
    //        Console.WriteLine(
    //            $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - Received MQTT Message Logged:{_newLine}" +
    //            $"- Topic = {eventArgs.ApplicationMessage.Topic + _newLine}" +
    //            $"- Payload = {Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload) + _newLine}" +
    //            $"- QoS = {eventArgs.ApplicationMessage.QualityOfServiceLevel + _newLine}" +
    //            $"- Retain = {eventArgs.ApplicationMessage.Retain + _newLine}");
    //    });
    //}

    #endregion Validation & Interception

    #region Handle Server Actions

    //public Task HandleServerStartedAsync(EventArgs eventArgs)
    //{
    //    return Task.Run(() =>
    //    {
    //        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
    //                          "HandleServerStartedAsync Handler Triggered");
    //    });

    //}

    //public Task HandleServerStoppedAsync(EventArgs eventArgs)
    //{
    //    return Task.Run(() =>
    //    {
    //        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
    //                          "HandleServerStoppedAsync Handler Triggered");
    //    });
    //}

    #endregion Handle Server Actions

    #region Handle Client Actions

    public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
                              "HandleClientConnectedAsync Handler Triggered");

            if (connectedClientIds.Count == 0)
                SubscribeKiss();

            var clientId = eventArgs.ClientId;
            connectedClientIds.Add(clientId);

            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
                              $"MQTT Client Connected:{_newLine} - ClientID = {clientId + _newLine}");
        });
    }

    public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
                                                  "HandleClientDisconnectedAsync Handler Triggered");

            var clientId = eventArgs.ClientId;
            connectedClientIds.Remove(clientId);

            Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
                              $"MQTT Client Disconnected:{_newLine} - ClientID = {clientId + _newLine}");
        });
    }

    //public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
    //{
    //   // Server.UnsubscribeAsync(eventArgs.ClientId, eventArgs.TopicFilter.Topic);
    //    return Task.Run(() =>
    //    {
    //        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " + "ClientSubscribedTopicHandler Handler Triggered");
    //    });
    //}

    //public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
    //{
    //    return Task.Run(() =>
    //    {
    //        Console.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - " +
    //                          "ClientSubscribedTopicHandler Handler Triggered");
    //    });
    //}

    #endregion Handle Client Actions

    #region Subscribe Topics

    public async Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context)
    {
        bool ProcessSubscription = true;
        string topic = context.TopicFilter.Topic;
        switch (true)
        {
            case bool _ when Regex.IsMatch(topic, @"Gs/(#IMEI#)/(Action|RF|Result)$".Replace("#IMEI#", context.ClientId)): break;
            case bool _ when Regex.IsMatch(topic, @"(AliveMessage)$"): break;
            default: ProcessSubscription = false; break;
        }
        context.AcceptSubscription = ProcessSubscription;
        await Task.CompletedTask;
    }

    public void SubscribeKiss()
    {
        Task.Run(async () =>
        {
            var msg = new MqttApplicationMessageBuilder().WithPayload($"MQTTnet hosted on GsTech has started up!").WithTopic("AliveMessage");

            while (connectedClientIds.Count > 0)
                try
                {
                    await Server.PublishAsync(msg.Build());
                    msg.WithPayload($"MQTTnet hosted on GsTech is still running at {DateTime.Now}!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
        });
    }

    #endregion Subscribe Topics
}