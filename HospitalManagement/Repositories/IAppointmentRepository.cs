using HospitalManagement.Models;

namespace HospitalManagement.Repositories;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(int id);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);

    Task<bool> HasDoctorOverlapAsync(
        int doctorId,
        DateTime appointmentDate,
        TimeSpan startTime,
        TimeSpan endTime);

    Task<bool> HasPatientOverlapAsync(
        int patientId,
        DateTime appointmentDate,
        TimeSpan startTime,
        TimeSpan endTime);

    Task<List<Appointment>> SearchAsync(
    int? doctorId,
    int? patientId,
    DateTime? date,
    string? status);

    Task<Dictionary<string, int>> GetAppointmentCountBySpecializationAsync(
    DateTime date);
}