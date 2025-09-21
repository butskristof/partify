# API - .NET Backend

.NET backend for Partify implemented as a ASP.NET Web API, using .NET Aspire for development environment setup. 

## Tech Stack

- **Framework**: .NET 10
- **Language**: C# 14
- **Architecture**: Clean Architecture, CQRS with Mediator pattern
- **Database**: PostgreSQL with Entity Framework Core (planned)
- **Real-time**: SignalR for live session updates (planned)
- **Development**: .NET Aspire for local development environment
- **Formatting**: CSharpier for code formatting
- **Project Structure**: Multi-project solution with separated responsibilities

## Commands

**Note**: API structure not yet created. These will be available once solution is set up:

- `dotnet build` - Build the solution
- `dotnet test` - Run all tests
- `dotnet run` - Run the API (or specific project)
- `dotnet format` - Format code with CSharpier
- `dotnet restore` - Restore NuGet packages

## Development Workflow

- **TDD Required**: Integration test → Unit test → Implementation
- Follow Test-Driven Development for all feature implementation
- Write integration tests first to define behavior
- Write unit tests for business logic
- Implement code to make tests pass
- Use the dotnet CLI to manage NuGet dependencies, do **not** update csproj files or Directory.Pacakges.props directly

## Architecture Guidelines (Planned)

- **Clean Architecture**: Separate concerns into layers (Domain, Application, Infrastructure, Presentation)
- **CQRS**: Separate Command and Query responsibilities with Mediator pattern
- **Multi-project Structure**: Keep responsibilities separated across projects
- **Internal Classes**: Seal internal classes by default
- **Central Package Management**: Use Directory.Packages.props for version management

## Code Guidelines

Always adhere to best practices across the .NET ecosystem—including C#, ASP.NET Core, and EF Core—by consulting the official Microsoft documentation, including language references, API guides, and architectural patterns.

- Use nullable reference types
- Prefer explicit types over var when clarity improves
- Use async/await for I/O operations
- Follow C# naming conventions (PascalCase for public members, camelCase for private)
- Document public APIs with XML comments

## Pending Implementation

- [ ] Create solution structure with projects
- [ ] Set up .NET Aspire app host project
- [ ] Configure integration test framework
- [ ] Set up unit test patterns and helpers
- [ ] Define domain models and business rules
- [ ] Implement Spotify API integration
- [ ] Set up OAuth authentication flow
- [ ] Configure database and repositories
- [ ] Implement real-time communication for queue updates

## Key Implementation Areas

- Solution structure with Clean Architecture projects
- .NET Aspire app host for development
- PostgreSQL database with EF Core migrations
- SignalR hubs for real-time queue and voting updates
- Spotify Web API integration with rate limiting
- OAuth 2.0 authentication flow
- CQRS pattern with MediatR for business logic

## Testing Strategy (Details TBD)

More detailed guidance will be provided for:
- Integration test structure and patterns
- Unit test organization and mocking
- Test data setup and cleanup
- API endpoint testing approaches