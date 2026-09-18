using HospitalManagement.Models;

namespace HospitalManagement.Repositories;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient> CreateAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task<List<Patient>> SearchAsync(string search);
}