<template>
  <div class="spotify-track">
    <template v-if="info">
      <div class="artwork">
        <Image :src="artwork" />
      </div>
      <div class="song-info">
        <div class="title">{{ title }}</div>
        <div class="artists">{{ artists?.join(', ') }}</div>
        <div>{{ id }}</div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import type { Track } from '@spotify/web-api-ts-sdk';

const props = defineProps<{ id: string }>();
const { sdk } = useSpotify();
const info = ref<Track | null>(null);
const title = computed(() => info.value?.name);
const artists = computed(() => info.value?.artists.map((a) => a.name));
const artwork = computed(() => info.value?.album.images[0]?.url);
const loadInfo = async () => {
  info.value = await sdk.tracks.get(props.id);
};
watch(
  () => props.id,
  async () => await loadInfo(),
  { immediate: true },
);
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.spotify-track {
  @include utilities.flex-row;
  .artwork {
    width: 20%;
    max-width: 100px;
    flex-shrink: 0;
  }
}
</style>
