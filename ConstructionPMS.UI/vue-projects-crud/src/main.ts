// src/main.ts
import { createApp } from 'vue';
import App from './App.vue';
import router from './router'; // Import the router
import store from './store'; // Import Vuex store
import './index.css'; // Import your Tailwind CSS

const app = createApp(App);

app.use(router); // Use the router
app.use(store); // Use the Vuex store

app.mount('#app');