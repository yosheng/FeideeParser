using FeideeParser.Bill.BillHandler;
using FeideeParser.Models;

namespace FeideeParser.Bill;

public static class ExportBillFactory
{
    public static BillHandlerBase CreateBillHandler(BillType billType)
    {
        BillHandlerBase billHandler = billType switch
        {
            BillType.WeChat => new WeChatBillHandler(),
            BillType.AliPay => new ALiPayBillHandler(),
            BillType.AbChina => new AbChinaBillHandler(),
            BillType.Icbc => new IcbcBillHandler(),
            _ => throw new ArgumentOutOfRangeException(nameof(billType), billType, null)
        };

        return billHandler;
    }
}