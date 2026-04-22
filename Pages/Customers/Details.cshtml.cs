using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Customers;

public class DetailsModel(ICustomerService customerService, IMeasurementService measurementService) : PageModel
{
    public CustomerDto? Customer { get; private set; }
    public List<MeasurementHistoryItemDto> MeasurementHistory { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Customer = await customerService.GetByIdAsync(id, cancellationToken);
        if (Customer is null)
        {
            return NotFound();
        }

        MeasurementHistory = await measurementService.GetHistoryForCustomerAsync(id, cancellationToken: cancellationToken);
        MeasurementHistory = MeasurementHistory.Take(5).ToList();

        return Page();
    }
}
