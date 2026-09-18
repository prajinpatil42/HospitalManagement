using HospitalManagement.DTOs;

namespace HospitalManagement.Servicess;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateStatusAsync(int id, string status);

    Task<List<AppointmentDto>> SearchAsync(
    int? doctorId,
    int? patientId,
    DateTime? date,
    string? status);

    Task<AppointmentSummaryDto> GetDailySummaryAsync(DateTime date);


}