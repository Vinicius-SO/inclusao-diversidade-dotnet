using Microsoft.EntityFrameworkCore;
using InclusaoDiversidadeEmpresas.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace InclusaoDiversidadeEmpresas.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<TreinamentoModel> Treinamentos { get; set; }
        public DbSet<ParticipacaoEmTreinamentoModel> ParticipacoesEmTreinamento { get; set; }
        public DbSet<RelatorioDeDiversidadeModel> RelatoriosDeDiversidade { get; set; }

        //CORREÇÃO OBRIGATÓRIA PARA ORACLE: Mapeamento de Tipos 
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {

            configurationBuilder.Properties<bool>()
                .HaveConversion<long>();
        }

        //  Configuração das Entidades (Fluent API)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da Tabela COLABORADORES
            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.ToTable("tbl_colaboradores");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.NomeColaborador).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Senha).IsRequired().HasMaxLength(255);
                entity.HasIndex(c => c.Email).IsUnique();


                entity.HasMany(c => c.Participacao)
                      .WithOne(p => p.Colaborador!)
                      .HasForeignKey(p => p.ColaboradorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da Tabela TREINAMENTOS
            modelBuilder.Entity<TreinamentoModel>(entity =>
            {
                entity.ToTable("tbl_treinamentos");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Descricao).IsRequired();

                // Relação 1-para-Muitos: Treinamento -> Participacao
                entity.HasMany(t => t.Participacao)
                      .WithOne(p => p.Treinamento!)
                      .HasForeignKey(p => p.TreinamentoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da Tabela (ParticipacaoEmTreinamentoModel)
            modelBuilder.Entity<ParticipacaoEmTreinamentoModel>(entity =>
            {
                entity.ToTable("tbl_participacao_treinamento");
                entity.HasKey(p => p.Id);

                // Garante que as Chaves Estrangeiras são NOT NULL
                entity.Property(p => p.ColaboradorId).IsRequired();
                entity.Property(p => p.TreinamentoId).IsRequired();

                // DataDeConclusao (opcional)
                entity.Property(p => p.DataDeConclusao).IsRequired(false);

                // Garante unicidade da participação (Colaborador + Treinamento)
                entity.HasIndex(p => new { p.ColaboradorId, p.TreinamentoId }).IsUnique();
            });

            // MODELBUILDER PARA O RELATÓRIO
            modelBuilder.Entity<RelatorioDeDiversidadeModel>(entity =>
            {
                entity.ToTable("tbl_relatorios_diversidade");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                entity.Property(r => r.DataGerada).IsRequired();
            });

        }
    }
}