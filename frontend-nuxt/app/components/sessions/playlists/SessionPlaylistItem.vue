<template>
  <div class="session-playlist-item">
    <div class="artwork">
      <Image :src="`/images/${item.artwork}`" />
    </div>
    <div class="song-info">
      <div class="title">{{ item.title }}</div>
      <div class="artist">{{ item.artist }}</div>
    </div>
    <div class="actions">
      <Button
        class="vote-button"
        icon="pi pi-heart"
        aria-label="Vote"
        label="12"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  item: {
    artwork: string;
    title: string;
    artist: string;
  };
}>();
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.session-playlist-item {
  border-radius: var(--default-radius);
  background-color: var(--color-surface-raised);
  padding: var(--default-spacing);

  @include utilities.flex-row;
  align-items: center;

  .artwork {
    width: 20%;
    max-width: 50px;
  }

  .song-info {
    flex-grow: 1;

    .title {
      font-weight: var(--font-weight-semibold);
    }

    .artist {
      color: var(--color-text-muted);
    }
  }

  .vote-button {
    //background-color: ;
  }
}
</style>
