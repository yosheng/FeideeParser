using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using FeideeParser.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FeideeParser.Web.Pages;

public class IndexBase : BootstrapBlazorRoot
{
    [Inject] public IBillService BillService { get; set; }
    
    [Inject]
    private SwalService SwalService { get; set; }
    
    [Inject]
    private DownloadService DownloadService { get; set; }

    protected ParseBill ParseBill { get; set; } = new ParseBill();
    
    protected List<IValidator> CustomerRules { get; } = new();
    
    private IEnumerable<BillTypeItem> _billTypeItems;

    protected override void OnInitialized()
    {
        _billTypeItems = BillService.GetImportTypeList();
    }

    protected async Task OnSubmit(EditContext context)
    {
        var modal = (ParseBill)context.Model;
        var billTypeItem = _billTypeItems.First(x => modal.BillType != null && x.Value == (int)modal.BillType);

        if (Path.GetExtension(modal.UploadFile!.Name) != $".{billTypeItem.ValidType}")
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Title = "上传档案类型错误",
                Content = billTypeItem.Description,
                ShowClose = true
            });
            return;
        }

        using var ms = new MemoryStream();
        await modal.UploadFile.OpenReadStream().CopyToAsync(ms);
        await using var billFileStream = new MemoryStream(BillService.GetParseBillFile(modal.BillType!.Value, ms));
        await DownloadService.DownloadFromStreamAsync(new DownloadOption()
        {
            FileStream = billFileStream,
            FileName = $"{DateTime.Now:yyyyMMdd}-{billTypeItem.Name}帐单.xls"
        });
    }
}

public class ParseBill
{
    [Required] [Display(Name = "帐单类型")] public BillType? BillType { get; set; }

    [Required] [Display(Name = "上传档案")] public IBrowserFile? UploadFile { get; set; }
}