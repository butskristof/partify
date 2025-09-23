import { fileURLToPath, URL } from 'node:url';

import { defineConfig, loadEnv, type ProxyOptions } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueDevTools from 'vite-plugin-vue-devtools';
// @ts-expect-error https://github.com/gxmari007/vite-plugin-eslint/issues/79
import eslint from 'vite-plugin-eslint';
import Components from 'unplugin-vue-components/vite';
import { PrimeVueResolver } from '@primevue/auto-import-resolver';
import basicSsl from '@vitejs/plugin-basic-ssl';

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  // the env isn't readily available, so we first create an object using Node's environment
  // and merge Vite's environment into it
  const env = { ...process.env, ...loadEnv(mode, process.cwd()) };

  // while developing the /api and /auth routes will be proxied to the BFF
  // when deployed, the BFF will serve the SPA assets and have these routes on the host itself
  // this way, we can emulate running the BFF and SPA running on the same host while using the
  // dev server for local development
  // changing the origin will make the BFF believe the dev server is running on the same origin,
  // but keep in mind it's including Secure cookies over http because it's localhost
  const BFF_PATHS = ['/api', '/auth'];
  const bffProxy = BFF_PATHS.reduce(
    (config, path) => {
      config[path] = {
        target: env.VITE_PARTIFY_BFF_BASEURL,
        secure: false, // don't verify the BFF's certificate (it'll be self-signed)
        changeOrigin: true,
      };
      return config;
    },
    {} as Record<string, ProxyOptions>,
  );

  return {
    plugins: [
      vue(),
      Components({
        resolvers: [PrimeVueResolver()],
      }),
      vueDevTools(),
      eslint(),
      basicSsl(),
    ],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    server: {
      proxy: {
        ...bffProxy,
      },
    },
  };
});
