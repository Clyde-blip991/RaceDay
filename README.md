RaceDay - Event Management System

 Overview

RaceDay is a full-stack web-based event management system designed for South African road running, walking, and cycling events. It allows Event Organisers to create and manage events with multiple categories, and Participants to discover events, enrol in categories, and track their performance results.

System Architecture

 User Roles

 Organiser
- Create, edit, and delete events
- Manage event categories and entry fees
- Capture participant results (finish times, positions)
- View all event enrolments
- Monitor event details and categories

 Participant
- Register an account
- Browse available events filtered by type (Running, Walking, Cycling)
- Enrol in event categories
- View personal enrolments and race results
- Track performance history across events

 Database Schema

The system uses 6 core entities:
- Users: Store both Organisers and Participants with role-based access
- Venues: Reusable event locations across South Africa
- Events: Created by Organisers, hosted at Venues
- Categories: Distance/fee variants within each Event
- Enrolments: Links Participants to Categories (many-to-many)
- Results: Performance records captured by Organisers

See `/docs/raceday_erd.png` for the full Entity Relationship Diagram.

API Specification

The API provides endpoints for:
- Authentication (register, login)
- User profile management
- Event CRUD operations
- Category management within events
- Enrolment handling
- Result capture and retrieval

See `/docs/endpoint_plan.docx` for the complete endpoint specification.

 Setup & Installation

 Requirements
- SQL Server Management Studio (SSMS) or SQL Server 2019+
- .NET 6.0 SDK (for Part 2)
- Docker (for Part 3)

Database Setup
1. Open SSMS
2. Execute the script in `/docs/raceday_database.sql`
3. Verify tables are created with sample data

CI/CD Pipeline

This repository uses GitHub Actions to validate the `/docs` folder on every push. The workflow checks that all required planning documents are present:
- `raceday_erd.png`
- `raceday_database.sql`
- `endpoint_plan.docx`

 Video Walkthrough

Watch a detailed walkthrough of the system design and database setup:
[RaceDay Planning & Database Setup](https://youtu.be/# RaceDay - Event Management System

## Overview

RaceDay is a full-stack web-based event management system designed for South African road running, walking, and cycling events. It allows Event Organisers to create and manage events with multiple categories, and Participants to discover events, enrol in categories, and track their performance results.

## System Architecture

The system is built across three parts:
- **Part 1:** System planning with ERD, API endpoint specification, and SQL database schema
- **Part 2:** RESTful API built in C# with unit tests and CI/CD automation
- **Part 3:** MVC web application consuming the API with Docker containerization and Azure Blob Storage integration

## User Roles

### Organiser
- Create, edit, and delete events
- Manage event categories and entry fees
- Capture participant results (finish times, positions)
- View all event enrolments
- Monitor event details and categories

### Participant
- Register an account
- Browse available events filtered by type (Running, Walking, Cycling)
- Enrol in event categories
- View personal enrolments and race results
- Track performance history across events

## Database Schema

The system uses 6 core entities:
- **Users:** Store both Organisers and Participants with role-based access
- **Venues:** Reusable event locations across South Africa
- **Events:** Created by Organisers, hosted at Venues
- **Categories:** Distance/fee variants within each Event
- **Enrolments:** Links Participants to Categories (many-to-many)
- **Results:** Performance records captured by Organisers

See `/docs/raceday_erd.png` for the full Entity Relationship Diagram.

## API Specification

The API provides endpoints for:
- Authentication (register, login)
- User profile management
- Event CRUD operations
- Category management within events
- Enrolment handling
- Result capture and retrieval

See `/docs/endpoint_plan.docx` for the complete endpoint specification.

 Setup & Installation

 Requirements
- SQL Server Management Studio (SSMS) or SQL Server 2019+
- .NET 6.0 SDK (for Part 2)
- Docker (for Part 3)

 Database Setup
1. Open SSMS
2. Execute the script in `/docs/raceday_database.sql`
3. Verify tables are created with sample data

 CI/CD Pipeline

This repository uses GitHub Actions to validate the `/docs` folder on every push. The workflow checks that all required planning documents are present:
- `raceday_erd.png`
- `raceday_database.sql`
- `endpoint_plan.docx`

Video Walkthrough

Watch a detailed walkthrough of the system design and database setup:
[RaceDay Planning & Database Setup]https://youtu.be/GsGBORuMWjQ 


## Part 2: RESTful API

The RaceDay API is built with ASP.NET Core Web API (.NET 8), Entity Framework Core and SQL Server. Swagger is included for testing.

### Roles

- **Organiser:** can create events. Only Organisers can create events; Participants receive 403 Forbidden.
- **Participant:** can register and log in. Participant-only functions (such as enrolling in events) are planned for the next iteration.

### Setup and Run Instructions

1. Run `docs/raceday_database.sql` in SQL Server Management Studio to create and seed the `RaceDayDB` database.
2. Open `RaceDay.API/RaceDay.API.sln` in Visual Studio 2022.
3. Update the `DefaultConnection` string in `appsettings.json` to match your SQL Server instance name.
4. Press **F5**. The API runs at `http://localhost:5075`, and Swagger is available at `http://localhost:5075/swagger`.

### Endpoints Implemented

| Method | Route | Role |
|---|---|---|
| POST | /api/auth/register | Public |
| POST | /api/auth/login | Public |
| GET | /api/events | Public |
| GET | /api/events/{id} | Public |
| POST | /api/events | Organiser (via X-User-Id header) |

### Video Walkthrough

Part 2 demonstration: [## Part 2: RESTful API

The RaceDay API is built with ASP.NET Core Web API (.NET 8), Entity Framework Core and SQL Server. Swagger is included for testing.

### Roles

- **Organiser:** can create events. Only Organisers can create events; Participants receive 403 Forbidden.
- **Participant:** can register and log in. Participant-only functions (such as enrolling in events) are planned for the next iteration.

### Setup and Run Instructions

1. Run `docs/raceday_database.sql` in SQL Server Management Studio to create and seed the `RaceDayDB` database.
2. Open `RaceDay.API/RaceDay.API.sln` in Visual Studio 2022.
3. Update the `DefaultConnection` string in `appsettings.json` to match your SQL Server instance name.
4. Press **F5**. The API runs at `http://localhost:5075`, and Swagger is available at `http://localhost:5075/swagger`.

### Endpoints Implemented

| Method | Route | Role |
|---|---|---|
| POST | /api/auth/register | Public |
| POST | /api/auth/login | Public |
| GET | /api/events | Public |
| GET | /api/events/{id} | Public |
| POST | /api/events | Organiser (via X-User-Id header) |

### Video Walkthrough

Part 2 demonstration: [https://youtu.be/9Vrv3bPwFkU]]

 Commits

This project follows best practices with meaningful, atomic commits reflecting real development work.)
