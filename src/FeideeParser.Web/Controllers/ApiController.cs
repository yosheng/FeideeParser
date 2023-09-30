using System;
using System.Linq;
using FeideeParser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeideeParser.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private readonly IBillService _billService;

    public ApiController(IBillService billService)
    {
        _billService = billService;
    }

    /// <summary>
    /// 下载转换帐单
    /// </summary>
    /// <param name="billType">原始帐单类型</param>
    /// <param name="file">原始帐单档案</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    [HttpPost]
    public FileResult DownloadParseBillFile(BillType billType, IFormFile file)
    {
        if (file == null)
        {
            throw new ArgumentException("请上传原始帐单!");
        }

        var billTypeItems = _billService.GetImportTypeList();
        var fileBytes = _billService.GetParseBillFile(billType, file.OpenReadStream());
        return File(fileBytes, "application/octet-stream",
            $"{DateTime.Now:yyyyMMdd}-{billTypeItems.First(x => x.Value == (int)billType).Name}帐单.xls");
    }
}