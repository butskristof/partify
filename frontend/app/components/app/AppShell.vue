<template>
  <div class="app">
    <div class="test-content">
      <h1>Partify</h1>
      <PrimeTest />
      <NuxtPage />
    </div>
  </div>
</template>

<script setup lang="ts">
import PrimeTest from '~/components/app/PrimeTest.vue';
</script>

<style scoped lang="scss">
.app {
  min-height: 100vh;

  background-color: var(--p-surface-500);
  color: var(--color-text);
}

.test-content {
  padding: var(--default-spacing);
  display: flex;
  flex-direction: column;
  gap: var(--default-spacing);
}
</style>
