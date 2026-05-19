# Примеры API команд для тестирования

## Базовые curl команды

### 1️⃣ Получить все товары
```bash
curl -k https://localhost:7088/api/products
```

### 2️⃣ Получить товар по ID
```bash
curl -k https://localhost:7088/api/products/1
```

### 3️⃣ Добавить новый товар
```bash
curl -k -X POST https://localhost:7088/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Новый товар",
    "description": "Описание товара",
    "price": 5000.00,
    "stock": 20
  }'
```

### 4️⃣ Обновить товар
```bash
curl -k -X PUT https://localhost:7088/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Обновленное имя",
    "description": "Обновленное описание",
    "price": 6000.00,
    "stock": 25
  }'
```

### 5️⃣ Удалить товар
```bash
curl -k -X DELETE https://localhost:7088/api/products/1
```

---

## PowerShell примеры (для Windows)

### Получить все товары
```powershell
Invoke-WebRequest -Uri "https://localhost:7088/api/products" `
  -Method GET `
  -SkipCertificateCheck | ConvertTo-Json
```

### Добавить товар
```powershell
$body = @{
    name = "Новый товар"
    description = "Описание"
    price = 5000.00
    stock = 20
} | ConvertTo-Json

Invoke-WebRequest -Uri "https://localhost:7088/api/products" `
  -Method POST `
  -Headers @{"Content-Type"="application/json"} `
  -Body $body `
  -SkipCertificateCheck | ConvertTo-Json
```

---

## JavaScript/Fetch примеры

### Получить все товары
```javascript
fetch('https://localhost:7088/api/products')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### Добавить товар
```javascript
const newProduct = {
  name: "Новый товар",
  description: "Описание товара",
  price: 5000.00,
  stock: 20
};

fetch('https://localhost:7088/api/products', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(newProduct)
})
  .then(response => response.json())
  .then(data => console.log('Created:', data))
  .catch(error => console.error('Error:', error));
```

### Обновить товар
```javascript
const updatedProduct = {
  id: 1,
  name: "Обновленное имя",
  description: "Обновленное описание",
  price: 6000.00,
  stock: 25
};

fetch('https://localhost:7088/api/products/1', {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(updatedProduct)
})
  .then(response => response.json())
  .then(data => console.log('Updated:', data))
  .catch(error => console.error('Error:', error));
```

### Удалить товар
```javascript
fetch('https://localhost:7088/api/products/1', {
  method: 'DELETE'
})
  .then(response => response.json())
  .then(data => console.log('Deleted:', data))
  .catch(error => console.error('Error:', error));
```

---

## C# примеры (HttpClient)

### Получить все товары
```csharp
using var client = new HttpClient();
var response = await client.GetAsync("https://localhost:7088/api/products");
var json = await response.Content.ReadAsStringAsync();
Console.WriteLine(json);
```

### Добавить товар
```csharp
var product = new
{
    name = "Новый товар",
    description = "Описание",
    price = 5000.00,
    stock = 20
};

var json = JsonSerializer.Serialize(product);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
    "https://localhost:7088/api/products", 
    content
);
```

---

## Postman

1. **Импортировать коллекцию:**
   - Откройте Postman
   - Нажмите "Import"
   - Выберите файл `postman_collection.json`

2. **Или создать вручную:**
   - New → HTTP Request
   - Выберите метод (GET, POST, PUT, DELETE)
   - Введите URL: `https://localhost:7088/api/products`
   - Для POST/PUT добавьте Body в формате JSON

---

## Примеры JSON для запросов

### Новый товар
```json
{
  "name": "Новый товар",
  "description": "Описание товара",
  "price": 5000.00,
  "stock": 20
}
```

### Обновление товара
```json
{
  "id": 1,
  "name": "Обновленный товар",
  "description": "Новое описание",
  "price": 6000.00,
  "stock": 25
}
```

---

## Статус коды

| Код | Значение |
|-----|----------|
| 200 | OK - Успешно |
| 201 | Created - Создано |
| 400 | Bad Request - Неверный запрос |
| 404 | Not Found - Не найдено |
| 500 | Internal Server Error - Ошибка сервера |

---

## Примеры ошибок

### Название товара пусто
```bash
curl -k -X POST https://localhost:7088/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"","description":"test","price":100,"stock":5}'
```
**Ответ:** 400 Bad Request - "Название товара обязательно"

### Товар не найден
```bash
curl -k https://localhost:7088/api/products/999
```
**Ответ:** 404 Not Found - "Товар не найден"

### Отрицательная цена
```bash
curl -k -X POST https://localhost:7088/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"test","description":"test","price":-100,"stock":5}'
```
**Ответ:** 400 Bad Request - "Цена не может быть отрицательной"

---

## Полезные флаги curl

- `-k` - Игнорировать SSL сертификат (для локального развития)
- `-v` - Verbose режим (показать все детали)
- `-H` - Добавить header
- `-d` - Отправить data
- `-X` - Указать HTTP метод
- `-w "\n"` - Добавить новую строку в конец

---

## Запуск всех тестов

Используйте скрипт `test_api.sh` (требует bash и curl):
```bash
chmod +x test_api.sh
./test_api.sh
```
