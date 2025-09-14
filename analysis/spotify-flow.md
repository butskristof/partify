# Spotify API Integration Flow for Partify

## Overview

This document outlines the technical implementation for integrating Spotify's Web API with Partify's collaborative playlist system. The approach uses a hybrid queue management strategy that combines Spotify's native queue with manual playback control.

## Core Strategy: Hybrid Queue + Manual Control

We maintain control over track selection while leveraging Spotify's built-in queue for seamless playback transitions. The key insight is to only queue one track ahead at any time, maintaining full control over the selection algorithm.

## API Flow Sequences

### 1. Session Initialization

```yaml
Authentication & Setup:
  1. GET /me
     - Verify user has Spotify Premium (required for playback control)
     - Store user profile information

  2. GET /me/player/devices
     - Retrieve list of available playback devices
     - Present device selection to host

  3. PUT /me/player
     Body: {
       "device_ids": ["selected_device_id"],
       "play": false
     }
     - Transfer playback to selected device
     - Keep paused initially until host starts session
```

### 2. Starting Playback

```yaml
Initial Track:
  1. Select first track from queue
     - Apply vote-based selection algorithm
     - Mark track as "now playing" in session

  2. PUT /me/player/play
     Body: {
       "uris": ["spotify:track:{trackId}"],
       "position_ms": 0
     }
     - Start playing selected track
     - Begin monitoring loop
```

### 3. Continuous Monitoring Loop

```yaml
Monitoring (every 1-2 seconds):
  1. GET /me/player/currently-playing
     Response includes:
     - item.id: Current track ID
     - progress_ms: Current position in track
     - duration_ms: Total track duration
     - is_playing: Playback state

  2. Calculate remaining time:
     remaining_ms = duration_ms - progress_ms

  3. Trigger next track preparation:
     if remaining_ms < 5000ms:
       - Select next track from queue
       - Prepare for queueing

  4. Queue next track:
     if remaining_ms < 2000ms AND not_already_queued:
       POST /me/player/queue?uri=spotify:track:{nextTrackId}
       - Add single track to Spotify's queue
       - Set queued flag to prevent duplicate queueing
```

### 4. Track Selection Algorithm

```yaml
When selecting next track:
  1. Query database for all queued tracks

  2. Group tracks by vote count

  3. Select highest vote tier

  4. Randomly pick from that tier

  5. Update database:
     - Mark track as played
     - Add to session history
     - Remove from active queue

  6. Broadcast update via SignalR:
     - New queue state
     - Upcoming track notification
```

### 5. Skip Handling

```yaml
When skip detected:
  1. Detection via monitoring:
     - Track URI changed unexpectedly
     - progress_ms reset to near 0

  2. If host-initiated skip:
     POST /me/player/next
     - Let Spotify advance to queued track

  3. Update internal state:
     - Mark skipped track in history
     - Continue normal monitoring
```

### 6. Pause/Resume Control

```yaml
Pause:
  PUT /me/player/pause
  - Stops playback immediately
  - Maintain queue state

Resume:
  PUT /me/player/play
  - No body = resumes current track
  - Continues from paused position
```

### 7. Queue Management Edge Cases

```yaml
Empty Queue:
  - Don't queue anything to Spotify
  - Let current track finish
  - Playback stops naturally
  - Monitor for new additions

Single Track Remaining:
  - Queue it normally
  - After it plays, handle empty queue

User Removes Queued Track:
  - If already queued to Spotify: Can't remove
  - If not yet queued: Select different track
  - Update selection when time comes
```

## Implementation Considerations

### Timing Parameters

- **Monitoring Interval**: 1-2 seconds (balance responsiveness vs API rate limits)
- **Preparation Threshold**: 5 seconds before track end (allows for selection logic)
- **Queue Threshold**: 2 seconds before track end (ensures smooth transition)
- **Transition Overlap**: ~200ms (prevents gaps between tracks)

### State Management

Track these states in your backend:

```typescript
interface PlaybackState {
  currentTrack: {
    id: string;
    startedAt: Date;
    duration: number;
  };
  nextTrack: {
    id: string;
    selectedAt: Date;
    queuedToSpotify: boolean;
  };
  queue: Track[];
  history: PlayedTrack[];
  isPlaying: boolean;
  currentDevice: string;
}
```

### Error Handling

1. **API Rate Limiting**
   - Implement exponential backoff
   - Cache responses where possible
   - Reduce polling frequency if needed

2. **Network Failures**
   - Retry with backoff strategy
   - Fallback to next track on persistent failures
   - Maintain local state for recovery

3. **Device Disconnection**
   - Detect via 404 responses
   - Prompt host to select new device
   - Attempt automatic reconnection

4. **Token Expiration**
   - Implement token refresh flow
   - Queue requests during refresh
   - Transparent retry after refresh

### Rate Limits

Spotify Web API rate limits:
- No official hard limit published
- Generally safe: ~180 requests/minute
- Implement adaptive throttling based on 429 responses

### Advantages of This Approach

1. **Full Control**: Complete control over track selection algorithm
2. **Seamless Playback**: No gaps between tracks
3. **Responsive**: Can adapt to votes in real-time
4. **Fallback Ready**: Gracefully handles edge cases
5. **Spotify Native**: Uses Spotify's queue for natural behavior

### Potential Limitations

1. Can't remove tracks already queued to Spotify
2. Requires constant monitoring (API calls)
3. ~2 second delay for skip reactions
4. Dependent on network stability

## Alternative: Pure Manual Control

If the hybrid approach proves problematic, fall back to pure manual control:

```yaml
For each track:
  1. Let current track play until last 200ms
  2. PUT /me/player/play with next track
  3. No Spotify queue involvement
  4. Complete control but possible micro-gaps
```

This approach trades seamlessness for absolute control.

## Next Steps

1. Implement monitoring service with SignalR integration
2. Build track selection algorithm with vote weighting
3. Create device management UI for hosts
4. Add comprehensive error handling and recovery
5. Implement analytics for queue behavior patterns