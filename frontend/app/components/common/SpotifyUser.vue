<template>
  <div class="spotify-user">
    <SpotifyUserAvatar />
    <div class="username">Kristof</div>
  </div>
</template>

<script setup lang="ts">
import SpotifyUserAvatar from '~/components/common/SpotifyUserAvatar.vue';
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.spotify-user {
  @include utilities.flex-row-align-center(false);
  gap: calc(var(--default-spacing) / 2);
}
</style>
