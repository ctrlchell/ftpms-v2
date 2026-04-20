using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class IndexModel(ICustomerService customerService) : PageModel
{
    public List<CustomerDto> Customers { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Customers = await customerService.GetAllAsync(cancellationToken);
    }
}
