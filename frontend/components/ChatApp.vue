<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 flex flex-col">
    <header class="bg-gradient-to-r from-indigo-600 to-purple-700 text-white p-4 shadow-lg">
      <div class="container mx-auto max-w-4xl flex items-center">
        <div class="w-12 h-12 bg-white rounded-full p-2 flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="text-indigo-600 w-8 h-8">
            <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"/>
          </svg>
        </div>
        <div class="ml-4">
          <h1 class="text-2xl font-bold">{{ appName }}</h1>
          <p class="text-indigo-200 text-sm">{{ appDescription }}</p>
        </div>
      </div>
    </header>

    <main class="flex-grow flex flex-col items-center py-6 px-4">
      <div class="w-full max-w-4xl bg-white rounded-2xl shadow-xl overflow-hidden flex flex-col h-[70vh]">
        <div class="bg-gray-50 px-6 py-4 border-b">
          <h2 class="text-lg font-semibold text-gray-800">Conversation</h2>
          <p class="text-xs text-gray-500">Session ID: {{ sessionId }}</p>
        </div>
        
        <div ref="messagesContainer" class="flex-grow overflow-y-auto p-4 bg-gray-50">
          <div v-if="messages.length === 0" class="flex flex-col items-center justify-center h-full text-gray-500 py-12">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mb-4 text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z" />
            </svg>
            <p class="text-center">Send a message to start the conversation</p>
            <p class="text-sm mt-2 text-center">Your messages will appear here</p>
          </div>
          
          <div v-else class="space-y-4">
            <div 
              v-for="message in messages" 
              :key="message.id"
              :class="[
                'p-4 rounded-2xl max-w-[85%] transition-all duration-300 shadow-sm',
                message.role === 'user' 
                  ? 'bg-gradient-to-r from-blue-50 to-blue-100 ml-auto border border-blue-200' 
                  : message.role === 'assistant' 
                    ? 'bg-gradient-to-r from-gray-50 to-gray-100 mr-auto border border-gray-200' 
                    : 'bg-red-50 mr-auto'
              ]"
            >
              <div class="flex items-start">
                <div v-if="message.role === 'user'" class="bg-blue-500 rounded-full w-8 h-8 flex items-center justify-center mr-3">
                  <span class="text-white text-sm font-bold">U</span>
                </div>
                <div v-else class="bg-indigo-500 rounded-full w-8 h-8 flex items-center justify-center mr-3">
                  <span class="text-white text-sm font-bold">AI</span>
                </div>
                
                <div class="flex-grow">
                  <div class="font-semibold text-sm mb-1 text-gray-700">
                    {{ message.role === 'user' ? 'You' : 'Assistant' }}
                  </div>
                  
                  <div v-if="message.isLoading" class="flex space-x-2 py-2">
                    <div class="w-3 h-3 bg-gray-400 rounded-full animate-bounce"></div>
                    <div class="w-3 h-3 bg-gray-400 rounded-full animate-bounce" style="animation-delay: 0.2s"></div>
                    <div class="w-3 h-3 bg-gray-400 rounded-full animate-bounce" style="animation-delay: 0.4s"></div>
                  </div>
                  <div v-else class="text-gray-800 whitespace-pre-wrap">
                    {{ message.content }}
                  </div>
                </div>
              </div>
              
              <div class="text-xs text-gray-500 mt-2 text-right">
                {{ formatTime(message.timestamp) }}
              </div>
            </div>
          </div>
        </div>

        <div class="bg-white border-t p-4">
          <div class="flex gap-3 items-stretch">
            <textarea
              v-model="userInput"
              placeholder="Type your message here..."
              class="flex-grow p-4 border border-gray-300 rounded-2xl focus:outline-none focus:ring-2 focus:ring-indigo-500 resize-none min-h-[60px] transition-all"
              :class="{'ring-2 ring-indigo-500': userInput}"
              @keydown.enter.exact.prevent="sendMessage"
              @keydown.shift.enter="userInput += '\n'"
            ></textarea>
            <button
              @click="sendMessage"
              :disabled="isLoading"
              :class="[
                'px-6 bg-gradient-to-r from-indigo-600 to-purple-700 text-white rounded-2xl hover:opacity-90 transition-all flex items-center justify-center shadow-md',
                isLoading ? 'opacity-70 cursor-not-allowed' : '',
                userInput ? 'scale-105' : ''
              ]"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 5l7 7-7 7M5 5l7 7-7 7" />
              </svg>
            </button>
          </div>
          <p class="text-xs text-gray-500 mt-2 text-center">Press Enter to send, Shift+Enter for new line</p>
        </div>
      </div>
    </main>

    <footer class="bg-gray-800 text-white py-4 text-center text-sm">
      <p>Powered by IntelliBot Cognitive Engine</p>
      <p class="mt-1 text-gray-400">Session ID: {{ sessionId }}</p>
    </footer>
  </div>
</template>

<script setup>
const { $config } = useNuxtApp()
const appName = $config.public.appName
const appDescription = $config.public.appDescription

const userInput = ref('')
const messages = ref([])
const isLoading = ref(false)
const sessionId = ref('')
const messagesContainer = ref(null)

onMounted(() => {
  const savedSession = localStorage.getItem('chatSession')
  if (savedSession) {
    try {
      const sessionData = JSON.parse(savedSession)
      sessionId.value = sessionData.sessionId
      messages.value = sessionData.messages
    } catch (e) {
      console.error('Failed to parse session data', e)
      createNewSession()
    }
  } else {
    createNewSession()
  }
  
  scrollToBottom()
})

const createNewSession = () => {
  sessionId.value = crypto.randomUUID()
  messages.value = []
  saveSession()
}

const saveSession = () => {
  const sessionData = {
    sessionId: sessionId.value,
    messages: messages.value
  }
  localStorage.setItem('chatSession', JSON.stringify(sessionData))
}

const formatTime = (date) => {
  const d = new Date(date)
  return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}

const scrollToBottom = () => {
  nextTick(() => {
    if (messagesContainer.value) {
      messagesContainer.value.scrollTop = messagesContainer.value.scrollHeight
    }
  })
}

const sendMessage = async () => {
  if (!userInput.value.trim() || isLoading.value) return
  
  const content = userInput.value
  userInput.value = ''
  isLoading.value = true
  saveSession()
  
  messages.value.push({
    id: Date.now(),
    content,
    role: 'user',
    timestamp: new Date()
  })
  
  scrollToBottom()
  
  try {
    const assistantMessage = {
      id: Date.now() + 1,
      content: '',
      role: 'assistant',
      timestamp: new Date(),
      isLoading: true
    }
    messages.value.push(assistantMessage)
    scrollToBottom()
    
    const { data: response } = await useFetch('/conversation', {
      baseURL: $config.public.apiBaseUrl,
      method: 'POST',
      body: {
        sessionId: sessionId.value,
        content
      }
    })
    
    assistantMessage.content = response.value?.reply || 'No response received'
    assistantMessage.isLoading = false
  } catch (error) {
    messages.value.push({
      id: Date.now() + 2,
      content: `Error: ${error.message || 'Failed to get response'}`,
      role: 'system',
      timestamp: new Date()
    })
  } finally {
    isLoading.value = false
    saveSession() 
    scrollToBottom()
  }
}
</script>

<style scoped>
.chat-container {
  scroll-behavior: smooth;
}

.message-enter-active {
  transition: all 0.3s ease;
}
.message-enter-from {
  opacity: 0;
  transform: translateY(10px);
}
</style>