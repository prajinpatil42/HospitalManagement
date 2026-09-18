using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Servicess;
using Moq;

namespace HospitalManagement.Tests;

public class AppointmentServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenDoctorIsInactive_ThrowsException()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient
            {
                Id = 1,
                PatientNumber = "P001"
            });

        doctorRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor
            {
                Id = 1,
                DoctorNumber = "D001",
                Name = "Dr. Test",
                IsActive = false
            });

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(10, 30, 0),
            Reason = "Test"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => service.CreateAsync(dto));

        Assert.Equal("Doctor is not active.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenStartTimeIsAfterEndTime_ThrowsException()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient
            {
                Id = 1,
                PatientNumber = "P001"
            });

        doctorRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor
            {
                Id = 1,
                DoctorNumber = "D001",
                Name = "Dr. Test",
                IsActive = true
            });

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(11, 0, 0),
            EndTime = new TimeSpan(10, 30, 0),
            Reason = "Test"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => service.CreateAsync(dto));

        Assert.Equal(
            "Start time must be before end time.",
            exception.Message);
    }


    [Fact]
    public async Task CreateAsync_WhenDoctorHasOverlap_ThrowsException()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient
            {
                Id = 1,
                PatientNumber = "P001"
            });

        doctorRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor
            {
                Id = 1,
                DoctorNumber = "D001",
                Name = "Dr. Test",
                IsActive = true
            });

        appointmentRepositoryMock
            .Setup(r => r.HasDoctorOverlapAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(10, 15, 0),
            EndTime = new TimeSpan(10, 45, 0),
            Reason = "Test overlap"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => service.CreateAsync(dto));

        Assert.Equal(
            "Doctor already has an appointment at this time.",
            exception.Message);
    }


    [Fact]
    public async Task CreateAsync_WhenPatientHasOverlap_ThrowsException()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient
            {
                Id = 1,
                PatientNumber = "P001"
            });

        doctorRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor
            {
                Id = 1,
                DoctorNumber = "D001",
                Name = "Dr. Test",
                IsActive = true
            });

        appointmentRepositoryMock
            .Setup(r => r.HasDoctorOverlapAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(r => r.HasPatientOverlapAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(10, 15, 0),
            EndTime = new TimeSpan(10, 45, 0),
            Reason = "Test patient overlap"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => service.CreateAsync(dto));

        Assert.Equal(
            "Patient already has an appointment at this time.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid_CreatesAppointment()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient
            {
                Id = 1,
                PatientNumber = "P001"
            });

        doctorRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor
            {
                Id = 1,
                DoctorNumber = "D001",
                Name = "Dr. Test",
                IsActive = true
            });

        appointmentRepositoryMock
            .Setup(r => r.HasDoctorOverlapAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(r => r.HasPatientOverlapAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Appointment>()))
            .ReturnsAsync((Appointment appointment) =>
            {
                appointment.Id = 10;
                return appointment;
            });

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(10, 30, 0),
            Reason = "Regular checkup"
        };

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal(1, result.PatientId);
        Assert.Equal(1, result.DoctorId);
        Assert.Equal("Scheduled", result.Status);
        Assert.Equal("Regular checkup", result.Reason);
    }
}