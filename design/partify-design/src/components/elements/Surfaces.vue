<template>
  <div class="surfaces">
    <div class="outer-surface">
      <div>Outer surface</div>
      <div class="inner-surface">Inner surface</div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
.surfaces {
  .outer-surface {
    background-color: var(--color-surface-1);
    padding: var(--default-spacing);
    border-radius: var(--default-radius);
  }
  .inner-surface {
    background-color: var(--color-surface-2);
    padding: var(--default-spacing);
    border-radius: var(--default-radius);
  }
}
</style>
