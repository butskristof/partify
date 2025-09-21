# Frontend - Nuxt Application

Nuxt 4 application for Partify collaborative playlists.

## Tech Stack

- **Framework**: Nuxt 4
- **Language**: TypeScript 5.9+
- **Styling**: Sass, Vue Scoped CSS
- **Code Quality**: ESLint, Prettier
- **Icons**: @nuxt/icon
- **Fonts**: @nuxt/fonts

## Commands

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run generate` - Generate static site
- `npm run preview` - Preview production build
- `npm run test:ts` - TypeScript type checking
- `npm run lint:check` - Check ESLint rules
- `npm run lint:fix` - Fix ESLint issues
- `npm run format:check` - Check Prettier formatting
- `npm run format:fix` - Apply Prettier formatting

## Code Guidelines

Always follow best practices across the Nuxt + Vue + TypeScript ecosystem—leveraging the official Nuxt and Vue documentation (including guides, configuration references, and architecture patterns)—to ensure code consistency, maintainability, and optimal architectural design.

- **Always run TypeScript type checking, linting and formatting after making changes**
- Use TypeScript with strict mode
- Follow existing Vue 3 Composition API patterns
- Use ES modules (import/export) syntax
- Destructure imports when possible
- Follow Nuxt 4 conventions for pages, components, composables
- Use Vue scoped CSS for styling
- **NEVER use Tailwind CSS** - Use Sass and Vue scoped CSS instead

## Code Style

- 2-space indentation (configured in .prettierrc.json)
- Use single quotes for strings
- Trailing commas where valid
- Vue component naming: PascalCase for files, kebab-case in templates

## Directory Structure (Nuxt 4)

Nuxt 4 uses an `app/` directory as the primary source directory:

```
app/
  assets/          - Static assets (CSS, images, fonts) processed by build tool
  components/      - Vue components
  composables/     - Vue Composition API functions (auto-imported)
  layouts/         - Layout components for page structure
  middleware/      - Route middleware functions
  pages/           - Vue pages for file-based routing
  plugins/         - Nuxt plugins
  utils/           - Utility functions (auto-imported)
  app.vue          - Root Vue component
  app.config.ts    - Application configuration
  error.vue        - Custom error page component
public/            - Static assets served directly (favicons, robots.txt)
server/            - Server-side logic and API routes
  api/             - API route handlers (auto-registered)
  middleware/      - Server middleware
.nuxt/             - Nuxt internal build artifacts (auto-generated)
.output/           - Production build output
nuxt.config.ts     - Main Nuxt configuration
```

## Pending Decisions

Update this section when libraries are chosen:

- **State Management**: [TBD - Pinia, Vuex, or built-in useState]
- **UI Components**: [TBD - Vuetify, PrimeVue, Headless UI, or custom]
- **Authentication**: [TBD - Integration approach with Spotify OAuth]
- **API Client**: [TBD - $fetch, axios, or custom composables]
- **Real-time Updates**: [TBD - SignalR, WebSockets, SSE, or polling]