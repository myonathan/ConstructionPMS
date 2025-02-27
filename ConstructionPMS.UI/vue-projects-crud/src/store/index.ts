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
    setUser(state, user: User | null) { // Mutation to set user
      state.user = user;
    },
  },
  actions: {
    async fetchProjects({ commit }) {
      const response = await axios.get('https://localhost:7188/api/projects', {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('setProjects', response.data);
    },
    async createProject({ commit }, project: Project) {
      const response = await axios.post('https://localhost:7188/api/projects', project, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('addProject', response.data);
    },
    async updateProject({ commit }, project: Project) {
      await axios.put(`https://localhost:7188/api/projects/${project.projectId}`, project, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('updateProject', project);
    },
    async deleteProject({ commit }, projectId: number) {
      await axios.delete(`https://localhost:7188/api/projects/${projectId}`, {
        headers: {
          Authorization: `Bearer ${this.state.user?.token}` // Include token in the request
        }
      });
      commit('deleteProject', projectId);
    },
    async login({ commit }, { email, password, router }) {
      try {
        const response = await axios.post('https://localhost:7188/api/Auth/login', { email, password });
        
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