using ftpms.DTOs;
using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ftpms.Pages.Measurements;

public class IndexModel(IMeasurementService measurementService, ICustomerService customerService) : PageModel
{
    public List<MeasurementDto> Measurements { get; private set; } = [];
    public List<SelectListItem> CustomerOptions { get; private set; } = [];
    public List<SelectListItem> TemplateOptions { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public int? CustomerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? TemplateType { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var customers = await customerService.GetAllAsync(cancellationToken);
        CustomerOptions = customers
            .Select(c => new SelectListItem($"{c.FirstName} {c.LastName}", c.Id.ToString(), CustomerId == c.Id))
            .ToList();

        TemplateOptions = MeasurementTemplates.All
            .Select(t => new SelectListItem(t, t, string.Equals(TemplateType, t, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Measurements = await measurementService.GetAllAsync(CustomerId, TemplateType, cancellationToken);
    }
}
