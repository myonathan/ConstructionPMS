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
  token: string;
}

// Define the base URL for the API
const BASE_URL = 'https://localhost:7188/api';

const store = createStore({
  state: {
    projects: [] as Project[],
    user: null as User | null,
    error: null as string | null, // Store error messages
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
        state.projects.splice(index, 1, updatedProject);
      } else {
        console.error('Project not found for update:', updatedProject.projectId);
      }
    },
    deleteProject(state, projectId: number) {
      state.projects = state.projects.filter(p => p.projectId !== projectId);
    },
    setUser(state, user: User | null) {
      state.user = user;
    },
    setError(state, error: string | null) {
      state.error = error;
    },
    clearError(state) {
      state.error = null;
    },
  },
  actions: {
    async fetchProjects({ commit, state }) {
      try {
        const response = await axios.get(`${BASE_URL}/projects/GetAllKafkaProjects`, {
          headers: { Authorization: `Bearer ${state.user?.token}` },
        });
        commit('setProjects', response.data);
      } catch (error) {
        commit('setError', 'Failed to fetch projects.');
      }
    },
    async createProject({ commit, state }, project: Project) {
      try {
        const response = await axios.post(`${BASE_URL}/projects/CreateKafkaProject`, project, {
          headers: { Authorization: `Bearer ${state.user?.token}` },
        });
        commit('addProject', response.data);
      } catch (error) {
        commit('setError', 'Failed to create project.');
      }
    },
    async updateProject({ commit, state }, project: Project) {
      try {
        const updatedProject = { ...project, projectId: project.projectId };
        await axios.put(`${BASE_URL}/projects/UpdateKafkaProject`, updatedProject, {
          headers: { Authorization: `Bearer ${state.user?.token}` },
        });
        commit('updateProject', updatedProject);
      } catch (error) {
        commit('setError', 'Failed to update project.');
      }
    },
    async deleteProject({ commit, state }, projectId: number) {
      try {
        await axios.delete(`${BASE_URL}/projects/DeleteKafkaProject/${projectId}`, {
          headers: { Authorization: `Bearer ${state.user?.token}` },
        });
        commit('deleteProject', projectId);
      } catch (error) {
        commit('setError', 'Failed to delete project.');
      }
    },
    async login({ commit }, { email, password, router }) {
      try {
        const response = await axios.post(`${BASE_URL}/Auth/login`, { email, password });
        const accessToken = response.data.accessToken;
        const user = { token: accessToken };
        commit('setUser', user);
        axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        router.push('/');
      } catch (error) {
        commit('setError', 'Login failed. Please check your credentials.');
      }
    },
  },
});

export default store;
