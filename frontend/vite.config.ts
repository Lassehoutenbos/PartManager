import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueJsx(),
    vueDevTools({
      // Configure the editor to use for "Open in Editor" feature
      // Set LAUNCH_EDITOR environment variable to your editor (e.g., 'code', 'webstorm', 'idea')
      // Defaults to empty string to prevent errors when no editor is configured
      launchEditor: process.env.LAUNCH_EDITOR || '',
    }),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
})
