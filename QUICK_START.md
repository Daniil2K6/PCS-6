# 🚀 Быстрый старт - Запуск проекта

## ⚙️ Требования

- .NET 9.0 SDK или выше
- SQL Server / LocalDB (устанавливается с Visual Studio)
- Браузер (Chrome, Firefox, Edge, Safari)

## 📋 Проверка установки

```bash
# Проверить версию .NET
dotnet --version

# Должно вывести: 9.0.305 или выше
```

## 🎯 Шаги запуска

### 1️⃣ Клонировать и перейти в папку

```bash
cd "/Users/daniilka/Documents/dev/мусор/Программирование Корп Систем/PCS-6"
```

### 2️⃣ Восстановить зависимости (опционально)

```bash
dotnet restore
```

### 3️⃣ Запустить сервер (Terminal 1)

```bash
cd ProductManagementSystem.Server
dotnet run
```

Вывод должен показать:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
    Now listening on: https://localhost:7088
```

### 4️⃣ Запустить клиент (Terminal 2)

```bash
cd ProductManagementSystem.Client
dotnet watch run
```

Вывод должен показать:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
    Now listening on: https://localhost:7279
```

### 5️⃣ Открыть в браузере

Перейти на: **https://localhost:7279**

## ✅ Проверка что работает

1. ✓ Загруженась главная страница
2. ✓ Нажать "Перейти к управлению товарами"
3. ✓ Видны 4 товара в таблице
4. ✓ Нажать "➕ Добавить товар" - открывается форма
5. ✓ Нажать "🌙 Темная тема" - переключается тема

## 🧪 Тестирование API (опционально)

```bash
# Получить все товары
curl https://localhost:7088/api/products

# Получить товар с ID 1
curl https://localhost:7088/api/products/1

# Добавить товар
curl -X POST https://localhost:7088/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Новый товар","description":"Описание","price":5000,"stock":10}'

# Обновить товар
curl -X PUT https://localhost:7088/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Обновленный товар","description":"Новое описание","price":6000,"stock":15}'

# Удалить товар
curl -X DELETE https://localhost:7088/api/products/1
```

## 📂 Структура файлов

```
PCS-6/
├── ProductManagementSystem.Shared/         # Общие модели
│   └── Product.cs                          # Модель товара
├── ProductManagementSystem.Server/         # REST API сервер
│   ├── Controllers/ProductsController.cs   # API контроллер
│   ├── Data/ApplicationDbContext.cs        # Entity Framework контекст
│   └── Program.cs                          # Конфигурация сервера
├── ProductManagementSystem.Client/         # Blazor WebAssembly клиент
│   ├── Pages/Products.razor                # Страница товаров
│   ├── Services/
│   │   ├── ProductService.cs               # Сервис для API вызовов
│   │   └── ThemeService.cs                 # Сервис темы
│   ├── Layout/MainLayout.razor             # Layout приложения
│   └── Program.cs                          # Конфигурация клиента
├── README.md                               # Полная документация
├── DEFENSE_GUIDE.md                        # Руководство для защиты
└── QUICK_START.md                          # Этот файл
```

## 🐛 Решение проблем

### Проблема: "Connection refused"
**Решение:** Убедитесь что оба терминала запущены и приложения слушают на правильных портах.

### Проблема: "Cannot connect to database"
**Решение:** Убедитесь что SQL Server / LocalDB установлены и запущены. Можно использовать:
```bash
# Список установленных SQL Server экземпляров
SqlLocalDB.exe info
```

### Проблема: HTTPS certificate warning
**Решение:** Это нормально для локальной разработки. Нажмите "Advanced" и "Continue anyway"

### Проблема: Port is already in use
**Решение:** Измените порты в appsettings.json или завершите процесс слушающий на порту

## 🔧 Полезные команды

```bash
# Очистить build артефакты
dotnet clean

# Пересобрать проект
dotnet build

# Запустить без watch режима
dotnet run

# Проверить ошибки компиляции
dotnet build --no-restore

# Откатить БД (если нужно)
dotnet ef database drop

# Применить миграции
dotnet ef database update
```

## 📚 Дополнительная информация

- [README.md](README.md) - Полная документация проекта
- [DEFENSE_GUIDE.md](DEFENSE_GUIDE.md) - Подготовка к защите
- [Документация Blazor](https://learn.microsoft.com/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/ef/)

## 🎓 Для защиты проекта

1. Убедитесь что проект работает по этому гайду
2. Прочитайте DEFENSE_GUIDE.md для подготовки
3. Потренируйтесь демонстрировать функции
4. Подготовьте ответы на вопросы

## ❓ Вопросы?

Если что-то не работает:
1. Проверьте требования выше
2. Посмотрите логи в терминале
3. Убедитесь что используется .NET 9.0
4. Попробуйте `dotnet clean && dotnet build`

---

**Удачи! 🚀**
