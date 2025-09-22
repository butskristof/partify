import { createRouter, createWebHistory } from 'vue-router';
import { routes } from './routes.ts';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      children: [
        {
          name: routes.home.name,
          path: routes.home.path,
          redirect: {
            name: routes.sessions.name,
          },
        },
        {
          name: routes.sessions.name,
          path: routes.sessions.path,
          redirect: {
            name: routes.sessions.children.overview.name,
          },
          children: [
            {
              name: routes.sessions.children.overview.name,
              path: routes.sessions.children.overview.path,
              component: () => import('@/pages/sessions/SessionsOverview.vue'),
            },
          ],
        },
      ],
    },
    {
      name: routes.notFound.name,
      path: routes.notFound.path,
      component: () => import('@/pages/NotFound.vue'),
    },
  ],
});

export default router;
