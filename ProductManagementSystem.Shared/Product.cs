namespace ProductManagementSystem.Shared
{
    /// <summary>
    /// Модель продукта для системы управления товарами
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Уникальный идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание товара
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество товара на складе
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
