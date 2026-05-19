using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Server.Data;
using ProductManagementSystem.Shared;

namespace ProductManagementSystem.Server.Controllers
{
    /// <summary>
    /// API контроллер для управления товарами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех товаров
        /// </summary>
        /// <returns>Список товаров</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Получение списка товаров");
                var products = await _context.Products
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка товаров");
                return StatusCode(500, new { message = "Ошибка при получении товаров" });
            }
        }

        /// <summary>
        /// Получить товар по ID
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <returns>Товар</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Получение товара с ID {id}");
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Товар с ID {id} не найден");
                    return NotFound(new { message = "Товар не найден" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении товара с ID {id}");
                return StatusCode(500, new { message = "Ошибка при получении товара" });
            }
        }

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="product">Данные товара</param>
        /// <returns>Созданный товар</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogWarning("Попытка добавления товара с null значением");
                    return BadRequest(new { message = "Данные товара не могут быть пустыми" });
                }

                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    return BadRequest(new { message = "Название товара обязательно" });
                }

                if (product.Price < 0)
                {
                    return BadRequest(new { message = "Цена не может быть отрицательной" });
                }

                product.Id = 0;
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Товар успешно добавлен с ID {product.Id}");
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении товара");
                return StatusCode(500, new { message = "Ошибка при добавлении товара" });
            }
        }

        /// <summary>
        /// Обновить существующий товар
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <param name="product">Обновленные данные товара</param>
        /// <returns>Обновленный товар</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (id <= 0 || product == null)
                {
                    _logger.LogWarning($"Попытка обновления товара с неверными параметрами: ID={id}");
                    return BadRequest(new { message = "Неверные параметры" });
                }

                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Товар с ID {id} не найден для обновления");
                    return NotFound(new { message = "Товар не найден" });
                }

                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    return BadRequest(new { message = "Название товара обязательно" });
                }

                if (product.Price < 0)
                {
                    return BadRequest(new { message = "Цена не может быть отрицательной" });
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Товар с ID {id} успешно обновлен");
                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении товара с ID {id}");
                return StatusCode(500, new { message = "Ошибка при обновлении товара" });
            }
        }

        /// <summary>
        /// Удалить товар по ID
        /// </summary>
        /// <param name="id">ID товара</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Попытка удаления товара с неверным ID: {id}");
                    return BadRequest(new { message = "Неверный ID товара" });
                }

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Товар с ID {id} не найден для удаления");
                    return NotFound(new { message = "Товар не найден" });
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Товар с ID {id} успешно удален");
                return Ok(new { message = "Товар успешно удален" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении товара с ID {id}");
                return StatusCode(500, new { message = "Ошибка при удалении товара" });
            }
        }
    }
}
