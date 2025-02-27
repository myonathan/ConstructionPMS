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
  token: string; // Include token in the User interface
}

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
      const response = await axios.get('/api/projects', {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('setProjects', response.data);
    },
    async createProject({ commit }, project: Project) {
      const response = await axios.post('/api/projects', project, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('addProject', response.data);
    },
    async updateProject({ commit }, project: Project) {
      await axios.put(`/api/projects/${project.projectId}`, project, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('updateProject', project);
    },
    async deleteProject({ commit }, projectId: number) {
      await axios.delete(`/api/projects/${projectId}`, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('deleteProject', projectId);
    },
    async login({ commit }, { email, password }) {
      try {
        const response = await axios.post('https://localhost:7188/api/Auth/login', { email, password });
        
        // Assuming the response contains user ID, email, and token
        const user = {
          id: response.data.id, // User ID
          email: response.data.email, // User email
          token: response.data.token // User token
        };
        
        commit('setUser ', user); // Set user state with token (removed space)
        
        // Optionally, you can set the token in the Axios default headers for future requests
        axios.defaults.headers.common['Authorization'] = `Bearer ${user.token}`;
        
      } catch (error) {
        console.error('Login failed:', error);
        
        // Optionally, you can throw a more descriptive error
        throw new Error('Login failed. Please check your credentials and try again.');
      }
    },
  },
});

export default store;