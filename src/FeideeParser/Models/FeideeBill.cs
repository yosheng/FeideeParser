namespace FeideeParser.Models;

public class FeideeBill
{
    public FeideeBill()
    {
        Income = new List<FeideeBillItem>();
        Outgo = new List<FeideeBillItem>();
        Transfer = new List<FeideeBillItem>();
    }
    
    public List<FeideeBillItem> Income { get; set; }

    public List<FeideeBillItem> Outgo { get; set; }

    public List<FeideeBillItem> Transfer { get; set; }
}