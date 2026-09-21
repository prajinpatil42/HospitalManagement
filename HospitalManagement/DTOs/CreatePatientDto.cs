using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.DTOs;

public class CreatePatientDto
{
    [Required]
    public string PatientNumber { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Phone number must be a valid 10-digit Indian mobile number.")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}