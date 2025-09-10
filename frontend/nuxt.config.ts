// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@nuxt/eslint', '@nuxt/fonts', '@nuxt/icon'],
  css: ['~/assets/styles/reset.css', '~/assets/styles/main.scss'],
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
  security: {},
});
