<template>
  <div class="app-shell">
    <h1>Partify</h1>
    <PrimeTest />
  </div>
</template>

<script setup lang="ts">
import PrimeTest from '~/components/app/PrimeTest.vue';
</script>

<style scoped lang="scss">
.app-shell {
  background-color: var(--p-surface-500);
  color: var(--color-text);
  padding: var(--default-spacing);
}
</style>
