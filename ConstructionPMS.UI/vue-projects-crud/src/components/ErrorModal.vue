<template>
    <div v-if="error" class="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
      <div class="bg-white rounded-lg shadow-lg p-6 max-w-sm">
        <h2 class="text-lg font-semibold text-red-600">Error</h2>
        <p class="text-gray-700 mt-2">{{ error }}</p>
        <button @click="closeModal" class="mt-4 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">
          Close
        </button>
      </div>
    </div>
  </template>
  
  <script>
  import { mapState, mapMutations } from 'vuex';
  
  export default {
    computed: {
      ...mapState(['error']),
    },
    methods: {
      ...mapMutations(['clearError']),
      closeModal() {
        this.clearError();
      },
    },
  };
  </script>
  