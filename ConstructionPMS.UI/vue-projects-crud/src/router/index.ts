import { createRouter, createWebHistory } from 'vue-router';
import ProjectList from '../components/ProjectList.vue';
import ProjectForm from '../components/ProjectForm.vue';
import Login from '../views/Login.vue'; // Import the Login component
import store from '../store'; // Import the Vuex store

const routes = [
  {
    path: '/',
    name: 'ProjectList',
    component: ProjectList,
    meta: { requiresAuth: true }, // Protect this route
  },
  {
    path: '/projects/new',
    name: 'CreateProject',
    component: ProjectForm,
    meta: { requiresAuth: true }, // Protect this route
  },
  {
    path: '/projects/:id',
    name: 'EditProject',
    component: ProjectForm,
    props: true, // Pass route params as props to the component
    meta: { requiresAuth: true }, // Protect this route
  },
  {
    path: '/login',
    name: 'Login',
    component: Login, // Add the login route
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Navigation guard to protect routes
router.beforeEach((to, from, next) => {
  const isAuthenticated = !!store.state.user; // Check if user is authenticated
  console.log('Navigating to:', to.path);
  console.log('User  state:', store.state.user);
  console.log('Is authenticated:', isAuthenticated);

  // Check if the route requires authentication
  if (to.meta.requiresAuth && !isAuthenticated) {
    console.log("User  is not authenticated, redirecting to login.");
    next('/login'); // Redirect to login if not authenticated
  } else {
    next(); // Proceed to the route
  }
});

export default router;