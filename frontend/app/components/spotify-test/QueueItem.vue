<template>
  <div class="queue-item">
    <div class="artwork">
      <Image :src="artwork" />
    </div>
    <div class="song-info">
      <div>{{ title }}</div>
      <div>{{ artists.join(', ') }}</div>
      <div>{{ id }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{ uri: string }>();
const runtimeConfig = useRuntimeConfig();
const accessToken = runtimeConfig.public.spotifyAccessToken;

const id = props.uri.split(':').pop();
const info: unknown = await $fetch(`https://api.spotify.com/v1/tracks/${id}`, {
  headers: {
    Authorization: `Bearer ${accessToken}`,
  },
});

const title = computed(() => info?.name);
const artists = computed(() => info?.artists.map((a) => a.name));
const artwork = computed(() => info?.album.images[0].url);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.queue-item {
  @include utilities.flex-row;

  .artwork {
    width: 20%;
    max-width: 100px;
  }
}
</style>
