# Clean Architecture Solution Template

[![Build](https://github.com/Hyperspan/Hyperspan.Base.Api/actions/workflows/build-test.yml/badge.svg)](https://github.com/Hyperspan/Hyperspan.Base.Api/actions/workflows/build-test.yml)
[![Nuget](https://img.shields.io/nuget/v/Hyperspan.Api?label=NuGet)](https://www.nuget.org/packages/Hyperspan.Api)
[![Nuget](https://img.shields.io/nuget/dt/Hyperspan.Api?label=Downloads)](https://www.nuget.org/packages/Hyperspan.Api)

The goal of this template is to provide a straightforward and efficient approach to enterprise application development, leveraging the power of Clean Architecture and ASP.NET Core. Using this template, you can effortlessly create a Single Page App (SPA) with ASP.NET Core and Angular or React, while adhering to the principles of Clean Architecture. Getting started is easy - simply install the **.NET template** (see below for full details).

If you find this project useful, please give it a star. Thanks! ⭐

## Getting Started

The easiest way to get started is to install the [.NET template](https://www.nuget.org/packages/Hyperspan.Api):

```bash
dotnet new install Hyperspan.Api
```

Once installed, create a new solution using the template. You can choose to use Postgres, MySQL, or create a Web API-only solution. Specify the client framework using the `-D` or `--database` option, and provide the output directory where your project will be created. Here are some examples:

To create a Web API with Postgres(Default), .NET Core and Hyperspan.Api:

```bash
dotnet new Hyperspan.Api --output YourProjectName
```

To create a Web API with MySQL, .NET Core and Hyperspan.Api:

```bash
dotnet new Hyperspan.Api --output YourProjectName -D MySQL
```

Launch the app:

```bash
cd YourProjectName
dotnet run
```

To learn more, run the following command:

```bash
dotnet new Hyperspan.Api --help
```

## Database

The template is configured to use Postgres by default. If you would prefer to use MySQL, create your solution using the following command:

```bash
dotnet new Hyperspan.Api --output YourProjectName -D MySQL
```

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

Running database migrations is easy. Ensure you add the following flags to your command (values assume you are executing from repository root)

For example, to add a new migration using **package manager**:

`add-migration 'SampleMigration'`

## Deploy

The template includes a full CI/CD pipeline. The pipeline is responsible for building, testing, publishing and deploying the solution to a self hosted runner. If you would like to learn more, read the [Self-Hosted runner](https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/about-self-hosted-runners).

## Technologies

- [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
- [Identity Framework](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)
- [PostgresSQL](https://www.postgresql.org/)

## Versions

The main branch is now on V1.0.0 The versions are available:

- [1.0.0](https://github.com/Hyperspan/Hyperspan.Base.Api/tree/v1.0.0)

<!-- ## Learn More -->

## Support

If you are having problems, please let me know by [raising a new issue](https://github.com/Hyperspan/Hyperspan.Base.Api/issues/new).

## License

This project is licensed with the [MIT license](LICENSE.txt).

## Contact

Email Address: info@ramson-developers.com
