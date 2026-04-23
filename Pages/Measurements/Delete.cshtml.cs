using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Measurements;

public class DeleteModel(IMeasurementService measurementService) : PageModel
{
    [BindProperty]
    public MeasurementDto? Measurement { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Measurement = await measurementService.GetByIdAsync(id, cancellationToken);
        return Measurement is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await measurementService.DeleteAsync(id, cancellationToken);
        return deleted ? RedirectToPage("Index") : NotFound();
    }
}
