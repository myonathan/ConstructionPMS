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
            v-model="formattedConstructionStartDate"
            type="date"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="projectCategory" class="block text-sm font-medium text-gray-700">Project Category</label>
          <select
            id="projectCategory"
            v-model="project.projectCategory"
            @change="checkOtherCategoryVisibility"
            required
            class="mt-1 border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option value="" disabled>Select a category</option>
            <option value="0">Education</option>
            <option value="1">Health</option>
            <option value="2">Office</option>
            <option value="3">Others</option>
          </select>
        </div>
        <div v-if="showOtherCategory"> <!-- Show this input if "Others" is selected -->
          <label for="otherCategory" class="block text-sm font-medium text-gray-700">Specify Other Category</label>
          <input
            id="otherCategory"
            v-model="project.otherCategory"
            placeholder="Enter other category"
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
import { defineComponent, ref, computed, nextTick, onMounted } from 'vue';
import { useStore } from 'vuex';
import { useRoute, useRouter } from 'vue-router';

export default defineComponent({
  setup() {
    const store = useStore();
    const route = useRoute();
    const router = useRouter();
    const isEdit = !!route.params.id;

    // Initialize project object
    const project = ref({
      projectId: isEdit ? Number(route.params.id) : 0,
      projectName: '',
      projectLocation: '',
      constructionStartDate: '',
      projectDetails: '',
      projectCategory: 0,
      otherCategory: ''
    });

    // Show/hide the Other Category input
    const showOtherCategory = ref(false);

    // If in edit mode, populate the project with the data from the store
    if (isEdit) {
      const existingProject = store.state.projects.find(p => p.projectId === project.value.projectId);
      if (existingProject) {
        project.value = { ...existingProject }; // Populate the project with existing data
      }
    }

    // Method to check if the Other Category input should be shown
    const checkOtherCategoryVisibility = () => {
      if (parseInt(project.value.projectCategory, 10) === 3) {
        console.log('others selected');
        showOtherCategory.value = true; // Show if "Others" is selected
      } else {
        showOtherCategory.value = false; // Hide if any other category is selected
        project.value.otherCategory = ''; // Clear the otherCategory value
      }
    };

    // Call the visibility check after initialization
    onMounted(() => {
      nextTick(() => {
        checkOtherCategoryVisibility();
        console.log('Component and children are fully rendered');
      });
    });

    // Computed property to format the construction start date for the date input
    const formattedConstructionStartDate = computed({
      get: () => {
        return project.value.constructionStartDate ? project.value.constructionStartDate.split('T')[0] : '';
      },
      set: (value) => {
        project.value.constructionStartDate = value;
      }
    });

    const submitForm = async () => {
      project.value.projectCategory = parseInt(project.value.projectCategory, 10); // Ensure it's an integer
      if (isEdit) {
        await store.dispatch('updateProject', project.value);
      } else {
        await store.dispatch('createProject', project.value);
      }
      router.push('/'); // Navigate back to the project list after submission
    };

    const goBack = () => {
      router.push('/'); // Navigate back to the Project List
    };

    return { project, submitForm, isEdit, goBack, formattedConstructionStartDate, showOtherCategory, checkOtherCategoryVisibility };
  },
});
</script>

<style scoped>
/* Additional custom styles can go here if needed */
</style>