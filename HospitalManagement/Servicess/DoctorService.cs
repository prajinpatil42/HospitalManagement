using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;

namespace HospitalManagement.Servicess;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DoctorDto>> GetAllAsync()
    {
        var doctors = await _repository.GetAllAsync();

        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            DoctorNumber = d.DoctorNumber,
            Name = d.Name,
            Specialization = d.Specialization,
            Phone = d.Phone,
            Email = d.Email,
            IsActive = d.IsActive
        }).ToList();
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return null;

        return new DoctorDto
        {
            Id = doctor.Id,
            DoctorNumber = doctor.DoctorNumber,
            Name = doctor.Name,
            Specialization = doctor.Specialization,
            Phone = doctor.Phone,
            Email = doctor.Email,
            IsActive = doctor.IsActive
        };
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
    {
        var doctor = new Doctor
        {
            DoctorNumber = dto.DoctorNumber,
            Name = dto.Name,
            Specialization = dto.Specialization,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = dto.IsActive
        };

        var createdDoctor = await _repository.CreateAsync(doctor);

        return new DoctorDto
        {
            Id = createdDoctor.Id,
            DoctorNumber = createdDoctor.DoctorNumber,
            Name = createdDoctor.Name,
            Specialization = createdDoctor.Specialization,
            Phone = createdDoctor.Phone,
            Email = createdDoctor.Email,
            IsActive = createdDoctor.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, CreateDoctorDto dto)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        doctor.DoctorNumber = dto.DoctorNumber;
        doctor.Name = dto.Name;
        doctor.Specialization = dto.Specialization;
        doctor.Phone = dto.Phone;
        doctor.Email = dto.Email;
        doctor.IsActive = dto.IsActive;

        await _repository.UpdateAsync(doctor);

        return true;
    }

    public async Task<List<DoctorDto>> SearchBySpecializationAsync(string specialization)
    {
        var doctors = await _repository
            .SearchBySpecializationAsync(specialization);

        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            DoctorNumber = d.DoctorNumber,
            Name = d.Name,
            Specialization = d.Specialization,
            Phone = d.Phone,
            Email = d.Email,
            IsActive = d.IsActive
        }).ToList();
    }
}