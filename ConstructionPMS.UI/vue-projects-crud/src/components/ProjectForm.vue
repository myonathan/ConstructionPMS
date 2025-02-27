<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="max-w-md w-full bg-white rounded-lg shadow-md p-8">
      <h1 class="text-2xl font-bold text-center mb-6">{{ isEdit ? 'Edit' : 'Create' }} Project</h1>
      <form @submit.prevent="submitForm" class="space-y-4">
        <div>
          <label for="projectName" class="block text-sm font-medium text-gray-700">Project Name</label>
          <input
            id="projectName"
            v-model="project.projectName"
            placeholder="Enter project name"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="projectLocation" class="block text-sm font-medium text-gray-700">Project Location</label>
          <input
            id="projectLocation"
            v-model="project.projectLocation"
            placeholder="Enter project location"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="constructionStartDate" class="block text-sm font-medium text-gray-700">Construction Start Date</label>
          <input
            id="constructionStartDate"
            v-model="project.constructionStartDate"
            type="date"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="projectDetails" class="block text-sm font-medium text-gray-700">Project Details</label>
          <textarea
            id="projectDetails"
            v-model="project.projectDetails"
            placeholder="Enter project details"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          ></textarea>
        </div>
        <button
          type="submit"
          class="w-full bg-blue-600 text-white font-semibold py-2 rounded-md hover:bg-blue-700 transition duration-200"
        >
          {{ isEdit ? 'Update' : 'Create' }} Project
        </button>
        <button
          type="button"
          @click="goBack"
          class="w-full bg-gray-300 text-gray-700 font-semibold py-2 rounded-md hover:bg-gray-400 transition duration-200 mt-4"
        >
          Back to Project List
        </button>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useStore } from 'vuex';
import { useRoute, useRouter } from 'vue-router';

export default defineComponent({
  setup() {
    const store = useStore();
    const route = useRoute();
    const router = useRouter();
    const isEdit = !!route.params.id;

    const project = {
      projectId: isEdit ? Number(route.params.id) : 0,
      projectName: '',
      projectLocation: '',
      constructionStartDate: '',
      projectDetails: '',
      projectCreatorId: 'some-creator-id', // Replace with actual creator ID
    };

    const submitForm = async () => {
      if (isEdit) {
        await store.dispatch('updateProject', project);
      } else {
        await store.dispatch('createProject', project);
      }
      router.push('/');
    };

    const goBack = () => {
      router.push('/'); // Navigate back to the Project List
    };

    return { project, submitForm, isEdit, goBack };
  },
});
</script>

<style scoped>
/* Additional custom styles can go here if needed */
</style>