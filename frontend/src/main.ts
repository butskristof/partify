import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

//#region styling

import './styles/reset.css';
import './styles/main.scss';

//#endregion

const app = createApp(App);

app.use(router);

app.mount('#app');
