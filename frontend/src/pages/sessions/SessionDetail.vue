<template>
  <div class="page-session-detail">
    <h1>Session</h1>
    <h2>{{ sessionId }}</h2>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';

const route = useRoute();
const sessionId: string = route.params.id as string;
</script>

<style scoped lang="scss"></style>
