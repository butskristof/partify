<template>
  <div class="login">
    <h1>Login with Spotify</h1>
    <div>client id: {{ clientId }}</div>

    <Button
      label="Login"
      @click="redirectForLogin"
    />
  </div>
</template>

<script setup lang="ts">
const runtimeConfig = useRuntimeConfig();
const clientId = runtimeConfig.public.spotifyClientId;
// const clientSecret = runtimeConfig.public.spotifyClientSecret;
const state = 'thisisstate';
const scope = 'user-read-playback-state user-modify-playback-state';
// res.redirect('https://accounts.spotify.com/authorize?' +
//   querystring.stringify({
//     response_type: 'code',
//     client_id: client_id,
//     scope: scope,
//     redirect_uri: redirect_uri,
//     state: state
//   }));
// });

const redirectForLogin = () => {
  console.log('Redirecting...');
  return navigateTo(
    {
      path: 'https://accounts.spotify.com/authorize',
      query: {
        response_type: 'code',
        client_id: clientId,
        redirect_uri: 'http://127.0.0.1:3000/auth/callback',
        state,
        scope,
      },
    },
    { external: true },
  );
};
</script>

<style scoped lang="scss"></style>
