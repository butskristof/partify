import { definePreset } from '@primeuix/themes';
import Lara from '@primeuix/themes/lara';

const primePreset = definePreset(Lara, {
  semantic: {
    primary: {
      '50': '#faf5fe',
      '100': '#e5cefa',
      '200': '#d1a7f6',
      '300': '#bc81f2',
      '400': '#a85aee',
      '500': '#9333ea',
      '600': '#7d2bc7',
      '700': '#6724a4',
      '800': '#511c81',
      '900': '#3b145e',
      '950': '#250d3b',
    },
    colorScheme: {
      light: {
        surface: {
          '0': '#ffffff',
          '50': '#f3f3f4',
          '100': '#c6c8cb',
          '200': '#999ca2',
          '300': '#6b7079',
          '400': '#3e4450',
          '500': '#111827',
          '600': '#0e1421',
          '700': '#0c111b',
          '800': '#090d15',
          '900': '#070a10',
          '950': '#04060a',
        },
      },
      dark: {
        surface: {
          '0': '#ffffff',
          '50': '#f3f3f4',
          '100': '#c6c8cb',
          '200': '#999ca2',
          '300': '#6b7079',
          '400': '#3e4450',
          '500': '#111827',
          '600': '#0e1421',
          '700': '#0c111b',
          '800': '#090d15',
          '900': '#070a10',
          '950': '#04060a',
        },
      },
    },
  },
});

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/fonts', '@nuxt/icon', '@primevue/nuxt-module', 'nuxt-security'],
  css: ['~/assets/styles/reset.css', 'primeicons/primeicons.css', '~/assets/styles/main.scss'],
  components: {
    // disable auto-import of components
    dirs: [],
  },
  fonts: {
    defaults: {
      // include all weights by default (it's hard to spot specific ones missing)
      weights: [200, 300, 400, 500, 600, 700, 800, 900],
    },
  },
  primevue: {
    options: {
      theme: {
        preset: primePreset,
        options: {
          darkModeSelector: 'none',
        },
      },
    },
  },
  routeRules: {
    // route to the groups overview by default
    '/': { redirect: '/sessions/13FB62D1-B255-4BE8-932D-528D3DFF49DC' },
  },
});
