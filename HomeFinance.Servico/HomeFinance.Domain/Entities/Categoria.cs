using HomeFinance.Domain.Enums;

namespace HomeFinance.Domain.Entities;

/// <summary>
/// Entidade que representa uma categoria de transação financeira.
/// Define categorias que podem ser utilizadas para classificar transações,
/// com restrições de finalidade (despesa, receita ou ambas).
/// </summary>
public class Categoria
{
    /// <summary>
    /// Identificador único da categoria, gerado automaticamente pelo banco de dados.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Descrição da categoria (ex: "Alimentação", "Salário", "Transporte").
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria que define para quais tipos de transações ela pode ser utilizada.
    /// Valores possíveis: Despesa, Receita ou Ambas.
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
    
    /// <summary>
    /// Navegação para as transações que utilizam esta categoria.
    /// Permite acesso às transações relacionadas através do Entity Framework.
    /// </summary>
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}

