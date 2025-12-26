using HomeFinance.Domain.Enums;

namespace HomeFinance.Domain.Entities;

/// <summary>
/// Entidade que representa uma transação financeira no sistema.
/// Armazena informações sobre receitas e despesas de uma pessoa,
/// associadas a uma categoria específica.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação, gerado automaticamente pelo banco de dados.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Descrição da transação (ex: "Compra no supermercado", "Salário mensal").
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação. Deve ser um número decimal positivo.
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação: Despesa (saída) ou Receita (entrada).
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Identificador da categoria associada a esta transação.
    /// Chave estrangeira para a entidade Categoria.
    /// </summary>
    public Guid CategoriaId { get; set; }
    
    /// <summary>
    /// Navegação para a categoria associada.
    /// Permite acesso à categoria relacionada através do Entity Framework.
    /// </summary>
    public virtual Categoria Categoria { get; set; } = null!;
    
    /// <summary>
    /// Identificador da pessoa associada a esta transação.
    /// Chave estrangeira para a entidade Pessoa.
    /// </summary>
    public Guid PessoaId { get; set; }
    
    /// <summary>
    /// Navegação para a pessoa associada.
    /// Permite acesso à pessoa relacionada através do Entity Framework.
    /// </summary>
    public virtual Pessoa Pessoa { get; set; } = null!;
}

