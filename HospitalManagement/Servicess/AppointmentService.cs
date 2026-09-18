using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;

namespace HospitalManagement.Servicess;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();

        return appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            DoctorId = a.DoctorId,
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Reason = a.Reason,
            CreatedDate = a.CreatedDate
        }).ToList();
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
            return null;

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Status = appointment.Status,
            Reason = appointment.Reason,
            CreatedDate = appointment.CreatedDate
        };
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        // Check patient
        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

        if (patient == null)
            throw new Exception("Patient not found.");

        // Check doctor
        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

        if (doctor == null)
            throw new Exception("Doctor not found.");

        // Check doctor active
        if (!doctor.IsActive)
            throw new Exception("Doctor is not active.");

        // Check time
        if (dto.StartTime >= dto.EndTime)
            throw new Exception("Start time must be before end time.");

        var appointmentDate =
            DateTime.SpecifyKind(dto.AppointmentDate, DateTimeKind.Utc);

        // Check doctor overlap
        var doctorOverlap =
            await _appointmentRepository.HasDoctorOverlapAsync(
                dto.DoctorId,
                appointmentDate,
                dto.StartTime,
                dto.EndTime);

        if (doctorOverlap)
            throw new Exception("Doctor already has an appointment at this time.");

        // Check patient overlap
        var patientOverlap =
            await _appointmentRepository.HasPatientOverlapAsync(
                dto.PatientId,
                appointmentDate,
                dto.StartTime,
                dto.EndTime);

        if (patientOverlap)
            throw new Exception("Patient already has an appointment at this time.");

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = appointmentDate,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = "Scheduled",
            Reason = dto.Reason,
            CreatedDate = DateTime.UtcNow
        };

        var createdAppointment =
            await _appointmentRepository.CreateAsync(appointment);

        return new AppointmentDto
        {
            Id = createdAppointment.Id,
            PatientId = createdAppointment.PatientId,
            DoctorId = createdAppointment.DoctorId,
            AppointmentDate = createdAppointment.AppointmentDate,
            StartTime = createdAppointment.StartTime,
            EndTime = createdAppointment.EndTime,
            Status = createdAppointment.Status,
            Reason = createdAppointment.Reason,
            CreatedDate = createdAppointment.CreatedDate
        };
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
            return false;

        var validStatuses = new[]
        {
            "Scheduled",
            "Completed",
            "Cancelled",
            "NoShow"
        };

        if (!validStatuses.Contains(status))
            throw new Exception(
                "Invalid status. Use Scheduled, Completed, Cancelled, or NoShow.");

        appointment.Status = status;

        await _appointmentRepository.UpdateAsync(appointment);

        return true;
    }

    public async Task<List<AppointmentDto>> SearchAsync(
    int? doctorId,
    int? patientId,
    DateTime? date,
    string? status)
    {
        var appointments = await _appointmentRepository.SearchAsync(
            doctorId,
            patientId,
            date,
            status);

        return appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            DoctorId = a.DoctorId,
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Reason = a.Reason,
            CreatedDate = a.CreatedDate
        }).ToList();
    }

    public async Task<AppointmentSummaryDto> GetDailySummaryAsync(
    DateTime date)
    {
        var appointments = await _appointmentRepository.SearchAsync(
            null,
            null,
            date,
            null);

        var specializationCounts =
            await _appointmentRepository
                .GetAppointmentCountBySpecializationAsync(date);

        return new AppointmentSummaryDto
        {
            TotalAppointments = appointments.Count,

            ScheduledAppointments = appointments.Count(a =>
                a.Status == "Scheduled"),

            CompletedAppointments = appointments.Count(a =>
                a.Status == "Completed"),

            CancelledAppointments = appointments.Count(a =>
                a.Status == "Cancelled"),

            NoShowAppointments = appointments.Count(a =>
                a.Status == "NoShow"),

            AppointmentCountByDoctorSpecialization =
                specializationCounts
        };
    }

}