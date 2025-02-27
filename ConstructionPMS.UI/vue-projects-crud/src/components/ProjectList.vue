<template>
  <div class="p-4">
    <h1 class="text-3xl font-bold mb-4">Projects</h1>
    <router-link to="/projects/new" class="btn btn-primary mb-4">Create New Project</router-link>
    <ul class="space-y-2">
      <li 
        v-for="project in projects" 
        :key="project.projectId" 
        class="flex items-center justify-between p-2 border rounded shadow hover:bg-gray-100"
      >
        <router-link 
          :to="`/projects/${project.projectId}`" 
          class="text-blue-600 hover:underline font-medium"
        >
          {{ project.projectName }}
        </router-link>
        <button 
          @click="deleteProject(project.projectId)" 
          class="ml-4 text-red-600 hover:text-red-800 transition-colors"
        >
          Delete
        </button>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useStore } from 'vuex';

export default defineComponent({
  setup() {
    const store = useStore();
    const projects = store.state.projects;

    const deleteProject = (projectId: number) => {
      store.dispatch('deleteProject', projectId);
    };

    // Fetch projects when the component is mounted
    store.dispatch('fetchProjects');

    return { projects, deleteProject };
  },
});
</script>

<style scoped>
/* Add any additional styles here if needed */
</style>