<template>
  <div class="unauthenticated-view">
    <h1>You are not logged in</h1>
    <p>Please log in to access the application</p>

    <div class="actions">
      <a :href="`/auth/login?returnUrl=${returnUrl}`">
        <Button
          label="Log in"
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
const returnUrl = computed(() => route.fullPath);
</script>

<style scoped lang="scss">
@use '@/styles/utilities.scss';

.unauthenticated-view {
  padding-block: calc(var(--default-spacing) * 4);
  height: 100%;
  text-align: center;

  @include utilities.flex-column;

  .actions {
    margin-top: calc(var(--default-spacing) * 2);
  }
}
</style>
