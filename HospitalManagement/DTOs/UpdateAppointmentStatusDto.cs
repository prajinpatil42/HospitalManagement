using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.DTOs;

public class UpdateAppointmentStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}