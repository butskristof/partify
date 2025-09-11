<template>
  <div class="queue-item">
    <div class="artwork"></div>
    <div class="media-info">
      <div class="song">song name</div>
      <div class="artist">artist</div>
    </div>
    <div class="vote-button">
      <VoteButton :voted="false" />
    </div>
  </div>
</template>

<script setup>
import VoteButton from '@/components/elements/VoteButton.vue';
</script>

<style scoped lang="scss">
.queue-item {
  background-color: rgba(#1f2937, 50%);
  border-radius: var(--default-radius);
  padding: 0.5rem;

  display: flex;
  flex-direction: row;
  gap: 1rem;

  .artwork {
    height: 50px;
    width: 50px;
    background-color: var(--color-surface-1);
    border-radius: var(--default-radius);
  }

  .media-info {
    .song {
      //font-weight: bold;
    }

    .artist {
      color: var(--color-text-muted);
    }
  }

  .vote-button {
    margin-left: auto;
    display: flex;
    justify-content: center;
    align-items: center;
  }
}
</style>
