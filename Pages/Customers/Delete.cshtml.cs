using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class DeleteModel(ICustomerService customerService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public CustomerDto? Customer { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Customer = await customerService.GetByIdAsync(Id, cancellationToken);
        return Customer is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var deleted = await customerService.DeleteAsync(Id, cancellationToken);
        return deleted ? RedirectToPage("Index") : NotFound();
    }
}
