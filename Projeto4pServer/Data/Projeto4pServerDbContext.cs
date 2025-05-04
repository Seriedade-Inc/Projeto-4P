using Microsoft.EntityFrameworkCore;
using Projeto4pSharedLibrary.Classes;

namespace Projeto4pServer.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> User { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Blasphemy> Blasphemies { get; set; }
        public DbSet<BlasphemyAbilities> BlasphemyAbilities { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do relacionamento entre Character e Inventory
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Character)
                .WithMany(c => c.Inventories)
                .HasForeignKey(i => i.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Characters) // Um User pode ter muitos Characters
                .WithOne() // Um Character não precisa de uma propriedade de navegação para o User
                .HasForeignKey(c => c.UserId) // Chave estrangeira em Character
                .OnDelete(DeleteBehavior.Cascade); // Deleção em cascata

            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasKey(a => a.Id); // Define a chave primária
                entity.Property(a => a.AgendaName).IsRequired(); // Exemplo de configuração
            });

            modelBuilder.Entity<Blasphemy>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Blasphemy>()
                .HasMany(b => b.BlasphemyAbilities)
                .WithOne()
                .HasForeignKey(ba => ba.BlasphemyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da entidade BlasphemyAbilities
            modelBuilder.Entity<BlasphemyAbilities>()
                .HasKey(ba => ba.Id);

        }   
    }
}