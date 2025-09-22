import { createRouter, createWebHistory } from 'vue-router';
import SessionsOverview from '@/pages/sessions/SessionsOverview.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/sessions',
    },
    {
      path: '/sessions',
      component: SessionsOverview,
    },
  ],
});

export default router;
