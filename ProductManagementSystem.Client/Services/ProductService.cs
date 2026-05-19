using System.Text;
using System.Text.Json;
using ProductManagementSystem.Shared;

namespace ProductManagementSystem.Client.Services
{
    /// <summary>
    /// Сервис для взаимодействия с API товаров
    /// </summary>
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private const string ApiEndpoint = "/api/products";

        public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Получить все товары
        /// </summary>
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                _logger.LogInformation("Загрузка списка товаров");
                var response = await _httpClient.GetAsync(ApiEndpoint);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<Product>>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Product>();
                
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке товаров");
                throw;
            }
        }

        /// <summary>
        /// Получить товар по ID
        /// </summary>
        public async Task<Product?> GetProductAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Загрузка товара с ID {id}");
                var response = await _httpClient.GetAsync($"{ApiEndpoint}/{id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Товар с ID {id} не найден");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<Product>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при загрузке товара с ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                _logger.LogInformation($"Добавление товара: {product.Name}");
                
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(ApiEndpoint, content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                var createdProduct = JsonSerializer.Deserialize<Product>(responseJson, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? throw new InvalidOperationException("Не удалось десериализировать ответ");
                
                _logger.LogInformation($"Товар успешно добавлен с ID {createdProduct.Id}");
                return createdProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении товара");
                throw;
            }
        }

        /// <summary>
        /// Обновить товар
        /// </summary>
        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            try
            {
                _logger.LogInformation($"Обновление товара с ID {id}");
                
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{ApiEndpoint}/{id}", content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                var updatedProduct = JsonSerializer.Deserialize<Product>(responseJson, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? throw new InvalidOperationException("Не удалось десериализировать ответ");
                
                _logger.LogInformation($"Товар с ID {id} успешно обновлен");
                return updatedProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении товара с ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление товара с ID {id}");
                
                var response = await _httpClient.DeleteAsync($"{ApiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();
                
                _logger.LogInformation($"Товар с ID {id} успешно удален");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении товара с ID {id}");
                throw;
            }
        }
    }
}
