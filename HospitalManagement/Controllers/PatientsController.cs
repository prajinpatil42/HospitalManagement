using HospitalManagement.DTOs;
using HospitalManagement.Servicess;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    // GET: api/Patients
    [HttpGet]
    public async Task<ActionResult<List<PatientDto>>> GetAll()
    {
        var patients = await _service.GetAllAsync();
        return Ok(patients);
    }

    // GET: api/Patients/1
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await _service.GetByIdAsync(id);

        if (patient == null)
            return NotFound("Patient not found.");

        return Ok(patient);
    }

    // POST: api/Patients
    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(CreatePatientDto dto)
    {
        var patient = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = patient.Id },
            patient
        );
    }

    // PUT: api/Patients/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePatientDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound("Patient not found.");

        return NoContent();
    }

    // GET: api/Patients/search?search=Rahul
    [HttpGet("search")]
    public async Task<ActionResult<List<PatientDto>>> Search(string search)
    {
        var patients = await _service.SearchAsync(search);

        return Ok(patients);
    }
}