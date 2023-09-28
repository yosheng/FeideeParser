using FeideeParser.Bill;
using FeideeParser.Models;

namespace FeideeParser.Tests.Bill;

public class ExportBillFactoryTest
{
    [Theory]
    [InlineData("Wechat.csv", BillType.WeChat)]
    [InlineData("Alipay.csv", BillType.AliPay)]
    [InlineData("AbChina.xls", BillType.AbChina)]
    [InlineData("Icbc.csv", BillType.Icbc)]
    [InlineData("Cmb.csv", BillType.Cmb)]
    public void ParseSuiBill(string fileName, BillType billType)
    {
        System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", fileName);
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var feideeBill = ExportBillFactory.CreateBillHandler(billType).GetFeideeBill(fs);
        Assert.NotNull(feideeBill);
    }
}