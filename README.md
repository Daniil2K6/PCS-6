# Система управления товарами интернет-магазина на Blazor WebAssembly

## Описание проекта

Система управления товарами интернет-магазина, разработанная на платформе Blazor WebAssembly с использованием ASP.NET Core и Entity Framework Core. Приложение позволяет выполнять полный CRUD (Create, Read, Update, Delete) операции с товарами через веб-интерфейс.

## Структура проекта

```
PCS-6/
├── ProductManagementSystem.Shared/          # Проект общих моделей и типов
│   └── Product.cs                           # Модель товара
├── ProductManagementSystem.Server/          # ASP.NET Core API сервер
│   ├── Controllers/
│   │   └── ProductsController.cs            # REST API контроллер для товаров
│   ├── Data/
│   │   └── ApplicationDbContext.cs          # Entity Framework Core контекст БД
│   └── Program.cs                           # Конфигурация приложения
├── ProductManagementSystem.Client/          # Blazor WebAssembly клиент
│   ├── Pages/
│   │   ├── Home.razor                       # Главная страница
│   │   └── Products.razor                   # Страница управления товарами
│   ├── Layout/
│   │   ├── MainLayout.razor                 # Главный layout
│   │   └── NavMenu.razor                    # Навигация
│   ├── Services/
│   │   ├── ProductService.cs                # Сервис для API вызовов
│   │   └── ThemeService.cs                  # Сервис управления темой
│   └── Program.cs                           # Конфигурация клиента
└── ProductManagementSystem.sln              # Solution файл
```

## Технологический стек

### Серверная часть
- **ASP.NET Core** - Фреймворк для создания веб-приложений
- **Entity Framework Core** - ORM для работы с базой данных
- **SQL Server** - Система управления базами данных
- **RESTful API** - Архитектура передачи данных

### Клиентская часть
- **Blazor WebAssembly** - Фреймворк для создания интерактивных веб-приложений на C#
- **Bootstrap 5** - CSS фреймворк для адаптивного дизайна
- **C# / Razor** - Языки программирования

### Инструменты разработки
- **.NET 9.0** - Платформа разработки
- **Visual Studio Code** - Редактор кода
- **Git** - Система контроля версий

## Функциональность

### API Endpoints

| Метод | URL | Описание |
|-------|-----|---------|
| GET | `/api/products` | Получить список всех товаров |
| GET | `/api/products/{id}` | Получить товар по ID |
| POST | `/api/products` | Создать новый товар |
| PUT | `/api/products/{id}` | Обновить товар |
| DELETE | `/api/products/{id}` | Удалить товар |

### Пользовательский интерфейс

1. **Главная страница** - Добро пожаловать и описание приложения
2. **Страница товаров** - Таблица со всеми товарами
3. **Добавление товара** - Модальное окно для создания нового товара
4. **Редактирование товара** - Модальное окно для изменения товара
5. **Удаление товара** - С запросом подтверждения
6. **Переключение темы** - Светлая и темная тема оформления

## Требования к проекту (выполненные)

✅ **BlazorClient (WebAssembly)** - Полнофункциональный клиент на Blazor WebAssembly  
✅ **BlazorServer (ASP.NET Core Host)** - API сервер на ASP.NET Core  
✅ **GET /api/products** - Возвращает список товаров  
✅ **POST /api/products** - Добавляет новый товар  
✅ **DELETE /api/products** - Удаляет указанный товар  
✅ **PUT /api/products** - Изменяет указанный товар  
✅ **Страницу отображения товаров** - Табличное представление товаров  
✅ **Форму добавления нового товара** - Модальное окно с валидацией  
✅ **Форму изменения товара** - Модальное окно редактирования  
✅ **Возможность удаления товара с запросом подтверждения** - Реализовано  
✅ **Хранение данных в БД с Entity Framework Core** - SQL Server, EF Core 9.0  
✅ **Реализация тем оформления (светлая/темная)** - Переключатель реализован  

## Запуск приложения

### Требования
- .NET 9.0 SDK
- SQL Server или LocalDB
- Visual Studio Code или другой редактор кода

### Шаги запуска

1. **Клонировать репозиторий**
```bash
git clone https://github.com/Daniil2K6/PCS-6.git
cd PCS-6
```

2. **Восстановить зависимости**
```bash
dotnet restore
```

3. **Запустить сервер (в отдельном терминале)**
```bash
cd ProductManagementSystem.Server
dotnet run
```
Сервер запустится на `https://localhost:7088`

4. **Запустить клиент (в отдельном терминале)**
```bash
cd ProductManagementSystem.Client
dotnet watch run
```
Клиент запустится на `https://localhost:7279`

5. **Открыть в браузере**
Перейти на `https://localhost:7279`

## Архитектура приложения

### Архитектурный паттерн: Three-Tier Architecture

```
┌─────────────────────────────────────────┐
│     Blazor WebAssembly Client           │
│  (UI Components, Pages, Services)       │
└──────────────┬──────────────────────────┘
               │ HTTP Requests/Responses
┌──────────────▼──────────────────────────┐
│    ASP.NET Core Web API Server          │
│  (Controllers, Business Logic)          │
└──────────────┬──────────────────────────┘
               │ SQL Queries
┌──────────────▼──────────────────────────┐
│   SQL Server Database                   │
│  (Data Persistence)                     │
└─────────────────────────────────────────┘
```

### Поток данных

1. **Клиент** отправляет HTTP запрос к API
2. **Сервер** получает запрос, обрабатывает его в контроллере
3. **Контроллер** использует Entity Framework для работы с БД
4. **БД** возвращает данные
5. **Сервер** отправляет ответ в формате JSON
6. **Клиент** получает данные и обновляет UI

## Классы и их ответственность

### ProductManagementSystem.Shared
- **Product** - Модель данных товара (Name, Description, Price, Stock, CreatedAt, UpdatedAt)

### ProductManagementSystem.Server
- **ApplicationDbContext** - Entity Framework контекст, управление БД
- **ProductsController** - REST API контроллер с методами CRUD
- **Program.cs** - Конфигурация сервиса, CORS, Entity Framework

### ProductManagementSystem.Client
- **ProductService** - Сервис для HTTP вызовов API
- **ThemeService** - Сервис для управления темой приложения
- **Products.razor** - Компонент для управления товарами
- **Home.razor** - Главная страница приложения

## Entity Framework Core - Конфигурация

```csharp
// Параметры columns
- Name: string (MaxLength 200, Required)
- Description: string (MaxLength 1000)
- Price: decimal (Precision 18,2)
- Stock: int (Required)
- CreatedAt: DateTime (Default: GETUTCDATE())
- UpdatedAt: DateTime (Default: GETUTCDATE())

// Индексы для оптимизации
- IX_Products_Name
- IX_Products_CreatedAt
```

## Обработка ошибок

1. **Валидация на клиенте** - Проверка данных перед отправкой
2. **Валидация на сервере** - Проверка данных в контроллере
3. **Обработка исключений** - Try-catch блоки с логированием
4. **Сообщения об ошибках** - Информативные сообщения пользователю

## Безопасность

1. **CORS (Cross-Origin Resource Sharing)** - Конфигурация для безопасных запросов
2. **HTTPS** - Использование защищенного протокола передачи
3. **Валидация входных данных** - Проверка всех входящих данных
4. **Логирование** - Логирование всех операций для отладки

## Тестирование

### Тестирование API с Postman/cURL

```bash
# Получить все товары
curl -X GET https://localhost:7088/api/products

# Добавить товар
curl -X POST https://localhost:7088/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Товар","description":"Описание","price":1000,"stock":5}'

# Обновить товар
curl -X PUT https://localhost:7088/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Новое имя","description":"Новое описание","price":2000,"stock":10}'

# Удалить товар
curl -X DELETE https://localhost:7088/api/products/1
```

## Возможные улучшения

1. Добавить аутентификацию и авторизацию
2. Реализовать пагинацию для больших количеств товаров
3. Добавить поиск и фильтрацию товаров
4. Реализовать категории товаров
5. Добавить фотографии товаров
6. Кэширование данных на клиенте
7. Оптимистичные обновления UI

## Автор

Создано в рамках практического задания по дисциплине "Программирование корпоративных систем"

## Лицензия

MIT

---

**Технологии**: C# | .NET 9.0 | Blazor WebAssembly | ASP.NET Core | Entity Framework Core | SQL Server | Bootstrap 5
