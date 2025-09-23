<template>
  <div class="header-container">
    <header>
      <div class="app-title">Partify</div>
    </header>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '@/styles/utilities';

.header-container {
  // header always has a predefined height and should always still to the top
  height: var(--app-header-height);
  position: sticky;
  top: 0;
  z-index: 1;

  border-bottom: 1px solid var(--p-surface-400);
  box-shadow: 0 1px 2px 0 rgb(255 255 255 / 0.05);
  background-color: hsl(from var(--p-surface-500) h s l / 0.75);
  backdrop-filter: blur(10px);
}

header {
  @include utilities.app-container;
  & {
    width: 100%;
    height: 100%;

    // vertically center the content inside the header
    @include utilities.flex-row-justify-between-align-center(false);
    gap: calc(var(--default-spacing) * 2);
    padding-inline: var(--default-spacing);
  }

  .app-title {
    flex-shrink: 0;
    @include utilities.bold;
    @include utilities.font-size(2xl);
  }
}
</style>
