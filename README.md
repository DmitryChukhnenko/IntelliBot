# IntelliBot  

**IntelliBot** — простой, но мощный чат-бот на основе ИИ, созданный с использованием .NET. Включает отдельный бэкенд для обработки данных и фронтенд веб-приложения для взаимодействия с пользователем. Весь проект контейнеризован с помощью Docker для удобной настройки и развертывания. Бэкенд использует **Microsoft Semantic Kernel** и подключается к **OpenRouter**, чтобы использовать широкий выбор языковых моделей (LLM).

---

### **Тестовое задание**  
**Цель:** Реализовать AI-чатбота с бэкендом на ASP.NET Core с использованием библиотеки Semantic Kernel и бесплатных моделей из OpenRouter. Фронтенд может быть реализован любым способом.  

**Выполнено:**  
- Бэкенд: ASP.NET Core Web API + Semantic Kernel + OpenRouter.  
- Фронтенд: Blazor Server.  
- Контейнеризация: Docker Compose.  
- Паттерны проектирования: DI, CQRS, Repository.  

---

## Архитектура  
Приложение разделено на два основных компонента:

### **`IntelliBot.Core`**  
**ASP.NET Core Web API** выступает в роли «мозга» чат-бота. Получает запросы пользователя, обрабатывает их через Semantic Kernel и генерирует ответ с помощью OpenRouter API.  

### **`IntelliBot.WebApp`**  
**Blazor Server** приложение предоставляет чистый и отзывчивый интерфейс для общения с ботом. Обменивается данными с API `IntelliBot.Core`.  

Вся система управляется через **Docker Compose**, который собирает и запускает оба сервиса в отдельных контейнерах.

---

## Основные технологии  
- **.NET 9**  
- **ASP.NET Core** (бэкенд API)  
- **Blazor Server** (фронтенд UI)  
- **Microsoft Semantic Kernel** (оркестровка ИИ)  
- **OpenRouter** (провайдер LLM)  
- **Docker & Docker Compose** (контейнеризация и оркестрация)  

---

## Начало работы  
Следуйте этим шагам, чтобы запустить IntelliBot локально.

### Предварительные требования  
1. Установите [Docker](https://www.docker.com/products/docker-desktop/).  
2. Получите [OpenRouter API Key](https://openrouter.ai/keys).  

### Конфигурация  
1. Клонируйте репозиторий:  
   ```bash  
   git clone https://github.com/DmitryChukhnenko/IntelliBot.git  
   cd IntelliBot  
   ```  
2. Создайте файл `.env` в корне проекта.  
3. Добавьте переменные окружения:  

   ```env  
   # Ваш OpenRouter API Key  
   OPENROUTER_API_KEY=sk-or-v1-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  

   # ID модели из OpenRouter (например, mistralai/mistral-7b-instruct)  
   OPENROUTER_MODEL_ID=google/gemma-7b-it  

   # Базовый URL OpenRouter API  
   OPENROUTER_BASE_URL=https://openrouter.ai/api/v1  

   # Разрешенные источники для CORS  
   ALLOWED_ORIGINS=http://localhost:5051  
   ```  

### Запуск приложения  
1. В терминале корня проекта выполните:  
   ```bash  
   docker-compose up --build  
   ```  
   Команда соберет образы и запустит контейнеры.  

2. Доступ к сервисам:  
   - **IntelliBot Web App**: [http://localhost:5051](http://localhost:5051)  
   - **Core API (Swagger UI)**: [http://localhost:5050/swagger](http://localhost:5050/swagger)  

---

## Структура проекта  
```
.
├── IntelliBot.Core/      # Бэкенд на ASP.NET Core  
│   ├── Controllers/      # API контроллеры (например, AssistantController)  
│   ├── Services/         # Бизнес-логика (например, OpenrouterAssistantService)  
│   ├── Models/           # Модели данных для запросов/ответов  
│   ├── Program.cs        # Настройка сервисов  
│   └── Dockerfile        # Docker-образ для бэкенда  
│  
├── IntelliBot.WebApp/    # Фронтенд на Blazor Server  
│   ├── Components/       # Компоненты Blazor (страницы, макеты)  
│   ├── Services/         # Сервис для вызова API  
│   ├── wwwroot/          # Статические файлы (CSS, изображения)  
│   └── Dockerfile        # Docker-образ для фронтенда  
│  
├── docker-compose.yml    # Оркестрация контейнеров  
└── IntelliBot.sln         # Решение Visual Studio  
```

---

## API-эндпоинты  
### **`POST /api/cognition`**  
Основной эндпоинт для обработки сообщений пользователя.  
- **Тело запроса:**  
  ```json  
  {  
    "content": "Ваше сообщение боту"  
  }  
  ```  
- **Ответ:**  
  ```json  
  {  
    "reply": "Ответ бота"  
  }  
  ```  

### **`GET /health`**  
Проверяет работоспособность сервиса. Возвращает статус "Healthy", если всё в порядке.

---

## Лицензия  
Проект распространяется под лицензией MIT. Подробности в файле [LICENSE.txt](./LICENSE.txt).  
