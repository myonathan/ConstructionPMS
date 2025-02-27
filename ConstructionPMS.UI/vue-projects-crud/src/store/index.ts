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
  token: string; // Only token is available from the login response
  // You can add other properties if you fetch user details later
}

// Define the base URL for the API
const BASE_URL = 'https://localhost:7188/api';

const store = createStore({
  state: {
    projects: [] as Project[],
    user: null as User | null, // User state
  },
  mutations: {
    setProjects(state, projects: Project[]) {
      state.projects = projects;
    },
    addProject(state, project: Project) {
      state.projects.push(project);
    },
    updateProject(state, updatedProject) {
      const index = state.projects.findIndex(p => p.projectId === updatedProject.projectId);
      if (index !== -1) {
        // Update the existing project
        state.projects.splice(index, 1, updatedProject);
      } else {
        // Optionally handle case where project is not found
        console.error('Project not found for update:', updatedProject.projectId);
      }
    },
    deleteProject(state, projectId: number) {
      state.projects = state.projects.filter(p => p.projectId !== projectId);
    },
    setUser(state, user: User | null) { // Mutation to set user
      state.user = user;
    },
  },
  actions: {
    async fetchProjects({ commit }) {
      const response = await axios.get(`${BASE_URL}/projects`, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('setProjects', response.data);
    },
    async createProject({ commit }, project: Project) {
      const response = await axios.post(`${BASE_URL}/projects`, project, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('addProject', response.data);
    },
    async updateProject({ commit }, project: Project) {
      console.log('Updating project:', project);
      // Ensure projectId is not modified
      const updatedProject = { ...project, projectId: project.projectId }; 
      await axios.put(`${BASE_URL}/projects`, updatedProject, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('updateProject', updatedProject);
    },
    async deleteProject({ commit }, projectId: number) {
      await axios.delete(`${BASE_URL}/projects/${projectId}`, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('deleteProject', projectId);
    },
    async login({ commit }, { email, password, router }) {
      try {
        const response = await axios.post(`${BASE_URL}/Auth/login`, { email, password });
        
        // Extract the access token from the response
        const accessToken = response.data.accessToken;

        // Create a user object with the token
        const user = {
          token: accessToken, // Store the access token
        };
        
        commit('setUser', user); // Set user state with token
        
        // Set the token in the Axios default headers for future requests
        axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        
        // Redirect to ProjectList after successful login
        router.push('/'); // Redirect to the root path (ProjectList)
        
      } catch (error) {
        console.error('Login failed:', error);
        throw new Error('Login failed. Please check your credentials and try again.');
      }
    },
  },
});

export default store;