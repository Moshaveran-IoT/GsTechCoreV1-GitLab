using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.CanBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessCanPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessCanPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessCanPayloadCommandHandler(IListenerService listener, IRepository<Broker> cameraRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save(payloadMessage =>
        {
            IResult<IEnumerable<Broker>> result;
            if (!JsonDocumentHelpers.TryParse(payloadMessage, out var dto) || dto == null)
            {
                result = IResult.Fail<IEnumerable<Broker>>([]);
            }
            else
            {
                using (dto)
                {
                    result = IResult.Success(processDto(args.Imei, dto).Build());
                }
            }

            return Task.FromResult(result);

            static IEnumerable<Broker> processDto(string imei, System.Text.Json.JsonDocument dto)
            {
                const string VALID_HEX_CHARS = "0123456789abcdefABCDEF";
                foreach (var app in dto.RootElement.EnumerateObject())
                {
                    var (key, value) = (app.Name, app.Value.GetRawText().Trim('\"'));
                    var isHex = key.All(VALID_HEX_CHARS.Contains) && value.All(VALID_HEX_CHARS.Contains);
                    if (!isHex)
                    {
                        continue;
                    }
                    var binaryString = string.Concat(key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    //x var Priority = binaryString[..6];
                    var reserved = binaryString[6..7];
                    var dataPage = binaryString[7..8];
                    var pduFormat = binaryString[8..16];
                    //x var sourceAddress = binaryString[24..32];
                    var decPduFormat = Convert.ToInt64(pduFormat, 2);
                    var pduSpecific = decPduFormat >= 240
                        ? binaryString[16..24]
                        : $"000000{reserved}{dataPage}{pduFormat}00000000";
                    var binaryPgn = $"000000{reserved}{dataPage}{pduFormat}{pduSpecific}";
                    var pgn = Convert.ToInt64(binaryPgn, 2);

                    yield return new Broker
                    {
                        CreatedOn = DateTime.Now,
                        Identifier = key,
                        Pgn = pgn,
                        Value = value,
                        Imei = imei
                    };
                }
            }
        }, args, cameraRepo);
        return processResult.WithValue(new Response());
    }
}