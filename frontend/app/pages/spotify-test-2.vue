<template>
  <div class="spotify-test">
    <Button
      label="Start"
      @click="start"
    />
    <div
      v-if="playbackState"
      class="now-playing"
    >
      <h2>Now playing</h2>
      <div v-if="nowPlayingItem">{{ nowPlayingItem.name }} ({{ nowPlayingItem.id }})</div>
      <div>Playing: {{ isPlaying }}</div>
      <div>Remaining: {{ msToTime(remaining!) }}</div>
      <div>should queue next: {{ shouldQueueNext }}</div>
    </div>

    <div class="spotify-queue">
      <h2>Spotify queue</h2>
      <div>queue length: {{ userQueue?.queue.length }}</div>
      <div>queue w/o current length: {{ userQueueWithoutNowPlayingItem?.length }}</div>
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
import { getTestQueue } from '~/utilities/test-data';
import PreformattedText from '~/components/common/PreformattedText.vue';
import SpotifyTrack from '~/components/spotify-test/SpotifyTrack.vue';
import type { PlaybackState, Queue } from '@spotify/web-api-ts-sdk';
import { msToTime } from '~/utilities/formatting';

const { sdk, profile } = useSpotify();

const queue = ref(getTestQueue().slice(0, 5));

const interval_updateUserQueue = ref<number | null>(null);
const interval_updatePlaybackState = ref<number | null>(null);
onMounted(() => {
  interval_updateUserQueue.value = setInterval(updateUserQueue, 1000);
  interval_updatePlaybackState.value = setInterval(updatePlaybackState, 1000);
});
onUnmounted(() => {
  if (interval_updateUserQueue.value != null) clearInterval(interval_updateUserQueue.value);
  if (interval_updatePlaybackState.value != null) clearInterval(interval_updatePlaybackState.value);
});

const playbackState = ref<PlaybackState | null>(null);
const updatePlaybackState = async () => {
  playbackState.value = await sdk.player.getPlaybackState();
};
const isPlaying = computed(() => playbackState.value?.is_playing ?? false);
const remaining = computed(() => {
  if (playbackState.value == null) return null;
  const progress = playbackState.value.progress_ms;
  const duration = playbackState.value.item.duration_ms;
  return duration - progress;
});
const nowPlayingItem = computed(() => playbackState.value?.item);
watch(nowPlayingItem, () => {
  // remove from queue
  queue.value = queue.value.filter((i) => i.id !== nowPlayingItem.value.id);
});
const device = computed(() => playbackState.value?.device);

const userQueue = ref<Queue | null>(null);
const updateUserQueue = async () => {
  userQueue.value = await sdk.player.getUsersQueue();
};
const userQueueWithoutNowPlayingItem = computed(() => {
  if (userQueue.value == null) return null;
  return userQueue.value.queue.filter((i) => i.id != nowPlayingItem.value?.id);
});

const shouldQueueNext = computed(() => {
  if (!isPlaying.value) return false;
  if (remaining.value == null || remaining.value > 10_000) return false;
  console.log(userQueueWithoutNowPlayingItem.value);
  if (userQueueWithoutNowPlayingItem.value?.length > 0) return false;

  return true;
});
watch(shouldQueueNext, async (newValue, oldValue) => {
  if (newValue === true && oldValue === false) {
    console.log('QUEUE NEXT');

    try {
      const nextTrack = getNextTrack();
      await sdk.player.addItemToPlaybackQueue(toTrackUri(nextTrack.id));
    } catch (e) {
      console.log('error adding to queue', e);
    }
    await updateUserQueue();
  }
});

const toTrackUri = (id: string) => `spotify:track:${id}`;

const start = async () => {
  const nextTrack = getNextTrack();
  await sdk.player.startResumePlayback(
    device.value.id,
    undefined,
    [toTrackUri(nextTrack.id)],
    undefined,
    0,
  );
};

const getNextTrack = () => {
  // random from queue
  const index = Math.floor(Math.random() * queue.value.length);
  return queue.value[index];
};
</script>

<style scoped lang="scss"></style>
