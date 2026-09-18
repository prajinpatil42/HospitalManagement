using HospitalManagement.DTOs;

namespace HospitalManagement.Servicess;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto dto);
    Task<bool> UpdateAsync(int id, CreateDoctorDto dto);
    Task<List<DoctorDto>> SearchBySpecializationAsync(string specialization);
}