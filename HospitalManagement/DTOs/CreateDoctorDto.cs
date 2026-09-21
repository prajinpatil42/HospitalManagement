using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.DTOs;

public class CreateDoctorDto
{
    [Required]
    public string DoctorNumber { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Phone number must be a valid 10-digit Indian mobile number.")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}