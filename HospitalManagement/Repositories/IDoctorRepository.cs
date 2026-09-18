using HospitalManagement.Models;

namespace HospitalManagement.Repositories;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task<Doctor> CreateAsync(Doctor doctor);
    Task UpdateAsync(Doctor doctor);
    Task<List<Doctor>> SearchBySpecializationAsync(string specialization);
}