export default defineNuxtConfig({
  modules: ['@nuxtjs/tailwindcss'],
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.API_BASE_URL || 'http://localhost:5050/api',
      appName: process.env.APP_NAME || 'IntelliBot Assistant',
      appDescription: process.env.APP_DESCRIPTION || 'AI-powered conversational assistant'
    }
  },
  build: {
    transpile: []
  },
  ssr: false,
  app: {
    head: {
      title: 'IntelliBot Assistant',
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
      ]
    }
  }
})