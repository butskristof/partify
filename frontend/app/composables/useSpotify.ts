import { SpotifyApi } from '@spotify/web-api-ts-sdk';
import type { UserProfile } from '@spotify/web-api-ts-sdk';

let sdk: SpotifyApi;

export const useSpotify = () => {
  // console.log('useSpotify');
  const runtimeConfig = useRuntimeConfig();
  const profile = useState<UserProfile>();
  const name = computed(() => profile.value?.display_name);
  const initializing = ref(false);

  const setupSdk = async () => {
    console.log('setting up sdk');
    const clientId = runtimeConfig.public.spotifyClientId;
    sdk = SpotifyApi.withUserAuthorization(clientId, 'http://127.0.0.1:3000', [
      'user-read-playback-state',
      'user-modify-playback-state',
    ]);
  };
  const loadProfile = async () => {
    console.log('loading profile');
    profile.value = await sdk.currentUser.profile();
  };

  const initialize = async () => {
    initializing.value = true;
    try {
      await setupSdk();
      await loadProfile();
    } finally {
      initializing.value = true;
    }
  };

  if (sdk == null) {
    initialize();
  }

  return {
    sdk,
    profile,
    name,
  };
};
