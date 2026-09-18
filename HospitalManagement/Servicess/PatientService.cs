using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;

namespace HospitalManagement.Servicess;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PatientDto>> GetAllAsync()
    {
        var patients = await _repository.GetAllAsync();

        return patients.Select(p => new PatientDto
        {
            Id = p.Id,
            PatientNumber = p.PatientNumber,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Phone = p.Phone,
            Email = p.Email
        }).ToList();
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _repository.GetByIdAsync(id);

        if (patient == null)
            return null;

        return new PatientDto
        {
            Id = patient.Id,
            PatientNumber = patient.PatientNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Phone = patient.Phone,
            Email = patient.Email
        };
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        var patient = new Patient
        {
            PatientNumber = dto.PatientNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email
        };

        var createdPatient = await _repository.CreateAsync(patient);

        return new PatientDto
        {
            Id = createdPatient.Id,
            PatientNumber = createdPatient.PatientNumber,
            FirstName = createdPatient.FirstName,
            LastName = createdPatient.LastName,
            DateOfBirth = createdPatient.DateOfBirth,
            Gender = createdPatient.Gender,
            Phone = createdPatient.Phone,
            Email = createdPatient.Email
        };
    }

    public async Task<bool> UpdateAsync(int id, CreatePatientDto dto)
    {
        var patient = await _repository.GetByIdAsync(id);

        if (patient == null)
            return false;

        patient.PatientNumber = dto.PatientNumber;
        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc); ;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;

        await _repository.UpdateAsync(patient);

        return true;
    }

    public async Task<List<PatientDto>> SearchAsync(string search)
    {
        var patients = await _repository.SearchAsync(search);

        return patients.Select(p => new PatientDto
        {
            Id = p.Id,
            PatientNumber = p.PatientNumber,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Phone = p.Phone,
            Email = p.Email
        }).ToList();
    }
}