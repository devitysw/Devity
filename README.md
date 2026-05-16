# Devity

`Devity` is a small collection of .NET libraries published from a single repository.

Projects in this repo:

- `Devity.Extensions`: general-purpose C# extension methods and lightweight helpers.
- `Devity.Blazor`: reusable Blazor UI components.
- `Devity.Mailing`: a thin MailKit-based abstraction for template-driven email sending.
- `Devity.Payout`: a wrapper around the Payout API.
- `Devity.BlazorTest`: sample app for trying the Blazor components locally.
- `Devity.Tests`: NUnit and bUnit test suite.

## Packages

Install the packages you need from NuGet:

```powershell
Install-Package Devity.Extensions
Install-Package Devity.Blazor
Install-Package Devity.Mailing
Install-Package Devity.Payout
```

Package-specific documentation:

- [`Devity.Extensions/README.md`](./Devity.Extensions/README.md)
- [`Devity.Blazor/README.md`](./Devity.Blazor/README.md)
- [`Devity.Mailing/README.md`](./Devity.Mailing/README.md)
- [`Devity.Payout/README.md`](./Devity.Payout/README.md)

## Requirements

- `Devity.Extensions`: `net9.0`
- `Devity.Blazor`: `net9.0`
- `Devity.Mailing`: `net9.0`
- `Devity.Payout`: `net7.0`

## Local Development

Restore and build the solution:

```bash
dotnet restore
dotnet build Devity.sln
```

Run the test suite:

```bash
dotnet test Devity.sln
```

Run the Blazor sample app:

```bash
dotnet run --project Devity.BlazorTest
```

If you change `Devity.Blazor` styles, rebuild the generated stylesheet:

```bash
npm install --prefix Devity.Blazor
npm run build:css --prefix Devity.Blazor
```

## Notes

- Packages are built and published individually from GitHub Actions.
- Version numbers are generated from the current date and time during build.
