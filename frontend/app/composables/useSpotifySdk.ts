import { SpotifyApi } from '@spotify/web-api-ts-sdk';
let sdk: SpotifyApi;

export const useSpotifySdk = () => {
  console.log('useSpotifySdk');
  const runtimeConfig = useRuntimeConfig();
  if (sdk) {
    console.log('using existing sdk');
    return sdk;
  }

  console.log('creating new sdk');
  const clientId = runtimeConfig.public.spotifyClientId;
  sdk = SpotifyApi.withUserAuthorization(clientId, 'http://127.0.0.1:3000', [
    'user-read-playback-state',
    'user-modify-playback-state',
  ]);
  console.log('sdk created', sdk);

  return sdk;
};
