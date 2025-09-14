<template>
  <div class="spotify-test">
    <h2>Spotify test</h2>

    <PlaybackManager @selected-device-changed="setSelectedDevice" />
    <div>selected device: {{ selectedDevice }}</div>

    <div>
      <Button
        label="Play"
        @click="play"
      />
      <Button
        label="Queue next"
        @click="queueNext"
      />
    </div>

    <div class="queue">
      <h3>Queue</h3>
      <div
        v-for="item in queue"
        :key="item.uri"
        class="queue"
      >
        <QueueItem :uri="item.uri" />
      </div>
    </div>

    <div class="playback-state">
      <h3>Playback state</h3>
      <div>Progress: {{ progress }} / {{ duration }}</div>
      <PreformattedText
        v-if="false"
        :value="playbackState"
      />
    </div>

    <div class="playback-queue">
      <h3>Playback queue</h3>
      <PreformattedText :value="actualQueue" />
    </div>
  </div>
</template>

<script setup lang="ts">
import QueueItem from '~/components/spotify-test/QueueItem.vue';
import PlaybackManager from '~/components/spotify-test/PlaybackManager.vue';
import PreformattedText from '~/components/common/PreformattedText.vue';

const runtimeConfig = useRuntimeConfig();
const accessToken = runtimeConfig.public.spotifyAccessToken;

const queue = ref([
  { uri: 'spotify:track:1Oq4ei25SeGivbXLcCnz7z' },
  { uri: 'spotify:track:6WK9dVrRABMkUXFLNlgWFh' },
  { uri: 'spotify:track:6AtZLIzUINvExIUy4QhdjP' },
]);

const selectedDevice = ref<string | null>(null);
const setSelectedDevice = (device: string | null) => (selectedDevice.value = device);

const play = async () => {
  const item = queue.value.shift();
  await $fetch('https://api.spotify.com/v1/me/player/play', {
    method: 'PUT',
    query: {
      device_id: selectedDevice.value,
    },
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: {
      uris: [item.uri],
      position_ms: 0,
    },
  });
};
const queueNext = async () => {
  const item = queue.value.shift();
  if (item == null) return;
  await $fetch('https://api.spotify.com/v1/me/player/queue', {
    method: 'POST',
    query: {
      uri: item.uri,
    },
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });
};

const playbackState = ref<unknown>(null);
const playbackQueue = ref<unknown>(null);
const actualQueue = computed(() => {
  const currentItem = playbackState.value?.item.uri;
  const queueUris = [
    ...new Set(
      playbackQueue.value?.queue
        .map((item: { uri: string }) => item.uri)
        .filter((i) => i !== currentItem),
    ),
  ];
  console.log('current item', currentItem);
  console.log('queue uris', queueUris);
  return queueUris;
});
const updatePlaybackState = async () => {
  const response = await $fetch('https://api.spotify.com/v1/me/player', {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });
  playbackState.value = response;
  const queueResponse = await $fetch('https://api.spotify.com/v1/me/player/queue', {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });
  playbackQueue.value = queueResponse;
};
updatePlaybackState();

const interval = ref<number | null>(null);
onMounted(() => {
  interval.value = setInterval(updatePlaybackState, 5000);
});
onUnmounted(() => {
  if (interval.value !== null) clearInterval(interval.value);
});
const duration = computed(() => playbackState.value?.item?.duration_ms);
const progress = computed(() => playbackState.value?.progress_ms);
const remaining = computed(() => duration.value - progress.value);
watch(remaining, () => {
  if (remaining.value < 10000) {
    // play();
    console.log('QUEUE NEXT!');
    if (actualQueue.value.includes(queue.value[0])) {
      console.log('already queued');
      return;
    }
    queueNext();
  }
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';
.queue {
  @include utilities.flex-column;
  gap: 1rem;
}
</style>
