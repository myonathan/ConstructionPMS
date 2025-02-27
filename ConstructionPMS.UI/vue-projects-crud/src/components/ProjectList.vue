<template>
  <div>
    <h1 class="text-2xl font-bold">Projects</h1>
    <router-link to="/projects/new" class="btn btn-primary">Create New Project</router-link>
    <ul>
      <li v-for="project in projects" :key="project.projectId" class="my-2">
        <router-link :to="`/projects/${project.projectId}`" class="text-blue-500 hover:underline">{{ project.projectName }}</router-link>
        <button @click="deleteProject(project.projectId)" class="ml-2 text-red-500">Delete</button>
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

    store.dispatch('fetchProjects');

    return { projects, deleteProject };
  },
});
</script>