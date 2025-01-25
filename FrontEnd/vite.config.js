import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    outDir: '../BackEnd/wwwroot', // Output to the wwwroot folder
    emptyOutDir: true,
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5115', // Backend URL
        changeOrigin: true,
      },
    },
  }
})
