export const routes = {
  home: {
    name: 'home',
    path: '/',
  },
  sessions: {
    name: 'sessions',
    path: 'sessions',
    children: {
      overview: {
        name: 'sessions.overview',
        path: '',
      },
    },
  },
  notFound: {
    name: 'not-found',
    path: '/:pathMatch(.*)*',
  },
} as const;
