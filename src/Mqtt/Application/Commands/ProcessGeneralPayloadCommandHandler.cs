using MediatR;

using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Internals;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using Broker = Moshaveran.Mqtt.DataAccess.DataSources.DbModels.GeneralBroker;
using Command = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessGeneralPayloadCommand;
using Response = Moshaveran.GsTech.Mqtt.Domain.Commands.ProcessGeneralPayloadCommandResponse;

namespace Moshaveran.GsTech.Mqtt.Application.Commands;

public sealed class ProcessGeneralPayloadCommandHandler(IListenerService listener, IRepository<Broker> cameraRepo)
    : ProcessPayloadCommandHandlerBase(listener)
    , IRequestHandler<Command, IResult<Response>>
{
    public async Task<IResult<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Check.MustBeArgumentNotNull(request);

        var args = request.Dto;
        var processResult = await this.Save(broker =>
        {
            if (string.IsNullOrEmpty(broker.InternetTotalVolume))
            {
                return IResult<Broker>.Failed!;
            }
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.InternetRemainingUssd = broker.InternetTotalVolume;
            var ussd = StringHelper.HexToUnicode(broker.InternetTotalVolume);
            if (ussd.Contains("صفر"))
            {
                broker.InternetTotalVolume = "بدون بسته";
                broker.InternetRemainingTime = "---";
                var match = ussd.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
                broker.InternetRemainingVolume = string.Concat(match, " ", "ریال");
            }
            else
            {
                broker.InternetTotalVolume = ussd.Split(":")[0].Trim();
                broker.InternetRemainingVolume = ussd.Split(":")[1].Trim().Split("،")[0].Trim();
                broker.InternetRemainingTime = ussd.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            }
            return IResult.Success(broker);
        }, args, cameraRepo);
        return processResult.WithValue(new Response());
    }
}