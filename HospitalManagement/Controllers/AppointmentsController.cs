using HospitalManagement.DTOs;
using HospitalManagement.Servicess;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;

    public AppointmentsController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppointmentDto>>> GetAll()
    {
        var appointments = await _service.GetAllAsync();
        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        var appointment = await _service.GetByIdAsync(id);

        if (appointment == null)
            return NotFound("Appointment not found.");

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create(
        CreateAppointmentDto dto)
    {
        try
        {
            var appointment = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = appointment.Id },
                appointment
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateAppointmentStatusDto dto)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(
                id,
                dto.Status);

            if (!updated)
                return NotFound("Appointment not found.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(
                id,
                "Cancelled");

            if (!updated)
                return NotFound("Appointment not found.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(
                id,
                "Completed");

            if (!updated)
                return NotFound("Appointment not found.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/noshow")]
    public async Task<IActionResult> NoShow(int id)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(
                id,
                "NoShow");

            if (!updated)
                return NotFound("Appointment not found.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<AppointmentDto>>> Search(
    int? doctorId,
    int? patientId,
    DateTime? date,
    string? status)
    {
        var appointments = await _service.SearchAsync(
            doctorId,
            patientId,
            date,
            status);

        return Ok(appointments);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<AppointmentSummaryDto>> GetDailySummary(
    DateTime date)
    {
        var summary = await _service.GetDailySummaryAsync(date);

        return Ok(summary);
    }
}