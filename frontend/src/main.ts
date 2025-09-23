//#region styling

import './styles/reset.css';
import 'primeicons/primeicons.css';
import './styles/main.scss';

//#endregion

import { createApp } from 'vue';
import App from './App.vue';
const app = createApp(App);

//#region router

import router from './router';
app.use(router);

//#endregion

//#region PrimeVue

import PrimeVue from 'primevue/config';
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
app.use(PrimeVue, {
  theme: {
    preset: primePreset,
    options: {
      darkModeSelector: 'none',
    },
  },
});

//#endregion

//#region Tanstack Query

import { VueQueryPlugin } from '@tanstack/vue-query';
app.use(VueQueryPlugin);

//#endregion

app.mount('#app');
