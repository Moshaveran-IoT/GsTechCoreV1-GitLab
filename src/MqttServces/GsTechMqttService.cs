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


namespace MqttServices
{
    public class MqttTask
    {
        public string Topic { get; set; }
        public string Payload { get; set; }
    }
    public class GsTechMqttService :
        IMqttServerConnectionValidator,
        IMqttServerSubscriptionInterceptor,
        IMqttServerClientConnectedHandler,
        IMqttServerClientDisconnectedHandler
    {

        public List<Task> Tasks = new();
        private readonly ILogger<GsTechMqttService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly string _newLine = Environment.NewLine;
        public IMqttServer Server;
        public List<string> connectedClientIds = new();

        public GsTechMqttService(ILogger<GsTechMqttService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }


        public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
        {
            options.WithoutDefaultEndpoint();
            options.WithDefaultEndpointPort(1885);
            options.WithConnectionValidator(this);
            options.WithSubscriptionInterceptor(this);
            options.WithAttributeRouting(true);
        }

        public void ConfigureMqttServer(IMqttServer mqtt)
        {
            Server = mqtt;
            mqtt.ClientConnectedHandler = this;
            mqtt.ClientDisconnectedHandler = this;
        }


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

        public async Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context)
        {
            var ProcessSubscription = true;
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
    }
}