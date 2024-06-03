using Group06_Project.Domain.Enums;
using Group06_Project.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group06_Project.RazorPage.Areas.Identity.Pages.Account.Manage;

public class ConfirmTopUp : PageModel
{
    private readonly ILogger<ConfirmTopUp> _logger;
    private readonly IPaymentService _paymentService;
    private readonly IBalanceService _balanceService;
    
    [TempData]
    public string? StatusMessage { get; set; }
    
    public ConfirmTopUp(ILogger<ConfirmTopUp> logger, IPaymentService paymentService, IBalanceService balanceService)
    {
        _logger = logger;
        _paymentService = paymentService;
        _balanceService = balanceService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var query = Request.QueryString.ToString();
        try
        {
            _logger.LogInformation("Query: {0}", query);
            await _paymentService.CheckPayment(query);
            var transactionReference = await _paymentService.GetTransactionReference(query);
            await _balanceService.ConfirmTransactionAsync(transactionReference);
            StatusMessage = "Top up successfully!";
        }
        catch (Exception e)
        {
            StatusMessage = e.Message;
        }
        return Redirect("./Index");
    }
}