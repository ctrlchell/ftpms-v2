using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ftpms.Pages.Measurements;

public class CreateModel(IMeasurementService measurementService, ICustomerService customerService) : PageModel
{
    [BindProperty]
    public MeasurementInputViewModel Input { get; set; } = new();

    public List<SelectListItem> CustomerOptions { get; private set; } = [];
    public List<SelectListItem> TemplateOptions { get; private set; } = [];
    public bool IsPrefilled { get; private set; }
    public int? SourceVersion { get; private set; }

    [BindProperty(SupportsGet = true)]
    public int? CustomerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? TemplateType { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadSelectionsAsync(cancellationToken);

        if (CustomerId.HasValue && !string.IsNullOrWhiteSpace(TemplateType))
        {
            var prefill = await measurementService.PreparePrefilledCreateModelAsync(CustomerId.Value, TemplateType, cancellationToken);
            Input = prefill.Model;
            IsPrefilled = prefill.IsPrefilled;
            SourceVersion = prefill.SourceVersion;
        }
        else
        {
            Input.DateTaken = DateTime.UtcNow.Date;
            Input.Version = 1;
            Input.IsActive = true;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        await LoadSelectionsAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (created, errors) = await measurementService.CreateVersionedAsync(Input, cancellationToken);
        ApplyErrors(errors);

        if (!ModelState.IsValid || created is null)
        {
            return Page();
        }

        return RedirectToPage("Details", new { id = created.Id });
    }

    public IActionResult OnPostReload()
    {
        return RedirectToPage(new { customerId = Input.CustomerId, templateType = Input.TemplateType });
    }

    private async Task LoadSelectionsAsync(CancellationToken cancellationToken)
    {
        var customers = await customerService.GetAllAsync(cancellationToken);
        CustomerOptions = customers
            .Select(c => new SelectListItem($"{c.FirstName} {c.LastName}", c.Id.ToString(), c.Id == Input.CustomerId || c.Id == CustomerId))
            .ToList();

        TemplateOptions = MeasurementTemplates.All
            .Select(t => new SelectListItem(t, t, string.Equals(t, Input.TemplateType ?? TemplateType, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    private void ApplyErrors(Dictionary<string, List<string>> errors)
    {
        foreach (var (key, messages) in errors)
        {
            foreach (var message in messages)
            {
                ModelState.AddModelError($"Input.{key}", message);
            }
        }
    }
}
