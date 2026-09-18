using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Servicess;
using Moq;

namespace HospitalManagement.Tests;

public class PatientServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenPatientExists_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 1,
            PatientNumber = "P001",
            FirstName = "Prajin",
            LastName = "Patil",
            DateOfBirth = new DateTime(1998, 7, 1),
            Gender = "Male",
            Phone = "9999999999",
            Email = "prajin@example.com"
        };

        var repositoryMock = new Mock<IPatientRepository>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(patient);

        var service = new PatientService(repositoryMock.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("P001", result.PatientNumber);
        Assert.Equal("Prajin", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatientDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repositoryMock = new Mock<IPatientRepository>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Patient?)null);

        var service = new PatientService(repositoryMock.Object);

        // Act
        var result = await service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }
}