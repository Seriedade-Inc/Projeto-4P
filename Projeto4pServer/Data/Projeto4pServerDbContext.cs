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
        public DbSet<CharacterSkills> CharacterSkills { get; set; } // Relacionamento 1:1 com CharacterSkills
        
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

            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.Character)
                .WithMany(c => c.Agendas)
                .HasForeignKey(a => a.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Blasphemy>()
                .HasOne(b => b.Character)
                .WithMany(c => c.Blasphemies)
                .HasForeignKey(b => b.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharacterSkills>()
                .HasKey(cs => cs.Id); // Define a chave primária

            modelBuilder.Entity<CharacterSkills>()
                .HasOne(cs => cs.Character)
                .WithOne(c => c.CharacterSkills) // Relacionamento 1:1 com Character
                .HasForeignKey<CharacterSkills>(cs => cs.CharacterId)
                .OnDelete(DeleteBehavior.Cascade); // Deleção em cascata

        }   
    }
}