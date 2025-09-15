<template>
  <div>
    <NuxtRouteAnnouncer />
    <ClientOnly>
      <AppShell />
    </ClientOnly>
  </div>
</template>

<script setup lang="ts">
import AppShell from '~/components/app/AppShell.vue';
</script>
