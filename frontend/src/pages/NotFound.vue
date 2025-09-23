<template>
  <div class="page-not-found">
    <h1>Page not found</h1>
    <p>Sorry, the page you requested could not be found.</p>

    <div class="actions">
      <router-link :to="{ name: routes.home.name }">
        <Button
          label="Go to overview"
          icon="pi pi-home"
        />
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { routes } from '@/router/routes.ts';
</script>

<style scoped lang="scss">
@use '@/styles/utilities.scss';

.page-not-found {
  @include utilities.full-page-info;
}
</style>
