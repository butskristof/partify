<template>
  <div class="unauthenticated-view">
    <h1>You are not logged in</h1>
    <p>Please log in to access the application</p>

    <div class="actions">
      <a :href="`/auth/login/spotify?returnUrl=${returnUrl}`">
        <Button
          label="Log in with Spotify"
          icon="pi pi-sign-in"
        />
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const returnUrl = computed(() => window.location.origin + route.fullPath);
</script>

<style scoped lang="scss">
@use '@/styles/utilities.scss';

.unauthenticated-view {
  @include utilities.full-page-info;
}
</style>
