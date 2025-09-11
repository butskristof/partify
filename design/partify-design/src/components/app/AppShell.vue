<template>
  <div class="app-shell">
    <main>
      <div class="elements">
        <div class="progress-bars">
          <ProgressBar :progress="1" />
          <ProgressBar :progress="0.2" />
        </div>
        <div class="vote-buttons">
          <VoteButton :voted="false" />
          <VoteButton :voted="true" />
        </div>
        <div class="buttons">
          <GeneralButton />
        </div>
        <div class="queue">
          <QueueItem
            v-for="i in 3"
            :key="i"
          />
        </div>
        <Surfaces />
      </div>
    </main>
  </div>
</template>

<script setup>
import ProgressBar from '@/components/elements/ProgressBar.vue';
import VoteButton from '@/components/elements/VoteButton.vue';
import GeneralButton from '@/components/elements/GeneralButton.vue';
import QueueItem from '@/components/elements/QueueItem.vue';
import Surfaces from '@/components/elements/Surfaces.vue';
</script>

<style scoped lang="scss">
.app-shell {
  display: flex;
  height: 100%;

  background-color: var(--color-surface-2);
}

main {
  padding: var(--default-spacing);
  width: 100%;
}

.elements {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;

  .progress-bars {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .vote-buttons {
    display: flex;
    flex-direction: row;
    gap: 1rem;
  }

  .queue {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
}
</style>
