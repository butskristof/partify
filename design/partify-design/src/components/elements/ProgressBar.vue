<template>
  <div class="progress-bar">
    <div class="progress" />
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  progress: {
    type: Number,
    required: true,
  },
});

const width = computed(() => `${props.progress * 100}%`);
</script>

<style scoped lang="scss">
.progress-bar {
  width: 100%;
  height: 10px;
  border-radius: 10px;
  background-color: var(--color-background);

  .progress {
    width: v-bind('width');
    height: 100%;
    border-radius: 10px;
    background-color: var(--color-primary);
    background: linear-gradient(to right, var(--color-primary), var(--color-secondary));
  }
}
</style>
