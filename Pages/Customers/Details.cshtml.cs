using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class DetailsModel(ICustomerService customerService) : PageModel
{
    public CustomerDto? Customer { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Customer = await customerService.GetByIdAsync(id, cancellationToken);
        return Customer is null ? NotFound() : Page();
    }
}
