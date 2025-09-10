# Design Prompt for Partify - Collaborative Party Playlist App

**Create a mobile-first responsive web application design for Partify, a collaborative party playlist manager that gives it a distinct identity separate from Spotify.**

## Core Concept
Design a party playlist app where hosts control playback while guests contribute songs and vote. The app should feel festive and social, with its own unique visual identity that doesn't rely on Spotify's branding.

## Visual Direction
- **Style Options to Explore**: Choose between dark/neon party atmosphere (think nightclub vibes), vibrant/playful (colorful and energetic), or elegant/sophisticated (premium feel)
- **NOT Spotify-green**: Create a unique color palette that establishes Partify's own brand
- **Mobile-first**: Primary design for phones, scaling gracefully to tablets and desktops
- **Modern, clean interface with personality** - not just another music app

## Key Screens to Design

### 1. Main Session View (Primary Screen)
**For All Users:**
- **Now Playing Section** (top/prominent):
  - Large album artwork
  - Song title, artist name
  - Progress bar
  - Who added this song (small avatar/name)
  
- **Queue Section** (scrollable list):
  - Compact list items with:
    - Small album artwork (left)
    - Song title & artist (center)
    - Heart-shaped vote button with count
    - Small avatar group of voters (space-constrained)
  - Subtle animation when songs are added/reordered
  
- **Floating Action Button** or prominent search bar for adding songs

**Host-Only Additions:**
- Playback controls (play/pause, skip)
- Queue management (remove songs, change queue mode)
- "Manage Session" options (subtle, maybe in header menu)

### 2. Search & Add Songs
- Full-screen modal or slide-up panel
- Search bar at top
- Results as tappable list items
- Quick "+" button to add to queue
- Recently added feedback animation

### 3. Share Session Modal
- Clean modal with:
  - Large QR code (center)
  - Copyable link below
  - "Share via..." options
  - Session name/host info

### 4. Presenter Mode (Tablet/TV View)
- Split view or adaptive layout:
  - Left/Top: Large now-playing with full artwork
  - Right/Bottom: Queue preview (next 5-10 songs)
  - Party session name
  - Real-time updates with smooth transitions

### 5. Playback History
- Chronological list of played songs
- Who added each song
- Final vote counts
- Timestamp or "played X songs ago"

## Interaction Patterns
- **Voting**: Tap heart to vote (fills/animates), tap again to remove
- **Real-time updates**: Subtle slide-in animations for new songs, gentle reordering
- **Navigation**: Bottom tab bar or single-page with sections
- **Host controls**: Contextual, not overwhelming the interface

## Information Hierarchy
1. What's currently playing (most prominent)
2. Quick actions (add song, vote)
3. Queue visibility
4. Session management (subtle for hosts)


## Design Requirements
- Clear visual hierarchy between hosts and guests (subtle badge/indicator)
- Accessible touch targets
- Dark mode consideration (party environments)
- Loading states and empty states
- Error handling UI
- Show participant count somewhere (not prominent)

## Unique Identity Elements
Create distinctive:
- Custom iconography for voting, queue modes, host indicator
- Unique typography pairing (headings vs body)
- Signature animations/transitions
- Party-themed visual elements without being cheesy

## Additional Considerations
- How to show queue ordering modes (random/votes/insertion)
- Visual feedback for successful actions
- Connection status indicator
- "Queue empty" state encouraging additions
- Session ended/history view treatment

**Generate multiple variations exploring different visual styles, but maintain consistent UX patterns across all versions. Show both mobile and tablet/desktop layouts for the main session view.**


## Responsive Behavior
- **Mobile (375-768px)**: Single column, full-width elements
- **Tablet (768-1024px)**: Consider side-by-side now-playing and queue
- **Desktop (1024px+)**: Utilize space with wider layouts, maybe sidebar navigation
- **Presenter Mode**: Optimized for larger displays (tablets/TVs)