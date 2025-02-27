import { createRouter, createWebHistory } from 'vue-router';
import ProjectList from '../components/ProjectList.vue';
import ProjectForm from '../components/ProjectForm.vue';

const routes = [
  {
    path: '/',
    name: 'ProjectList',
    component: ProjectList,
  },
  {
    path: '/projects/new',
    name: 'CreateProject',
    component: ProjectForm,
  },
  {
    path: '/projects/:id',
    name: 'EditProject',
    component: ProjectForm,
    props: true, // Pass route params as props to the component
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;