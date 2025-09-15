<template>
  <div class="spotify-auth-test">
    <h1>Spotify auth test</h1>
    <div
      v-if="profile != null"
      class="profile"
    >
      <h2>Profile</h2>
      <PreformattedText :value="profile" />

      <h2>Access token</h2>
      <PreformattedText :value="accessToken" />
    </div>
  </div>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';
import type { AccessToken } from '@spotify/web-api-ts-sdk';

const { profile, sdk } = useSpotify();
const accessToken = ref<AccessToken | null>(null);

const init = async () => {
  accessToken.value = await sdk.getAccessToken();
};
onMounted(() => {
  init();
});
</script>

<style scoped lang="scss"></style>
