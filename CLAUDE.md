# Partify

Collaborative party playlists powered by Spotify. Provides similar functionality as Spotify Jam but extends it with voting, queue management and persistent session history.

## Business Context

- **Hosts**: Create sessions, control playback, manage queue, upgrade guests
- **Guests**: Join via link/QR, add songs, vote on tracks
- **Requirements**: Host needs Spotify Premium, guests need any Spotify account

## Architecture

- **Frontend**: Nuxt 4 application (@frontend/CLAUDE.md)
- **API**: .NET backend with clean architecture (@api/CLAUDE.md)
- **Database**: PostgreSQL with Entity Framework Core (planned)
- **Real-time**: SignalR for live updates (planned)
- **Integration**: Spotify Web API, OAuth authentication
- **Queue Algorithm**: Orders songs by vote count, randomly selects from highest-voted tier when advancing

## Code Guidelines

- Keep to the scope of what the user asks - don't add extra steps without asking
- Always run relevant commands (build, test, lint, format, ...) after making changes
- Ask for clarification instead of making assumptions
- Update CLAUDE.md files when they are out-of-date, no longer represent the state of the project or additional information could be useful for future tasks

## MCP Integration

- Use configured MCP servers, especially Context7 for up-to-date library documentation
- Query Context7 instead of relying on potentially outdated training data

## Commands

- **Frontend**: See @frontend/CLAUDE.md
- **API**: See @api/CLAUDE.md