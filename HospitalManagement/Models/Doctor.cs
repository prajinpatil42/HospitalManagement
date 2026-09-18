namespace HospitalManagement.Models;

public class Doctor
{
    public int Id { get; set; }

    public string DoctorNumber { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}