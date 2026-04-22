using ftpms.DTOs;
using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ftpms.Pages.Measurements;

public class HistoryModel(IMeasurementService measurementService, ICustomerService customerService) : PageModel
{
    public List<MeasurementHistoryItemDto> Items { get; private set; } = [];
    public string CustomerName { get; private set; } = string.Empty;
    public List<SelectListItem> TemplateOptions { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public int CustomerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? TemplateType { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var customer = await customerService.GetByIdAsync(CustomerId, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        CustomerName = $"{customer.FirstName} {customer.LastName}".Trim();
        TemplateOptions = MeasurementTemplates.All
            .Select(t => new SelectListItem(t, t, string.Equals(t, TemplateType, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Items = await measurementService.GetHistoryForCustomerAsync(CustomerId, TemplateType, cancellationToken);
        return Page();
    }
}
