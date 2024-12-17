using Microsoft.EntityFrameworkCore;
using RecipeLibraryEF.Models.Entities;

namespace RecipeLibraryEF.DataAccess;

public class RecipeContext: DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }

    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId);

        modelBuilder.Entity<UserFavorite>()
            .HasKey(uf => new { uf.UserId, uf.RecipeId });

        modelBuilder.Entity<UserFavorite>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UserFavorites)
            .HasForeignKey(uf => uf.UserId);

        modelBuilder.Entity<UserFavorite>()
            .HasOne(uf => uf.Recipe)
            .WithMany(r => r.UserFavorites)
            .HasForeignKey(uf => uf.RecipeId);

        modelBuilder.Entity<Ingredient>()
        .HasIndex(i => i.Name)
        .IsUnique();
    }
}
