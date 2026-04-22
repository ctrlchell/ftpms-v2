using ftpms.DTOs;
using ftpms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ftpms.Pages.Measurements;

public class DetailsModel(IMeasurementService measurementService) : PageModel
{
    public MeasurementDto? Measurement { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Measurement = await measurementService.GetByIdAsync(id, cancellationToken);
        return Measurement is null ? NotFound() : Page();
    }
}
