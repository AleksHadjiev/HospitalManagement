CREATE DATABASE HospitalManagement_Db
USE HospitalManagement_Db

-- Едно отделение - много пациенти
-- Един доктор - много пациенти

CREATE TABLE Department (
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	DepartmentName NVARCHAR(128) NOT NULL,
	Capacity INT,
	DepartmentNumber BIGINT,
	HeadNurse BIT DEFAULT 1,
	NursesCount INT
)

CREATE TABLE Doctor (
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	FullName NVARCHAR(64) NOT NULL,
	Age INT,
	PersonalId BIGINT NOT NULL,
	Specialization NVARCHAR(128) NOT NULL,
	ExperienceStartDate DATE NOT NULL
)

CREATE TABLE Patient (
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	DepartmentId UNIQUEIDENTIFIER NOT NULL,
	DoctorId UNIQUEIDENTIFIER NOT NULL,
	FullName NVARCHAR(64) NOT NULL,
	Age INT,
	PersonalId BIGINT NOT NULL,
	EntryDate DATE,
	Insurance BIT DEFAULT 0,
	CONSTRAINT FK_DepartmentPatient FOREIGN KEY(DepartmentId)
	REFERENCES Department(Id),
	CONSTRAINT FK_DoctorPatient FOREIGN KEY(DoctorId)
	REFERENCES Doctor(Id)
)

CREATE TABLE Users (
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	Username NVARCHAR(32) NOT NULL,
	[Password] NVARCHAR(1024) NOT NULL,
	IsAdmin BIT DEFAULT 0
)