<!-- src/App.vue -->
<template>
  <div id="app">
    <router-view></router-view> <!-- This is where routed components will be displayed -->
    <ErrorModal />
  </div>
</template>

<script lang="ts">
import ErrorModal from './components/ErrorModal.vue';
export default {
  name: 'App',
  components: { ErrorModal },
};
</script>

<style>
/* Add any global styles here */
</style>