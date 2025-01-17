import { createApp } from 'vue'
import App from '@/App.vue'
import './assets/css/custom.css';//custom
import { config } from '@/config/app-settings.js';
import router from '@/config/routes/index.js';
const app = createApp(App);
app.config.globalProperties.appConfig = config;
app.use(router);
app.mount('#app');
