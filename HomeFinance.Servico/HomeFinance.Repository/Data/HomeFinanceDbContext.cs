using HomeFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Repository.Data;

/// <summary>
/// Contexto do Entity Framework para o banco de dados HomeFinance.
/// Gerencia todas as entidades e suas relações, além de configurar o mapeamento
/// entre as entidades de domínio e as tabelas do banco de dados PostgreSQL.
/// </summary>
public class HomeFinanceDbContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext, incluindo string de conexão</param>
    public HomeFinanceDbContext(DbContextOptions<HomeFinanceDbContext> options)
        : base(options)
    {
    }
    
    /// <summary>
    /// DbSet para a entidade Pessoa.
    /// Representa a tabela de pessoas no banco de dados.
    /// </summary>
    public DbSet<Pessoa> Pessoas { get; set; }
    
    /// <summary>
    /// DbSet para a entidade Categoria.
    /// Representa a tabela de categorias no banco de dados.
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }
    
    /// <summary>
    /// DbSet para a entidade Transacao.
    /// Representa a tabela de transações no banco de dados.
    /// </summary>
    public DbSet<Transacao> Transacoes { get; set; }
    
    /// <summary>
    /// Método chamado durante a criação do modelo para configurar
    /// os mapeamentos e relacionamentos entre as entidades.
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelo do Entity Framework</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuração da entidade Pessoa
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Idade)
                .IsRequired();
            
            // Configuração de exclusão em cascata: ao deletar uma pessoa,
            // todas suas transações são automaticamente deletadas
            entity.HasMany(e => e.Transacoes)
                .WithOne(e => e.Pessoa)
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configuração da entidade Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Finalidade)
                .IsRequired()
                .HasConversion<int>(); // Converte enum para int no banco
        });
        
        // Configuração da entidade Transacao
        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Valor)
                .IsRequired()
                .HasPrecision(18, 2); // Define precisão decimal (18 dígitos, 2 casas decimais)
            entity.Property(e => e.Tipo)
                .IsRequired()
                .HasConversion<int>(); // Converte enum para int no banco
            
            // Relacionamento com Categoria
            entity.HasOne(e => e.Categoria)
                .WithMany(e => e.Transacoes)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); // Impede deletar categoria que tem transações
            
            // Relacionamento com Pessoa já configurado acima com cascade delete
        });
    }
}

