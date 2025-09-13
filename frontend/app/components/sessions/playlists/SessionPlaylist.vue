<template>
  <div class="session-playlist">
    <IconField>
      <InputIcon class="pi pi-search" />
      <InputText
        v-model="searchValue"
        class="search-input"
        placeholder="Search the queue"
      />
    </IconField>
    <SessionPlaylistItem
      v-for="(item, i) in filteredData"
      :key="i"
      :item
    />
  </div>
</template>

<script setup lang="ts">
import SessionPlaylistItem from '~/components/sessions/playlists/SessionPlaylistItem.vue';

const searchValue = ref('');
const data = [
  {
    artwork: 'artwork-01.png',
    title: 'Time after time',
    artist: 'Cyndi Lauper',
  },
  {
    artwork: 'artwork-02.png',
    title: 'Dreams',
    artist: 'Fleetwood Mac',
  },
  {
    artwork: 'artwork-03.png',
    title: 'Enjoy the silence',
    artist: 'Depeche Mode',
  },
];
const filteredData = computed(() => {
  if (!searchValue.value) return data;

  const search = searchValue.value.toLowerCase();
  return data.filter(
    (item) =>
      item.title.toLowerCase().includes(search) || item.artist.toLowerCase().includes(search),
  );
});
</script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

.search-input {
  width: 100%;
}

.session-playlist {
  @include utilities.flex-column;
}
</style>
