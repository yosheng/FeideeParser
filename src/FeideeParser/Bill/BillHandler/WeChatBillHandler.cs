using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FeideeParser.Models;
using FeideeParser.Models.Bills;

namespace FeideeParser.Bill.BillHandler;

public class WeChatBillHandler : BillHandlerBase
{
    public override FeideeBill GetFeideeBill(Stream stream)
    {
        var configuration = new CsvConfiguration(CultureInfo.CurrentUICulture)
        {
            TrimOptions = TrimOptions.Trim,
            HasHeaderRecord = true,
            ShouldSkipRecord = recordArgs =>
            {
                if (recordArgs.Row[0] == null) return false;
                return recordArgs.Row.Parser.RawRecord.Contains(",,,,,,,,");
            }
        };

        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, configuration);
        csv.Read();
        csv.ReadHeader();
        var records = new List<WeChatBill>();

        while (csv.Parser.Read())
        {
            if (csv.Parser.Record == null)
            {
                continue;
            }

            records.Add(csv.GetRecord<WeChatBill>());
        }

        var groupDataList = records.GroupBy(x => x.TradingBehavior != "支出")
            .ToLookup(g => g.Key, g => g.ToList());

        var exportTemplate = new FeideeBill();

        foreach (var group in groupDataList)
        {
            // 收入
            if (group.Key)
            {
                foreach (var item in group)
                {
                    exportTemplate.Income = item.Select(x => new FeideeBillItem()
                    {
                        TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                        Category = "其他收入",
                        SubCategory = "经营所得",
                        SourceAccount = "微信钱包",
                        Amount = decimal.Parse(GetAmount(x.Amount)),
                        Remark = $"{x.Counterparty}-{x.Merchandise}",
                    }).ToList();
                }
            }

            // 支出
            else
            {
                foreach (var item in group)
                {
                    exportTemplate.Outgo = item.Select(x => new FeideeBillItem
                    {
                        TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                        Category = "其他杂项",
                        SubCategory = "其他支出",
                        SourceAccount = "微信钱包",
                        Amount = decimal.Parse(GetAmount(x.Amount)),
                        Remark = $"{x.Counterparty}-{x.Merchandise}",
                    }).ToList();
                }
            }
        }

        return exportTemplate;
    }

    private string GetAmount(string amount)
    {
        var filter = new Regex(@"\d+(,\d+)*(\.\d+)?");
        var match = filter.Match(amount);
        return match.Success ? match.Value : "0";
    }
}