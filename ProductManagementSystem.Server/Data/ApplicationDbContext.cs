using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Shared;

namespace ProductManagementSystem.Server.Data
{
    /// <summary>
    /// Контекст базы данных для управления товарами
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Набор товаров в базе данных
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для таблицы Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Price)
                    .HasPrecision(18, 2);

                entity.Property(e => e.Stock)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Индексы для оптимизации запросов
                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("IX_Products_Name");

                entity.HasIndex(e => e.CreatedAt)
                    .HasDatabaseName("IX_Products_CreatedAt");
            });

            // Seed данные по умолчанию
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Ноутбук ASUS VivoBook",
                    Description = "Портативный ноутбук с процессором Intel Core i7",
                    Price = 45000m,
                    Stock = 5,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Монитор Dell 27 дюймов",
                    Description = "4K монитор для работы и развлечений",
                    Price = 25000m,
                    Stock = 10,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "Клавиатура Logitech",
                    Description = "Механическая клавиатура RGB",
                    Price = 3500m,
                    Stock = 20,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 4,
                    Name = "Мышь Razer DeathAdder",
                    Description = "Игровая мышь высокой точности",
                    Price = 2500m,
                    Stock = 15,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
