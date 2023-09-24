using CsvHelper.Configuration.Attributes;

namespace FeideeParser.Models.Bills;

/// <summary>
/// 微信帐单
/// </summary>
[Delimiter(",")]
public class WeChatBill
{
    /// <summary>
    /// 交易时间
    /// </summary>
    [Name("交易时间")]
    public string TransactionDate { get; set; }

    /// <summary>
    /// 交易类型
    /// </summary>
    [Name("交易类型")]
    public string TransactionType { get; set; }

    /// <summary>
    /// 交易对方
    /// </summary>
    [Name("交易对方")]
    public string Counterparty { get; set; }

    /// <summary>
    /// 商品
    /// </summary>
    [Name("商品")]
    public string Merchandise { get; set; }

    /// <summary>
    /// 收/支
    /// </summary>
    [Name("收/支")]
    public string TradingBehavior { get; set; }

    /// <summary>
    /// 金额(元)
    /// </summary>
    [Name("金额(元)")]
    public string Amount { get; set; }

    /// <summary>
    /// 支付方式
    /// </summary>
    [Name("支付方式")]
    public string PayMethod { get; set; }

    /// <summary>
    /// 当前状态
    /// </summary>
    [Name("当前状态")]
    public string CurrentStatus { get; set; }

    /// <summary>
    /// 交易单号
    /// </summary>
    [Name("交易单号")]
    public string TransactionNo { get; set; }

    /// <summary>
    /// 商户单号
    /// </summary>
    [Name("商户单号")]
    public string MerchantNo { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Name("备注")]
    public string Remark { get; set; }
}