using HospitalManagement.DTOs;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Servicess;
using Moq;

namespace HospitalManagement.Tests;

public class AppointmentServiceTests
{

    // Test 1: Inactive doctor
    // Verifies that an appointment cannot be created for an inactive doctor.
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

    // Test 2: Invalid appointment time
    // Verifies that the appointment is rejected when the start time is after the end time.
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

    // Test 3: Doctor appointment conflict
    // Verifies that an appointment cannot be created when the doctor is already booked.
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

    // Test 4: Patient appointment conflict
    // Verifies that a patient cannot have two overlapping appointments.

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

    // Test 5: Successful appointment creation
    // Verifies that an appointment is created successfully when all data is valid.
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

    // Test 6: Non-existent patient
    // Verifies that an appointment cannot be created when the patient does not exist.
    [Fact]
    public async Task CreateAsync_WhenPatientDoesNotExist_ThrowsException()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        patientRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

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

        Assert.Equal("Patient not found.", exception.Message);
    }

    // Test 7: Non-existent doctor
    // Verifies that an appointment cannot be created when the doctor does not exist.
    [Fact]
    public async Task CreateAsync_WhenDoctorDoesNotExist_ThrowsException()
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
            .ReturnsAsync((Doctor?)null);

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

        Assert.Equal("Doctor not found.", exception.Message);
    }


    // Test 8: Appointment cancellation
    // Verifies that an existing appointment can be changed from Scheduled to Cancelled.

    [Fact]
    public async Task UpdateStatusAsync_WhenAppointmentExists_CancelsAppointment()
    {
        // Arrange
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var patientRepositoryMock = new Mock<IPatientRepository>();
        var doctorRepositoryMock = new Mock<IDoctorRepository>();

        var appointment = new Appointment
        {
            Id = 1,
            PatientId = 1,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 9, 25),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(10, 30, 0),
            Status = "Scheduled",
            Reason = "Regular checkup"
        };

        appointmentRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        var service = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);

        // Act
        var result = await service.UpdateStatusAsync(1, "Cancelled");

        // Assert
        Assert.True(result);
        Assert.Equal("Cancelled", appointment.Status);

        appointmentRepositoryMock.Verify(
            r => r.UpdateAsync(appointment),
            Times.Once);
    }
}