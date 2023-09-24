using FeideeParser.Models;

namespace FeideeParser.Bill;

public abstract class BillHandlerBase
{
    public abstract FeideeBill GetFeideeBill(Stream stream);
}