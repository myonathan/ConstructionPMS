import { createStore } from 'vuex';
import axios from 'axios';

export interface Project {
  projectId: number;
  projectName: string;
  projectLocation: string;
  projectStage: string;
  projectCategory: string;
  otherCategory?: string;
  constructionStartDate: string;
  projectDetails: string;
  projectCreatorId: string;
}

interface User {
  id: string;
  email: string;
}

const store = createStore({
  state: {
    projects: [] as Project[],
    user: null as User | null, // Add user state
  },
  mutations: {
    setProjects(state, projects: Project[]) {
      state.projects = projects;
    },
    addProject(state, project: Project) {
      state.projects.push(project);
    },
    updateProject(state, updatedProject: Project) {
      const index = state.projects.findIndex(p => p.projectId === updatedProject.projectId);
      if (index !== -1) {
        state.projects[index] = updatedProject;
      }
    },
    deleteProject(state, projectId: number) {
      state.projects = state.projects.filter(p => p.projectId !== projectId);
    },
    setUser (state, user: User | null) { // Mutation to set user
      state.user = user;
    },
  },
  actions: {
    async fetchProjects({ commit }) {
      const response = await axios.get('/api/projects');
      commit('setProjects', response.data);
    },
    async createProject({ commit }, project: Project) {
      const response = await axios.post('/api/projects', project);
      commit('addProject', response.data);
    },
    async updateProject({ commit }, project: Project) {
      await axios.put(`/api/projects/${project.projectId}`, project);
      commit('updateProject', project);
    },
    async deleteProject({ commit }, projectId: number) {
      await axios.delete(`/api/projects/${projectId}`);
      commit('deleteProject', projectId);
    },
    async login({ commit }, { email, password }) {
      try {
        const response = await axios.post('/api/login', { email, password });
        const user = response.data; // Assuming the response contains user data
        commit('setUser ', user);
      } catch (error) {
        console.error('Login failed:', error);
        throw error; // Rethrow the error to handle it in the component
      }
    },
    async logout({ commit }) {
      await axios.post('/api/logout'); // Call your logout API
      commit('setUser ', null); // Clear user state
    },
  },
});

export default store;