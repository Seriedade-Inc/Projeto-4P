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
        public DbSet<AgendaAbilities> AgendaAbilities { get; set; }
        public DbSet<Blasphemy> Blasphemies { get; set; }
        public DbSet<BlasphemyAbilities> BlasphemyAbilities { get; set; }   
        public DbSet<CharAgenda> CharAgendas { get; set; }
        public DbSet<CharBlasphemy> CharBlasphemies { get; set; }
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

            // Configuração da entidade Agenda
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasKey(a => a.Id); // Define a chave primária
                entity.Property(a => a.AgendaName).IsRequired(); // Exemplo de configuração
            });

            // Configuração da entidade AgendaAbilities
            modelBuilder.Entity<AgendaAbilities>()
                .HasKey(aa => aa.Id); // Define a chave primária

            // Configuração da entidade Blasphemy
            modelBuilder.Entity<Blasphemy>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Blasphemy>()
                .HasMany(b => b.BlasphemyAbilities)
                .WithOne(a => a.Blasphemy)
                .HasForeignKey(ba => ba.BlasphemyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da entidade BlasphemyAbilities
            modelBuilder.Entity<BlasphemyAbilities>()
                .HasKey(ba => ba.Id);

            // Configuração da entidade CharAgenda
            modelBuilder.Entity<CharAgenda>()
                .HasKey(ca => ca.Id);

            modelBuilder.Entity<CharAgenda>()
                .HasOne(ca => ca.Character)
                .WithMany(c => c.CharAgendas) // Relacionamento com a lista de CharAgendas em Character
                .HasForeignKey(ca => ca.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharAgenda>()
                .HasOne(ca => ca.AgendaAbility)
                .WithMany() // AgendaAbilities não precisa de uma lista de CharAgendas
                .HasForeignKey(ca => ca.AgendaAbilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharAgenda>()
                .HasOne(ca => ca.Agenda)
                .WithMany()
                .HasForeignKey(ca => ca.AgendaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da entidade CharBlasphemy
            modelBuilder.Entity<CharBlasphemy>()
                .HasKey(cb => cb.Id);

            modelBuilder.Entity<CharBlasphemy>()
                .HasOne(cb => cb.Character)
                .WithMany(c => c.CharBlasphemies) // Relacionamento com a lista de CharBlasphemies em Character
                .HasForeignKey(cb => cb.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharBlasphemy>()
                .HasOne(cb => cb.BlasphemyAbility)
                .WithMany()
                .HasForeignKey(cb => cb.BlasphemyAbilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharBlasphemy>()
                .HasOne(cb => cb.Blasphemy)
                .WithMany()
                .HasForeignKey(cb => cb.BlasphemyId)
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