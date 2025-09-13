<template>
  <div class="session-detail-page">
    <div class="session-header">
      <h2>Maastricht fridge party</h2>
      <div class="actions" />
    </div>
    <NowPlaying />
    <div class="queue">
      <div class="header">
        <h2>Queue</h2>
        <div class="actions">
          <Button
            label="Add new song"
            icon="pi pi-plus"
          />
        </div>
      </div>
      <SessionPlaylist />
    </div>
  </div>
</template>

<script setup lang="ts">
import SessionPlaylist from '~/components/sessions/playlists/SessionPlaylist.vue';
import NowPlaying from '~/components/sessions/now-playing/NowPlaying.vue';

const route = useRoute();
const sessionId: string = route.params.id as string;
console.log(sessionId);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.session-detail-page {
  @include utilities.flex-column;
}

.session-header {
  @include utilities.flex-row-justify-between;
  h2 {
  }

  .actions {
  }
}

.queue {
  .header {
    @include utilities.flex-row-justify-between;
  }
  @include utilities.flex-column;
}
</style>
