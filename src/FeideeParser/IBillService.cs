using FeideeParser.Models;

namespace FeideeParser;

public interface IBillService
{
    IEnumerable<BillTypeItem> GetImportTypeList();
    
    byte[] GetParseBillFile(BillType billType, Stream stream);
}