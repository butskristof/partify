<template>
  <div class="spotify-auth-callback">
    <h1>Spotify auth callback</h1>
    <div>
      <h3>Code</h3>
      <PreformattedText :value="code" />
    </div>
    <div>
      <h3>State</h3>
      <PreformattedText :value="state" />
    </div>
    <div>
      <h3>Client ID</h3>
      <PreformattedText :value="clientId" />
    </div>
    <div>
      <h3>Client Secret</h3>
      <PreformattedText :value="clientSecret" />
    </div>
    <div>
      <h3>Basic auth</h3>
      <PreformattedText :value="basicAuth" />
    </div>

    <Button
      label="Get access token"
      @click="getAccessToken"
    />

    <div>
      <h3>Response</h3>
      <PreformattedText :value="responseBody" />
    </div>
    <div>
      <h3>Access token</h3>
      <PreformattedText :value="accessToken" />
    </div>
  </div>
</template>

<script setup lang="ts">
import PreformattedText from '~/components/common/PreformattedText.vue';

const route = useRoute();
const code = route.query.code as string;
const state = route.query.state as string;

const runtimeConfig = useRuntimeConfig();
const clientId = runtimeConfig.public.spotifyClientId;
const clientSecret = runtimeConfig.public.spotifyClientSecret;
const basicAuth = computed(() => btoa(`${clientId}:${clientSecret}`));

const responseBody = ref<unknown>(null);
const accessToken = computed(() => responseBody.value?.access_token);

const getAccessToken = async () => {
  const response = await $fetch('https://accounts.spotify.com/api/token', {
    method: 'POST',
    headers: {
      'content-type': 'application/x-www-form-urlencoded',
      Authorization: `Basic ${basicAuth.value}`,
    },
    body: new URLSearchParams({
      code,
      redirect_uri: 'http://127.0.0.1:3000/auth/callback',
      grant_type: 'authorization_code',
    }),
  });
  console.log(response);
  responseBody.value = response;
};

// app.get('/callback', function(req, res) {
//
//   var code = req.query.code || null;
//   var state = req.query.state || null;
//
//   if (state === null) {
//     res.redirect('/#' +
//       querystring.stringify({
//         error: 'state_mismatch'
//       }));
//   } else {
//     var authOptions = {
//       url: 'https://accounts.spotify.com/api/token',
//       form: {
//         code: code,
//         redirect_uri: redirect_uri,
//         grant_type: 'authorization_code'
//       },
//       headers: {
//         'content-type': 'application/x-www-form-urlencoded',
//         'Authorization': 'Basic ' + (new Buffer.from(client_id + ':' + client_secret).toString('base64'))
//       },
//       json: true
//     };
//   }
// });
</script>

<style scoped lang="scss"></style>
