using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasDoctorOverlapAsync(
        int doctorId,
        DateTime appointmentDate,
        TimeSpan startTime,
        TimeSpan endTime)
    {
        return await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate.Date == appointmentDate.Date &&
            a.Status == "Scheduled" &&
            startTime < a.EndTime &&
            endTime > a.StartTime);
    }

    public async Task<bool> HasPatientOverlapAsync(
        int patientId,
        DateTime appointmentDate,
        TimeSpan startTime,
        TimeSpan endTime)
    {
        return await _context.Appointments.AnyAsync(a =>
            a.PatientId == patientId &&
            a.AppointmentDate.Date == appointmentDate.Date &&
            a.Status == "Scheduled" &&
            startTime < a.EndTime &&
            endTime > a.StartTime);
    }

    public async Task<List<Appointment>> SearchAsync(
    int? doctorId,
    int? patientId,
    DateTime? date,
    string? status)
    {
        var query = _context.Appointments
            .AsNoTracking()
            .AsQueryable();

        if (doctorId.HasValue)
        {
            query = query.Where(a => a.DoctorId == doctorId.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(a => a.PatientId == patientId.Value);
        }

        if (date.HasValue)
        {
            var searchDate = DateTime.SpecifyKind(
                date.Value.Date,
                DateTimeKind.Utc);

            var nextDate = searchDate.AddDays(1);

            query = query.Where(a =>
                a.AppointmentDate >= searchDate &&
                a.AppointmentDate < nextDate);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        return await query.ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetAppointmentCountBySpecializationAsync(
    DateTime date)
    {
        var searchDate = DateTime.SpecifyKind(
            date.Date,
            DateTimeKind.Utc);

        var nextDate = searchDate.AddDays(1);

        return await _context.Appointments
            .AsNoTracking()
            .Where(a =>
                a.AppointmentDate >= searchDate &&
                a.AppointmentDate < nextDate)
            .Join(
                _context.Doctors,
                appointment => appointment.DoctorId,
                doctor => doctor.Id,
                (appointment, doctor) => doctor.Specialization)
            .GroupBy(specialization => specialization)
            .Select(group => new
            {
                Specialization = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(
                x => x.Specialization,
                x => x.Count);
    }
}