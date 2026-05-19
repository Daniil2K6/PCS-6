#!/bin/bash

# Script для тестирования API Product Management System
# Убедитесь что сервер запущен на https://localhost:7088

API_URL="https://localhost:7088/api/products"

echo "========================================="
echo "Testing Product Management System API"
echo "========================================="

# Игнорировать SSL ошибки для локального тестирования (добавить -k флаг)
CURL_OPTS="-k -s"

# 1. GET - Получить все товары
echo ""
echo "1️⃣ Получение списка всех товаров..."
echo "GET $API_URL"
curl $CURL_OPTS -X GET "$API_URL" | python3 -m json.tool

# 2. GET - Получить товар по ID
echo ""
echo ""
echo "2️⃣ Получение товара с ID 1..."
echo "GET $API_URL/1"
curl $CURL_OPTS -X GET "$API_URL/1" | python3 -m json.tool

# 3. POST - Добавить новый товар
echo ""
echo ""
echo "3️⃣ Добавление нового товара..."
echo "POST $API_URL"
NEW_PRODUCT_RESPONSE=$(curl $CURL_OPTS -X POST "$API_URL" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Тестовый товар",
    "description": "Описание тестового товара",
    "price": 9999.99,
    "stock": 50
  }')

echo "$NEW_PRODUCT_RESPONSE" | python3 -m json.tool

# Извлечь ID из ответа (если нужно)
NEW_PRODUCT_ID=$(echo "$NEW_PRODUCT_RESPONSE" | grep -o '"id":[0-9]*' | head -1 | cut -d: -f2)
echo "Создан товар с ID: $NEW_PRODUCT_ID"

# 4. PUT - Обновить товар
echo ""
echo ""
echo "4️⃣ Обновление товара..."
echo "PUT $API_URL/1"
curl $CURL_OPTS -X PUT "$API_URL/1" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Ноутбук ASUS (Обновлено)",
    "description": "Обновленное описание",
    "price": 50000.00,
    "stock": 3
  }' | python3 -m json.tool

# 5. DELETE - Удалить товар (если он создавался)
if [ ! -z "$NEW_PRODUCT_ID" ]; then
  echo ""
  echo ""
  echo "5️⃣ Удаление товара..."
  echo "DELETE $API_URL/$NEW_PRODUCT_ID"
  curl $CURL_OPTS -X DELETE "$API_URL/$NEW_PRODUCT_ID"
  echo ""
  echo "Товар удален"
fi

# 6. Финальная проверка - получить все товары
echo ""
echo ""
echo "6️⃣ Финальная проверка - все товары..."
echo "GET $API_URL"
curl $CURL_OPTS -X GET "$API_URL" | python3 -m json.tool

echo ""
echo ""
echo "========================================="
echo "✅ Тестирование завершено!"
echo "========================================="
