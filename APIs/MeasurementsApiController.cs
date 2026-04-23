using ftpms.Interfaces;
using ftpms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ftpms.APIs;

[ApiController]
[Route("api/measurements")]
public class MeasurementsApiController(IMeasurementService measurementService) : ControllerBase
{
    [HttpGet("customer/{customerId:int}")]
    public async Task<IActionResult> GetByCustomer(int customerId, CancellationToken cancellationToken)
    {
        var measurements = await measurementService.GetAllAsync(customerId: customerId, cancellationToken: cancellationToken);
        return Ok(measurements);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var measurement = await measurementService.GetByIdAsync(id, cancellationToken);
        return measurement is null ? NotFound() : Ok(measurement);
    }

    [HttpGet("customer/{customerId:int}/latest")]
    public async Task<IActionResult> GetLatest(int customerId, [FromQuery] string templateType, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(templateType))
        {
            return BadRequest("templateType is required.");
        }

        var measurement = await measurementService.GetLatestAsync(customerId, templateType, cancellationToken);
        return measurement is null ? NotFound() : Ok(measurement);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MeasurementInputViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var (created, errors) = await measurementService.CreateVersionedAsync(model, cancellationToken);
        if (errors.Count > 0)
        {
            foreach (var (key, messages) in errors)
            {
                foreach (var message in messages)
                {
                    ModelState.AddModelError(key, message);
                }
            }

            return ValidationProblem(ModelState);
        }

        return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MeasurementInputViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var (updated, errors) = await measurementService.UpdateAsync(id, model, cancellationToken);
        if (errors.Count > 0)
        {
            foreach (var (key, messages) in errors)
            {
                foreach (var message in messages)
                {
                    ModelState.AddModelError(key, message);
                }
            }

            return ValidationProblem(ModelState);
        }

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await measurementService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
