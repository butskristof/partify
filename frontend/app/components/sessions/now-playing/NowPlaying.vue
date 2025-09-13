<template>
  <div class="now-playing">
    <div class="artwork">
      <div class="image-wrapper">
        <Image :src="`/images/${item.artwork}`" />
        <div class="added-by">
          <span>Added by</span>
          <SpotifyUser />
        </div>
      </div>
    </div>
    <div class="song-info">
      <div class="title">{{ item.title }}</div>
      <div class="artist">{{ item.artist }}</div>
    </div>
    <div class="song-progress">
      <ProgressBar
        class="progress-bar"
        :value="40"
        :show-value="false"
      />
      <div class="timing">
        <div class="now">1:48</div>
        <div class="total">4:29</div>
      </div>
    </div>
    <div
      v-if="false"
      class="actions"
    >
      <Button
        icon="pi pi-pause"
        rounded
        size="large"
        class="pause-button"
      />
      <Button
        icon="pi pi-step-forward"
        rounded
        severity="contrast"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import SpotifyUser from '~/components/common/SpotifyUser.vue';

const item = {
  artwork: 'artwork-04.png',
  title: 'Uptown Funk (feat. Bruno Mars)',
  artist: 'Mark Ronson, Bruno Mars',
};
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.now-playing {
  @include utilities.flex-column;
}

.artwork {
  @include utilities.flex-row;
  justify-content: center;

  .image-wrapper {
    position: relative;

    .added-by {
      @include utilities.flex-row-align-center(false);
      justify-content: flex-end;
      gap: calc(var(--default-spacing) / 2);
      padding-inline: var(--default-spacing);
      padding-block: calc(var(--default-spacing) / 2);

      position: absolute;
      width: 100%;
      bottom: 0;

      background-color: hsl(from var(--p-surface-500) h s l / 0.5);
      backdrop-filter: blur(10px);
    }
  }
}

.song-info {
  @include utilities.flex-column(false);

  .title {
    font-weight: var(--font-weight-semibold);
    font-size: var(--text-xl);
  }

  .artist {
    color: var(--color-text-muted);
    font-size: var(--text-lg);
  }
}

.song-progress {
  @include utilities.flex-column(false);
  gap: calc(var(--default-spacing) / 2);
  .progress-bar {
    --p-progressbar-height: 0.5rem;
    --p-progressbar-value-background: linear-gradient(
      to right,
      var(--color-primary),
      var(--color-secondary)
    );
  }

  .timing {
    @include utilities.flex-row-justify-between();
    color: var(--color-text-muted);
    font-size: var(--text-sm);
  }
}

.actions {
  display: grid;
  grid-template-columns: 1fr auto 1fr;
  align-items: center;
  gap: var(--default-spacing);

  // TODO replace w/ classes to target buttons
  > :first-child {
    grid-column: 2;
  }

  > :last-child {
    grid-column: 3;
    justify-self: start;
  }

  .pause-button {
    ::v-deep(.p-button-icon) {
      font-weight: var(--font-weight-bold);
    }
  }
}
</style>
