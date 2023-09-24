using FeideeParser.Bill;
using FeideeParser.Helper;
using FeideeParser.Models;

namespace FeideeParser;

public class BillService : IBillService
{
    public IEnumerable<BillTypeItem> GetImportTypeList()
    {
        var list = EnumHelper.GetEnumItem(typeof(BillType)).Select(x => new BillTypeItem()
        {
            Value = x.Key,
            Name = x.Value.Item1,
            Description = x.Value.Item2,
            ValidType = x.Value.Item3
        }).ToList();

        return list;
    }

    public byte[] GetParseBillFile(BillType billType, Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var feideeBill = ExportBillFactory.CreateBillHandler(billType).GetFeideeBill(stream);

        return BillHelper.GenerateExcelByte(feideeBill);
    }
}