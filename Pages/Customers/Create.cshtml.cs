using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class CreateModel(ICustomerService customerService) : PageModel
{
    [BindProperty]
    public CustomerInputViewModel Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await customerService.CreateAsync(Input, cancellationToken);
        return RedirectToPage("Index");
    }
}
