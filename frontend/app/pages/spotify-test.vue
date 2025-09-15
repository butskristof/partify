<template>
  <div class="spotify-test">
    <h1>Spotify test</h1>
    <div>Current user: {{ username }}</div>
    <Button
      label="Start"
      @click="start"
    />

    <div class="playback-state">
      <h3>Playback state</h3>
      <h4>Currently playing</h4>
      <SpotifyTrack
        v-if="playbackState?.item"
        :id="playbackState?.item?.id ?? ''"
      />
      <div>Remaining: {{ msToTime(remaining) }}</div>
      <PreformattedText
        v-if="false"
        :value="playbackState"
      />
    </div>

    <div class="queue">
      <h2>Queue</h2>
      <SpotifyTrack
        v-for="item in queue"
        :id="item.id"
        :key="item.id"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import SpotifyTrack from '~/components/spotify-test/SpotifyTrack.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';
import type { Devices, PlaybackState, Queue, TrackItem } from '@spotify/web-api-ts-sdk';
import { getTestQueue } from '~/utilities/test-data';

const { name: username, sdk } = useSpotify();

const queue = ref(getTestQueue());

const availableDevices = ref<Devices | null>(null);
const loadAvailableDevices = async () => {
  availableDevices.value = await sdk.player.getAvailableDevices();
};
loadAvailableDevices();
const selectedDevice = computed(() => availableDevices.value?.devices[0]?.id ?? null);
const playbackState = ref<PlaybackState | null>(null);
const spotifyQueue = ref<TrackItem[] | null>(null);
const interval = ref<number | null>(null);
const loadPlaybackState = async () => {
  playbackState.value = await sdk.player.getPlaybackState();
  const q = await sdk.player.getUsersQueue();
  spotifyQueue.value = q.queue.filter((i) => i.id != playbackState.value?.item.id);
  // const current = await sdk.player.getCurrentlyPlayingTrack();
  // console.log(current);
};
onMounted(() => {
  console.log('mounted');
  interval.value = setInterval(loadPlaybackState, 1000);
});
loadPlaybackState();
onUnmounted(() => {
  console.log('unmounted');
  if (interval.value != null) clearInterval(interval.value);
});

const nextTrack = computed(() => queue.value[0]);
const start = async () => {
  if (nextTrack.value != null && selectedDevice.value != null) {
    await sdk.player.startResumePlayback(
      selectedDevice.value,
      undefined,
      [`spotify:track:${nextTrack.value.id}`],
      undefined,
      0,
    );
    queue.value.shift();
    await loadPlaybackState();
  }
};
const isQueueing = ref(false);
const queueNextTrack = async () => {
  console.log('queueNextTrack');
  if (isQueueing.value) {
    console.log('already queueing');
    return;
  }
  try {
    isQueueing.value = true;
    if (nextTrack.value) {
      await sdk.player.addItemToPlaybackQueue(`spotify:track:${nextTrack.value.id}`);
      queue.value.shift();
      await loadPlaybackState();
    }
  } finally {
    isQueueing.value = false;
  }
};

const remaining = computed(() => {
  if (playbackState.value == null) return null;
  const current = playbackState.value.progress_ms;
  const total = playbackState.value.item?.duration_ms;
  const remaining = total - current;
  return remaining;
});
watch(remaining, () => {
  if (remaining.value == null || remaining.value > 30_000) return;
  console.log('less than 30s remaining');
  console.log(
    'queue',
    spotifyQueue.value?.map((i) => i.id),
  );
  if (isQueueing.value) {
    console.log('already queueing');
    return;
  }
  if (
    spotifyQueue.value == null ||
    spotifyQueue.value?.length > 0 ||
    spotifyQueue.value.some((i) => i.id === nextTrack.value?.id)
  ) {
    console.log('next track already in queue');
    return;
  }
  console.log('QUEUE NEXT');
  queueNextTrack();
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.queue {
  @include utilities.flex-column;
}
</style>
