namespace HospitalManagement.DTOs;

public class AppointmentSummaryDto
{
    public int TotalAppointments { get; set; }
    public int ScheduledAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CancelledAppointments { get; set; }
    public int NoShowAppointments { get; set; }

    public Dictionary<string, int> AppointmentCountByDoctorSpecialization { get; set; }
        = new();
}