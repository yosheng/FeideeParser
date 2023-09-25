using CsvHelper.Configuration.Attributes;

namespace FeideeParser.Models.Bills
{
    /// <summary>
    /// 工商银行帐单
    /// </summary>
    [Delimiter(",")]
    public class IcbcBill
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [Name("交易日期")]
        public string TransactionDate { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Name("摘要")]
        public string Summary { get; set; }

        /// <summary>
        /// 交易场所
        /// </summary>
        [Name("交易场所")]
        public string Description { get; set; }

        /// <summary>
        /// 交易国家或地区简称
        /// </summary>
        [Name("交易国家或地区简称")]
        public string Region { get; set; }

        /// <summary>
        /// 钞/汇
        /// </summary>
        [Name("钞/汇")]
        public string Unit { get; set; }

        /// <summary>
        /// 交易金额(收入)
        /// </summary>
        [Name("交易金额(收入)")]
        public string TransactionIncome { get; set; }

        /// <summary>
        /// 交易金额(支出)
        /// </summary>
        [Name("交易金额(支出)")]
        public string TransactionOutCome { get; set; }
        
        /// <summary>
        /// 交易币种
        /// </summary>
        [Name("交易币种")]
        public string TransactionCurrency { get; set; }

        /// <summary>
        /// 记账金额(收入)
        /// </summary>
        [Name("记账金额(收入)")]
        public string Income { get; set; }

        /// <summary>
        /// 记账金额(支出)
        /// </summary>
        [Name("记账金额(支出)")]
        public string Outcome { get; set; }

        /// <summary>
        /// 记账币种
        /// </summary>
        [Name("记账币种")]
        public string Currency { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [Name("余额")]
        public string AccountingCurrencyBalance { get; set; }
        
        /// <summary>
        /// 对方户名
        /// </summary>
        [Name("对方户名")]
        public string TransactionAccount { get; set; }
    }
}