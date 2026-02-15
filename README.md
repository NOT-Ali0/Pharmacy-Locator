🏥 PharmaLink – Pharmaceutical Supply Management Platform
📌 Overview

PharmaLink is a SaaS-based B2B platform designed to digitize and optimize the pharmaceutical supply chain.

The system connects:

🏥 Pharmacies

🏢 Suppliers

🚗 Sales Representatives

It provides structured order management, visit scheduling, analytics, and performance tracking within a unified platform.

🎯 Problem

Pharmacies and suppliers often rely on phone calls and messaging apps to manage orders and representative visits.
This leads to:

Lack of transparency

No performance tracking

Poor coordination

No data-driven insights

💡 Solution

PharmaLink provides:

Order Management System

Representative Task Assignment

Supplier Performance Score

Sales & Order Analytics Dashboard

Role-Based Access Control

Multi-tenant SaaS-ready architecture

🏗️ Backend Architecture

The backend is built using .NET 8 following Clean Architecture principles to ensure scalability, maintainability, and separation of concerns.

🧱 Architecture Layers

Domain Layer

Entities

Enums

Core Business Logic

Application Layer

DTOs

Commands & Queries (CQRS)

Handlers

Interfaces

Infrastructure Layer

Database Access

Repository Implementations

External Services

API Layer

Controllers

Authentication

Role-based Authorization

⚙️ Technologies Used (Backend)

.NET 8 Web API

Entity Framework Core

Clean Architecture

CQRS Pattern

JWT Authentication

Role-Based Authorization

In-Memory / Simulated Data (for MVP stage)

RESTful API Design

🔐 Security

HTTPS Encryption

Password Hashing

JWT Authentication

Role-based access control

Multi-tenant data isolation
