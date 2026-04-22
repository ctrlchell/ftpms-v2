using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ftpms.Pages.Measurements;

public class EditModel(IMeasurementService measurementService, ICustomerService customerService) : PageModel
{
    [BindProperty]
    public MeasurementInputViewModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public List<SelectListItem> CustomerOptions { get; private set; } = [];
    public List<SelectListItem> TemplateOptions { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var measurement = await measurementService.GetByIdAsync(Id, cancellationToken);
        if (measurement is null)
        {
            return NotFound();
        }

        Input = new MeasurementInputViewModel
        {
            CustomerId = measurement.CustomerId,
            TemplateType = measurement.TemplateType,
            Version = measurement.Version,
            DateTaken = measurement.DateTaken,
            Notes = measurement.Notes,
            IsActive = measurement.IsActive,
            ParentMeasurementId = measurement.ParentMeasurementId,
            Chest = measurement.Chest,
            Waist = measurement.Waist,
            Hip = measurement.Hip,
            Shoulder = measurement.Shoulder,
            Neck = measurement.Neck,
            SleeveLength = measurement.SleeveLength,
            ArmRound = measurement.ArmRound,
            Wrist = measurement.Wrist,
            Bicep = measurement.Bicep,
            TopLength = measurement.TopLength,
            TrouserLength = measurement.TrouserLength,
            Thigh = measurement.Thigh,
            Knee = measurement.Knee,
            Ankle = measurement.Ankle,
            Inseam = measurement.Inseam,
            BustPoint = measurement.BustPoint,
            UnderBust = measurement.UnderBust,
            RoundSleeve = measurement.RoundSleeve,
            GownLength = measurement.GownLength,
            SkirtLength = measurement.SkirtLength
        };

        await LoadSelectionsAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        await LoadSelectionsAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (updated, errors) = await measurementService.UpdateAsync(Id, Input, cancellationToken);

        foreach (var (key, messages) in errors)
        {
            foreach (var message in messages)
            {
                ModelState.AddModelError($"Input.{key}", message);
            }
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        return updated ? RedirectToPage("Details", new { id = Id }) : NotFound();
    }

    private async Task LoadSelectionsAsync(CancellationToken cancellationToken)
    {
        var customers = await customerService.GetAllAsync(cancellationToken);
        CustomerOptions = customers
            .Select(c => new SelectListItem($"{c.FirstName} {c.LastName}", c.Id.ToString(), c.Id == Input.CustomerId))
            .ToList();

        TemplateOptions = MeasurementTemplates.All
            .Select(t => new SelectListItem(t, t, string.Equals(t, Input.TemplateType, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
