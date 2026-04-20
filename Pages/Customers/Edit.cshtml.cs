using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class EditModel(ICustomerService customerService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public CustomerInputViewModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var customer = await customerService.GetByIdAsync(Id, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        Input = new CustomerInputViewModel
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var updated = await customerService.UpdateAsync(Id, Input, cancellationToken);
        return updated ? RedirectToPage("Index") : NotFound();
    }
}
