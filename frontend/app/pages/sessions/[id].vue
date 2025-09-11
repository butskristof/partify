<template>
  <div class="session-detail-page">
    <h1>Session detail</h1>
    <h2>{{ sessionId }}</h2>
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const sessionId: string = route.params.id as string;
</script>

<style scoped lang="scss"></style>
