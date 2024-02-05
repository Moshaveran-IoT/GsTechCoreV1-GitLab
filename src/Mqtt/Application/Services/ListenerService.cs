﻿using Application.Interfaces;

using Google.Protobuf.WellKnownTypes;

using Grpc.Net.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace Application.Services;

public sealed class ListenerService : IListenerService
{
    private readonly MqqtReceiveService.MqqtReceiveServiceClient _grpcClient;
    private readonly ILogger<ListenerService> _logger;

    public ListenerService(IConfiguration configuration, ILogger<ListenerService> logger, ILoggerFactory loggerFactory)
    {
        _grpcClient = createGrpcClient();

        MqqtReceiveService.MqqtReceiveServiceClient createGrpcClient()
        {
            var opt = new GrpcChannelOptions
            {
                LoggerFactory = loggerFactory,
            };
            var channel = GrpcChannel.ForAddress(configuration["GrpcServiceSettings:ServerUrl"]!, opt);
            return new MqqtReceiveService.MqqtReceiveServiceClient(channel);
        }

        this._logger = logger;
    }

    public Task LogClientConnectedAsync(string clientId, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clientId);
        try
        {
            _ = _grpcClient.ClientConnectedAsync(new()
            {
                ClientId = clientId,
                Time = Now()
            }, cancellationToken: token);
        }
        catch
        {
            //_logger.LogDebug(ex, "Exception on {method}", nameof(LogClientConnectedAsync));
        }
        return Task.CompletedTask;
    }

    public Task LogClientDisconnectedAsync(string clientId, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clientId);
        try
        {
            _ = _grpcClient.ClientDisconnectedAsync(new()
            {
                ClientId = clientId,
                Time = Now()
            }, cancellationToken: token);
        }
        catch
        {
            //_logger.LogDebug(ex, "Exception on {method}", nameof(LogClientDisconnectedAsync));
        }
        return Task.CompletedTask;
    }

    public Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(args);
        try
        {
            var grpcRequest = new PayloadReceivedParams
            {
                ClientID = args.ClientId,
                IMEI = args.Imei,
                Time = Now(),
                BrokerType = typeof(TBroker).Name,
                Log = args.LogMessage,
                SaveStatus = args.Status,
            };
            _ = this._grpcClient.PayloadReceivedAsync(grpcRequest, cancellationToken: token);
        }
        catch
        {
            //_logger.LogDebug(ex, "Exception on {method}", nameof(LogPayloadReceivedAsync));
        }
        return Task.CompletedTask;
    }

    private static Timestamp Now() => Timestamp.FromDateTime(DateTime.UtcNow);
}