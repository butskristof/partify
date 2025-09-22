<template>
  <div class="app">
    <AppHeader />
    <div class="content">
      <div class="page">
        <main>
          <AuthenticatedView v-if="isAuthenticated" />
          <UnauthenticatedView v-else />
        </main>
      </div>

      <AppFooter />
    </div>
  </div>
</template>

<script setup lang="ts">
import AppHeader from '@/components/app/AppHeader.vue';
import AppFooter from '@/components/app/AppFooter.vue';
import { ref } from 'vue';
import AuthenticatedView from '@/components/app/AuthenticatedView.vue';
import UnauthenticatedView from '@/components/app/UnauthenticatedView.vue';

const isAuthenticated = ref(false);
</script>

<style scoped lang="scss">
@use '@/styles/utilities';

.app {
  min-height: 100vh;
  @include utilities.flex-column(false);

  .content {
    // make sure the content container fills at least the available screen space below the header
    min-height: calc(100vh - var(--app-header-height));
    @include utilities.flex-column(false);

    .page {
      @include utilities.app-container;

      & {
        flex-grow: 1;
        margin: var(--default-spacing);

        @include utilities.media-min-xl {
          margin-inline: auto;
          width: 100%;
        }
      }
    }
  }
}
</style>
