using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Domain.Dtos;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;

using System.Text;

namespace Moshaveran.GsTech.Mqtt.Application.Internals;

public abstract class ProcessPayloadCommandHandlerBase(IListenerService listenerService)
{
    protected Task<IResult> Save<TDbBroker, TDto>(Func<string, Task<IResult<IEnumerable<TDbBroker>>>> initialize, TDto args, IRepository<TDbBroker> repo) where TDto : ProcessPayloadDto
        => this.InnerSave(initialize, args, repo);

    protected Task<IResult> Save<TDbBroker, TDto>(Func<TDbBroker, string, Task<IResult<TDbBroker>>> initialize, TDto args, IRepository<TDbBroker> repo) where TDto : ProcessPayloadDto
        => this.InnerSave(payloadMessage =>
        {
            return !StringHelper.TryParseJson(payloadMessage, out TDbBroker? broker) || broker == null
                ? IResult.Fail<IEnumerable<TDbBroker>>([], "Invalid JSON format.").ToAsync()
                : initialize(broker, payloadMessage).WithValue(EnumerableHelper.ToEnumerable);
        }, args, repo);

    protected Task<IResult> Save<TDbBroker, TDto>(Func<TDbBroker, IResult<TDbBroker>> initialize, TDto args, IRepository<TDbBroker> repo) where TDto : ProcessPayloadDto
        => this.InnerSave(payloadMessage =>
        {
            return !StringHelper.TryParseJson(payloadMessage, out TDbBroker? broker) || broker == null
                ? IResult.Fail<IEnumerable<TDbBroker>>([], "Invalid JSON format.").ToAsync()
                : initialize(broker).WithValue(EnumerableHelper.ToEnumerable).ToAsync();
        }, args, repo);

    private async Task<IResult> InnerSave<TDbBroker, TDto>(Func<string, Task<IResult<IEnumerable<TDbBroker>>>> initialize, TDto dto, IRepository<TDbBroker> repo) where TDto : ProcessPayloadDto
    {
        var (status, logMessage) = (SaveStatus.SaveSuccess, string.Empty);

        try
        {
            (var result, (status, logMessage)) = await save(initialize, dto, repo);
            return result;
        }
        catch (Exception ex)
        {
            (status, logMessage) = (SaveStatus.SaveFailure, ex.GetBaseException().Message);
            return IResult.Failed;
        }
        finally
        {
            await sendToListener(dto, status, logMessage);
        }

        static async Task<IResult<(SaveStatus Status, string LogMessage)>> save(Func<string, Task<IResult<IEnumerable<TDbBroker>>>> initialize, TDto args, IRepository<TDbBroker> repo)
        {
            (SaveStatus Status, string LogMessage) value;
            var payloadMessage = Encoding.UTF8.GetString(args.Payload);
            var initBrokers = await initialize(payloadMessage);
            if (initBrokers.IsFailure || initBrokers.Value?.Any() is not true)
            {
                value = (SaveStatus.InvalidRequest, initBrokers.Message ?? "Invalid payload format.");
                return initBrokers.WithValue(value);
            }

            foreach (var broker in initBrokers.Value)
            {
                var insertResult = await repo.Insert(broker, false);
                if (!insertResult.IsSucceed)
                {
                    value = (SaveStatus.SaveFailure, insertResult.Message ?? "Payload cannot be saved.");
                    return insertResult.WithValue(value);
                }
            }
            var saveResult = await repo.SaveChanges();

            value = saveResult.Process(onSucceed: r => (SaveStatus.SaveSuccess, r!.Message ?? "Payload is saved successfully.")
                                     , onFailure: r => (SaveStatus.SaveFailure, r!.Message ?? "Payload cannot be saved."));

            return saveResult.WithValue(value);
        }

        Task sendToListener(TDto args, SaveStatus status, string logMessage)
            => listenerService.LogPayloadReceivedAsync(new LogPayloadReceivedArgs<TDbBroker>(args.ClientId, args.Imei, logMessage, status));
    }
}