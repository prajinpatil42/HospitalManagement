using HospitalManagement.DTOs;

namespace HospitalManagement.Servicess;

public interface IPatientService
{
    Task<List<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<bool> UpdateAsync(int id, CreatePatientDto dto);
    Task<List<PatientDto>> SearchAsync(string search);
}