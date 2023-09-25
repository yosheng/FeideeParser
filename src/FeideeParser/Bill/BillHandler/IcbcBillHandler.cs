using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FeideeParser.Helper;
using FeideeParser.Models;
using FeideeParser.Models.Bills;
using Ganss.Excel;

namespace FeideeParser.Bill.BillHandler
{
    public class IcbcBillHandler : BillHandlerBase
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
                    return recordArgs.Row.Parser.RawRecord.Contains(",,,,,,,,,,");
                }
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, configuration);
            csv.Read();
            csv.ReadHeader();
            var records = new List<IcbcBill>();

            while (csv.Parser.Read())
            {
                if (csv.Parser.Record == null)
                {
                    continue;
                }

                records.Add(csv.GetRecord<IcbcBill>());
            }

            #region 处理元数据

            records.ForEach(x =>
            {
                x.TransactionDate = TrimContent(x.TransactionDate);
                x.Summary = TrimContent(x.Summary);
                x.Description = TrimContent(x.Description);
                x.Region = TrimContent(x.Region);
                x.Unit = TrimContent(x.Unit);
                x.TransactionIncome = TrimContent(x.TransactionIncome);
                x.TransactionOutCome = TrimContent(x.TransactionOutCome);
                x.TransactionCurrency = TrimContent(x.TransactionCurrency);
                x.Income = TrimContent(x.Income);
                x.Outcome = TrimContent(x.Outcome);
                x.Currency = TrimContent(x.Currency);
                x.AccountingCurrencyBalance = TrimContent(x.AccountingCurrencyBalance);
                x.TransactionAccount = TrimContent(x.TransactionAccount);
            });
            records = records.Where(x => !string.IsNullOrWhiteSpace(x.TransactionDate)).ToList();

            #endregion

            var groupDataList = records.GroupBy(x => !string.IsNullOrEmpty(x.Income))
                .ToLookup(g => g.Key, g => g.ToList());

            var exportTemplate = new FeideeBill();

            foreach (var group in groupDataList)
            {
                // 收入
                if (group.Key)
                {
                    foreach (var items in group)
                    {
                        exportTemplate.Income = items.Select(x => new FeideeBillItem()
                        {
                            TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            Category = "职业收入",
                            SubCategory = "利息收入",
                            SourceAccount = "中国工商银行",
                            Amount = decimal.Parse(TrimContent(x.Income)),
                            Remark = x.Description,
                        }).ToList();
                    }
                }

                // 支出或转帐
                else
                {
                    foreach (var items in group)
                    {
                        // 转帐: 找出支付宝或微信支付纪录
                        exportTemplate.Transfer = items
                            .Where(x => x.Description.Contains("支付宝") || x.Description.Contains("财付通"))
                            .Select(x =>
                                new FeideeBillItem
                                {
                                    TransactionDateTime =
                                        DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                                    SourceAccount = "中国工商银行",
                                    TargetAccount = BillHelper.GetTargetAccount(x.Description),
                                    Amount = decimal.Parse(x.Outcome)
                                }).ToList();

                        // 支出
                        exportTemplate.Outgo = items
                            .Where(x => !x.Description.Contains("支付宝") && !x.Description.Contains("财付通"))
                            .Select(x =>
                                new FeideeBillItem
                                {
                                    TransactionDateTime =
                                        DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                                    Category = "其他杂项",
                                    SubCategory = "其他支出",
                                    SourceAccount = "中国工商银行",
                                    Amount = decimal.Parse(x.Outcome),
                                    Remark = x.Description,
                                }).ToList();
                    }
                }
            }

            return exportTemplate;
        }

        private string TrimContent(string content)
        {
            return content.Replace("\"", "").TrimEnd('\t').Trim();
        }
    }
}