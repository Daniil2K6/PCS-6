# 🏗️ Архитектура приложения

## Общая архитектура системы

```
┌─────────────────────────────────────────────────────────────┐
│                  INTERNET / WEB BROWSER                     │
│                  (https://localhost:7279)                   │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ HTTPS
                              │
┌─────────────────────────────────────────────────────────────┐
│              BLAZOR WEBASSEMBLY CLIENT                      │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  User Interface (Razor Components)                  │   │
│  │  ├── MainLayout.razor                              │   │
│  │  ├── NavMenu.razor                                 │   │
│  │  ├── Home.razor                                    │   │
│  │  └── Products.razor (CRUD)                         │   │
│  └─────────────────────────────────────────────────────┘   │
│                        │                                    │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Services                                           │   │
│  │  ├── ProductService (HTTP API calls)               │   │
│  │  └── ThemeService (light/dark mode)                │   │
│  └─────────────────────────────────────────────────────┘   │
│                        │                                    │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  HttpClient (JSON communication)                    │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ HTTP/REST API
                              │ (https://localhost:7088)
                              │
┌─────────────────────────────────────────────────────────────┐
│             ASP.NET CORE WEB API SERVER                    │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Controllers                                        │   │
│  │  └── ProductsController                            │   │
│  │      ├── GET /api/products                         │   │
│  │      ├── GET /api/products/{id}                    │   │
│  │      ├── POST /api/products                        │   │
│  │      ├── PUT /api/products/{id}                    │   │
│  │      └── DELETE /api/products/{id}                 │   │
│  └─────────────────────────────────────────────────────┘   │
│                        │                                    │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Business Logic                                     │   │
│  │  ├── Data validation                               │   │
│  │  ├── Error handling                                │   │
│  │  └── Logging                                       │   │
│  └─────────────────────────────────────────────────────┘   │
│                        │                                    │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Entity Framework Core                             │   │
│  │  ├── ApplicationDbContext                          │   │
│  │  ├── DbSet<Product>                                │   │
│  │  └── LINQ to SQL translation                       │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ SQL Commands
                              │
┌─────────────────────────────────────────────────────────────┐
│                   SQL SERVER (LocalDB)                      │
│                                                             │
│  ProductManagementSystemDb                                 │
│  ├── dbo.Products (Table)                                  │
│  │   ├── Id (int, PK)                                     │
│  │   ├── Name (nvarchar(200))                             │
│  │   ├── Description (nvarchar(1000))                     │
│  │   ├── Price (decimal(18,2))                            │
│  │   ├── Stock (int)                                      │
│  │   ├── CreatedAt (datetime)                             │
│  │   └── UpdatedAt (datetime)                             │
│  ├── IX_Products_Name (Index)                              │
│  └── IX_Products_CreatedAt (Index)                         │
└─────────────────────────────────────────────────────────────┘
```

---

## Диаграмма потока данных (CRUD операции)

### 1. CREATE (Добавление товара)

```
┌─────────────────────────────────────────────────────────┐
│ User заполняет форму в UI                               │
└──────────────────────┬──────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────┐
│ ClientSide Validation (валидация на клиенте)            │
│ - Проверка пустых полей                                │
│ - Проверка корректности чисел                           │
│ - Проверка диапазонов значений                          │
└──────────────────────┬──────────────────────────────────┘
                       │ (если валидна)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ ProductService.CreateProductAsync()                     │
│ Сериализация данных в JSON                              │
│ POST https://localhost:7088/api/products                │
└──────────────────────┬──────────────────────────────────┘
                       │ (HTTP POST)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ ProductsController.CreateProduct()                      │
│ - Десериализация JSON в объект Product                  │
│ - Server-side validation                                │
│ - Установка CreatedAt и UpdatedAt                       │
└──────────────────────┬──────────────────────────────────┘
                       │ (если валидна)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ ApplicationDbContext.Products.Add()                      │
│ Entity Framework отслеживает изменения                  │
└──────────────────────┬──────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────┐
│ await dbContext.SaveChangesAsync()                       │
│ Entity Framework генерирует SQL INSERT                   │
│ INSERT INTO Products (...) VALUES (...)                 │
└──────────────────────┬──────────────────────────────────┘
                       │ (SQL Query)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ SQL Server выполняет INSERT                             │
│ Товар сохраняется в БД                                  │
│ Возвращается новый ID                                   │
└──────────────────────┬──────────────────────────────────┘
                       │ (ответ: 201 Created)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ Сервер отправляет JSON с созданным товаром             │
│ {"id": 5, "name": "...", ...}                           │
└──────────────────────┬──────────────────────────────────┘
                       │ (HTTP Response)
                       ▼
┌─────────────────────────────────────────────────────────┐
│ Клиент получает ответ                                   │
│ Добавляет товар в локальный список                      │
│ Обновляет UI (таблица)                                  │
│ Показывает сообщение об успехе                          │
└─────────────────────────────────────────────────────────┘
```

### 2. READ (Получение товаров)

```
Пользователь открывает страницу "Товары"
        │
        ▼
ProductService.GetProductsAsync()
        │
        ▼
GET https://localhost:7088/api/products
        │
        ▼
ProductsController.GetProducts()
        │
        ▼
dbContext.Products.OrderByDescending(p => p.CreatedAt).ToListAsync()
        │
        ▼
SQL Server: SELECT * FROM Products ORDER BY CreatedAt DESC
        │
        ▼
Возвращается JSON массив товаров
        │
        ▼
Клиент парсит JSON и отображает в таблице
```

### 3. UPDATE (Редактирование товара)

```
Пользователь нажимает "Редактировать"
        │
        ▼
Форма загружается с текущими данными товара
        │
        ▼
Пользователь изменяет данные и нажимает "Сохранить"
        │
        ▼
Валидация (как в CREATE)
        │
        ▼
PUT https://localhost:7088/api/products/1
        │
        ▼
ProductsController.UpdateProduct(id, product)
        │
        ▼
dbContext.Products.FindAsync(id)
        │
        ▼
Обновление существующего товара
        │
        ▼
await dbContext.SaveChangesAsync()
        │
        ▼
SQL: UPDATE Products SET ... WHERE Id = 1
        │
        ▼
Ответ с обновленным товаром (200 OK)
        │
        ▼
Таблица обновляется в UI
```

### 4. DELETE (Удаление товара)

```
Пользователь нажимает "Удалить"
        │
        ▼
Модальное окно: "Вы уверены?"
        │
        ▼
Пользователь подтверждает
        │
        ▼
DELETE https://localhost:7088/api/products/1
        │
        ▼
ProductsController.DeleteProduct(id)
        │
        ▼
dbContext.Products.FindAsync(id)
        │
        ▼
dbContext.Products.Remove(product)
        │
        ▼
await dbContext.SaveChangesAsync()
        │
        ▼
SQL: DELETE FROM Products WHERE Id = 1
        │
        ▼
Ответ: 200 OK
        │
        ▼
Товар удаляется из UI таблицы
```

---

## Архитектура Blazor компонента

```
┌──────────────────────────────────────────────┐
│ Products.razor                               │
│                                              │
│ @page "/products"                            │
│ @inject ProductService                       │
│ @inject ThemeService                         │
│                                              │
│ ┌────────────────────────────────────────┐  │
│ │ Markup (HTML)                          │  │
│ │ ├── Header (Название, кнопки)         │  │
│ │ ├── Products Table                      │  │
│ │ ├── Modal форма для добавления         │  │
│ │ ├── Modal форма для редактирования     │  │
│ │ └── Modal подтверждения удаления      │  │
│ └────────────────────────────────────────┘  │
│                                              │
│ ┌────────────────────────────────────────┐  │
│ │ @code Block (Logic)                     │  │
│ │                                          │  │
│ │ Fields:                                   │  │
│ │ - products: List<Product>               │  │
│ │ - isLoading, isSaving, isDeleting       │  │
│ │ - showFormModal, showDeleteConfirm      │  │
│ │ - editingProduct, deletingProduct       │  │
│ │                                          │  │
│ │ Lifecycle Methods:                       │  │
│ │ - OnInitializedAsync()                  │  │
│ │                                          │  │
│ │ Methods:                                  │  │
│ │ - LoadProducts()                         │  │
│ │ - ShowAddForm(), ShowEditForm()          │  │
│ │ - SaveProduct()                          │  │
│ │ - ConfirmDelete()                        │  │
│ │ - ToggleTheme()                          │  │
│ └────────────────────────────────────────┘  │
└──────────────────────────────────────────────┘
```

---

## Архитектура контроллера

```
┌──────────────────────────────────────────────┐
│ ProductsController : ControllerBase          │
│                                              │
│ [ApiController]                              │
│ [Route("api/[controller]")]                  │
│                                              │
│ Constructor:                                  │
│ - ApplicationDbContext _context              │
│ - ILogger<ProductsController> _logger        │
│                                              │
│ ┌────────────────────────────────────────┐  │
│ │ Methods:                                │  │
│ │                                          │  │
│ │ [HttpGet]                                │  │
│ │ GetProducts() -> Task<List<Product>>    │  │
│ │                                          │  │
│ │ [HttpGet("{id}")]                        │  │
│ │ GetProduct(id) -> Task<Product>         │  │
│ │                                          │  │
│ │ [HttpPost]                               │  │
│ │ CreateProduct(product) -> Task<Product> │  │
│ │                                          │  │
│ │ [HttpPut("{id}")]                        │  │
│ │ UpdateProduct(id, product) ->           │  │
│ │   Task<Product>                         │  │
│ │                                          │  │
│ │ [HttpDelete("{id}")]                     │  │
│ │ DeleteProduct(id) -> Task<IActionResult>│  │
│ └────────────────────────────────────────┘  │
│                                              │
│ Каждый метод:                                │
│ 1. Валидирует входные данные                │
│ 2. Выполняет операцию с БД                  │
│ 3. Обрабатывает ошибки                      │
│ 4. Логирует операцию                        │
│ 5. Возвращает результат                     │
└──────────────────────────────────────────────┘
```

---

## Entity Framework Core паттерн

```
┌──────────────────────────────────────────────┐
│ ApplicationDbContext : DbContext             │
│                                              │
│ Constructor:                                  │
│ ApplicationDbContext(options)                │
│                                              │
│ DbSet<Product> Products { get; set; }       │
│                                              │
│ protected override void OnModelCreating()    │
│ {                                            │
│   // Конфигурация таблицы                   │
│   modelBuilder.Entity<Product>(entity =>     │
│   {                                          │
│     entity.HasKey(e => e.Id);               │
│     entity.Property(e => e.Name).IsRequired │
│       .HasMaxLength(200);                    │
│     entity.Property(e => e.Price)           │
│       .HasPrecision(18, 2);                  │
│     entity.HasIndex(e => e.Name);           │
│     // Seed data                             │
│   });                                        │
│ }                                            │
└──────────────────────────────────────────────┘
```

---

## Dependency Injection контейнер

```
Program.cs (Сервер):
├── builder.Services.AddDbContext<ApplicationDbContext>()
├── builder.Services.AddControllers()
└── builder.Services.AddCors()

Program.cs (Клиент):
├── builder.Services.AddScoped<HttpClient>()
├── builder.Services.AddScoped<ThemeService>()
└── builder.Services.AddScoped<ProductService>()

Внедрение в компоненты:
├── @inject ProductService ProductService
├── @inject ThemeService ThemeService
└── @inject ILogger<Products> Logger
```

---

## HTTP запрос/ответ цикл

### Пример: Получение всех товаров

#### Request:
```http
GET /api/products HTTP/1.1
Host: localhost:7088
User-Agent: Blazor/9.0
Accept: application/json
```

#### Response:
```http
HTTP/1.1 200 OK
Content-Type: application/json
Date: Mon, 19 May 2026 12:00:00 GMT

[
  {
    "id": 1,
    "name": "Ноутбук ASUS VivoBook",
    "description": "Портативный ноутбук",
    "price": 45000.00,
    "stock": 5,
    "createdAt": "2026-05-19T12:00:00Z",
    "updatedAt": "2026-05-19T12:00:00Z"
  },
  ...
]
```

---

## Безопасность архитектуры

```
┌────────────────────────────────────────┐
│ Браузер пользователя (HTTP/2)           │
│ + HTTPS шифрование                      │
│ + CORS проверки                         │
└────────────────────────────────────────┘
                   │
                   ▼
┌────────────────────────────────────────┐
│ ASP.NET Core сервер                    │
│ + Валидация всех входных данных        │
│ + Проверка типов (C#)                   │
│ + Обработка исключений                  │
│ + Логирование                           │
└────────────────────────────────────────┘
                   │
                   ▼
┌────────────────────────────────────────┐
│ Entity Framework Core                  │
│ + Параметризованные запросы            │
│ + Защита от SQL injection              │
│ + Контроль доступа                     │
└────────────────────────────────────────┘
                   │
                   ▼
┌────────────────────────────────────────┐
│ SQL Server База данных                 │
│ + Индексы для оптимизации              │
│ + Транзакции для целостности           │
│ + Резервное копирование                │
└────────────────────────────────────────┘
```

---

## Масштабируемость

```
Текущая архитектура позволяет:

┌─────────────────────┐
│ Масштабирование     │
├─────────────────────┤
│ Горизонтальное      │
│ - Несколько API     │
│   серверов          │
│ - Load Balancer     │
│                     │
│ Вертикальное        │
│ - Больше памяти     │
│ - Мощнее сервер     │
│                     │
│ БД масштабирование  │
│ - Replikация        │
│ - Sharding          │
│ - SQL Server HA     │
└─────────────────────┘
```

---

Эта архитектура демонстрирует:
✅ Чистое разделение concerns'ов
✅ Асинхронное программирование
✅ Современные паттерны проектирования
✅ Масштабируемость и безопасность
✅ Best practices .NET разработки
