<template>
  <div>
    <h1 class="text-2xl font-bold">Projects</h1>
    <router-link to="/projects/new" class="btn btn-primary">Create New Project</router-link>
    
    <div v-if="loading" class="mt-4">Loading projects...</div>
    <div v-if="error" class="mt-4 text-red-500">{{ error }}</div>
    
    <ul v-if="!loading && !error">
      <li v-for="project in projects" :key="project.projectId" class="flex justify-between items-center border-b py-2">
        <div class="flex-1">
          <router-link :to="`/projects/${project.projectId}`" class="text-blue-600 hover:underline">
            {{ project.projectName }}
          </router-link>
          <div class="text-sm text-gray-600">
            Location: {{ project.projectLocation }} | 
            Start Date: {{ new Date(project.constructionStartDate).toLocaleDateString() }} | 
            Stage: {{ project.projectStage }}
          </div>
        </div>
        <button @click="deleteProject(project.projectId)" class="text-red-600 hover:underline">
          Delete
        </button>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useStore } from 'vuex';

export default defineComponent({
  setup() {
    const store = useStore();
    const projects = store.state.projects;
    const loading = ref(true);
    const error = ref<string | null>(null);

    const deleteProject = async (projectId: number) => {
      try {
        await store.dispatch('deleteProject', projectId);
      } catch (err) {
        console.error('Failed to delete project:', err);
        error.value = 'Failed to delete project. Please try again.';
      }
    };

    const fetchProjects = async () => {
      try {
        await store.dispatch('fetchProjects');
      } catch (err) {
        console.error('Failed to fetch projects:', err);
        error.value = 'Failed to load projects. Please try again.';
      } finally {
        loading.value = false;
      }
    };

    onMounted(fetchProjects); // Fetch projects when the component is mounted

    return { projects, loading, error, deleteProject };
  },
});
</script>

<style scoped>
/* Additional custom styles can go here if needed */
.btn {
  background-color: #3b82f6; /* Tailwind blue-500 */
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 0.375rem; /* Tailwind rounded-md */
  text-decoration: none;
}

.btn-primary:hover {
  background-color: #2563eb; /* Tailwind blue-600 */
}
</style>