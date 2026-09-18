\# Mini Hospital Appointment System



A RESTful Hospital Appointment Management API built using ASP.NET Core Web API, Entity Framework Core, and PostgreSQL.



\## Technologies Used



\- C#

\- ASP.NET Core Web API

\- .NET 10

\- Entity Framework Core

\- PostgreSQL

\- Npgsql

\- Swagger / OpenAPI

\- xUnit

\- Moq

\- Git / GitHub



\## Architecture



The application follows a layered architecture:



\- Controllers

\- DTOs

\- Services

\- Repositories

\- Entity Models

\- Entity Framework Core DbContext

\- PostgreSQL Database



\## Main Features



\### Patient Management



\- Create patient

\- Get all patients

\- Get patient by ID

\- Update patient

\- Search patients



\### Doctor Management



\- Create doctor

\- Get all doctors

\- Get doctor by ID

\- Update doctor

\- Search doctors by specialization



\### Appointment Management



\- Schedule appointment

\- Get all appointments

\- Get appointment by ID

\- Cancel appointment

\- Complete appointment

\- Mark appointment as NoShow

\- Search appointments using multiple filters

\- Daily appointment summary



\## Appointment Business Rules



The system validates:



\- Patient must exist

\- Doctor must exist

\- Doctor must be active

\- Start time must be before end time

\- Doctor cannot have overlapping scheduled appointments

\- Patient cannot have overlapping scheduled appointments

\- Appointment status must be valid



Supported statuses:



\- Scheduled

\- Completed

\- Cancelled

\- NoShow



\## Appointment Search



Appointments can be filtered using:



\- Doctor ID

\- Patient ID

\- Appointment date

\- Status



Multiple filters can also be combined.



Example:



GET `/api/Appointments/search?doctorId=1\&date=2026-09-20\&status=Scheduled`



\## Daily Summary



The API provides a daily appointment summary containing:



\- Total appointments

\- Scheduled appointments

\- Completed appointments

\- Cancelled appointments

\- No-show appointments

\- Appointment count by doctor specialization



Example:



GET `/api/Appointments/summary?date=2026-09-20`



\## API Endpoints



\### Patients



| Method | Endpoint | Description |

|---|---|---|

| GET | `/api/Patients` | Get all patients |

| GET | `/api/Patients/{id}` | Get patient by ID |

| POST | `/api/Patients` | Create patient |

| PUT | `/api/Patients/{id}` | Update patient |

| GET | `/api/Patients/search?search=` | Search patients |



\### Doctors



| Method | Endpoint | Description |

|---|---|---|

| GET | `/api/Doctors` | Get all doctors |

| GET | `/api/Doctors/{id}` | Get doctor by ID |

| POST | `/api/Doctors` | Create doctor |

| PUT | `/api/Doctors/{id}` | Update doctor |

| GET | `/api/Doctors/search?specialization=` | Search by specialization |



\### Appointments



| Method | Endpoint | Description |

|---|---|---|

| GET | `/api/Appointments` | Get all appointments |

| GET | `/api/Appointments/{id}` | Get appointment by ID |

| POST | `/api/Appointments` | Schedule appointment |

| PUT | `/api/Appointments/{id}/status` | Update appointment status |

| PUT | `/api/Appointments/{id}/cancel` | Cancel appointment |

| PUT | `/api/Appointments/{id}/complete` | Complete appointment |

| PUT | `/api/Appointments/{id}/noshow` | Mark as NoShow |

| GET | `/api/Appointments/search` | Search appointments |

| GET | `/api/Appointments/summary?date=` | Get daily summary |



\## Database



The application uses PostgreSQL with Entity Framework Core Code First.



Database:



`HospitalManagementDb`



Migrations included:



\- InitialCreate

\- AddAppointmentIndexes



To apply migrations:



```bash

dotnet ef database update





\## Configuration



The repository does not contain the local database password.



Create `appsettings.Local.json` locally:



```json

{

&#x20; "ConnectionStrings": {

&#x20;   "DefaultConnection": "Host=localhost;Port=5432;Database=HospitalManagementDb;Username=postgres;Password=YOUR\_PASSWORD"

&#x20; }

}











