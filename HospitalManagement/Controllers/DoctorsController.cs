using HospitalManagement.DTOs;
using HospitalManagement.Servicess;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _service;

    public DoctorsController(IDoctorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<DoctorDto>>> GetAll()
    {
        var doctors = await _service.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
    {
        var doctor = await _service.GetByIdAsync(id);

        if (doctor == null)
            return NotFound("Doctor not found.");

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create(CreateDoctorDto dto)
    {
        var doctor = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = doctor.Id },
            doctor
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateDoctorDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound("Doctor not found.");

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<DoctorDto>>> SearchBySpecialization(
        string specialization)
    {
        var doctors = await _service
            .SearchBySpecializationAsync(specialization);

        return Ok(doctors);
    }
}