# Storing Authentication Tokens

## Overview

Partify uses a clean separation between user authentication and Spotify API token management:

1. **Authentication**: OAuth flow → user signs in → only user ID/claims stored in cookie
2. **Token Storage**: Separate `UserTokens` table with encrypted tokens in database
3. **Token Service**: Dedicated service handles all token operations (refresh, validation, storage)
4. **Client Factory**: Simple factory creates Spotify clients using the token service

## Architecture Benefits

- Clean separation of concerns
- Tokens encrypted at rest in database
- No tokens transmitted in cookies after initial auth
- Centralized token revocation and audit capabilities
- Better for horizontal scaling
- No cookie size constraints

## Security Considerations

**Access Tokens**: Short-lived (~1 hour), limited scope, designed by OAuth 2.0 to be stored
**Refresh Tokens**: Long-lived, more sensitive - always encrypt these

Database storage with encryption is more secure than cookie storage (no XSS/CSRF exposure).

## Implementation

### Database Schema
```csharp
public class UserToken
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }  // Encrypted
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Encryption Setup
- Use ASP.NET Core Data Protection API
- Register: `services.AddDataProtection()`
- Apply via EF value converters for transparent encryption/decryption

### Service Interfaces
```csharp
public interface ITokenService
{
    Task<string?> GetValidAccessTokenAsync(string userId);
    Task StoreTokensAsync(string userId, TokenResponse tokens);
    Task RevokeTokensAsync(string userId);
}

public interface ISpotifyClientFactory
{
    Task<SpotifyClient?> CreateClientAsync();
}
```

## Key Implementation Areas

- EF Core entity configuration with encrypted columns
- Token service with automatic refresh logic
- Integration with existing OAuth authentication flow
- Client factory that abstracts token management from controllers