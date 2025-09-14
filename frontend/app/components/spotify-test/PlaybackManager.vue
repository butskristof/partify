<template>
  <div class="playback-manager">
    <h3>Available devices</h3>
    <Listbox
      v-model="selectedDevice"
      :options="availableDevices"
      option-label="name"
    />
    <Button
      label="Transfer playback"
      @click="transferPlayback"
    />
  </div>
</template>

<script setup lang="ts">
const emit = defineEmits<{
  (e: 'selected-device-changed', device: string | null): void;
}>();

const runtimeConfig = useRuntimeConfig();
const accessToken = runtimeConfig.public.spotifyAccessToken;

const availableDevices = (
  await $fetch('https://api.spotify.com/v1/me/player/devices', {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })
).devices;
const selectedDevice = ref(null);
watch(selectedDevice, () => {
  console.log(selectedDevice.value.id);
  emit('selected-device-changed', selectedDevice.value.id);
});
</script>

<style scoped lang="scss"></style>
