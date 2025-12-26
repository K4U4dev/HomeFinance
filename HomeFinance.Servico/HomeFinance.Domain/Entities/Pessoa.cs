namespace HomeFinance.Domain.Entities;

/// <summary>
/// Entidade que representa uma pessoa no sistema.
/// Armazena informações básicas de identificação e idade para controle de gastos.
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Identificador único da pessoa, gerado automaticamente pelo banco de dados.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa em anos. Deve ser um número inteiro positivo.
    /// Utilizada para validações de regras de negócio (ex: menores de 18 só podem ter despesas).
    /// </summary>
    public int Idade { get; set; }
    
    /// <summary>
    /// Navegação para as transações associadas a esta pessoa.
    /// Permite acesso às transações relacionadas através do Entity Framework.
    /// </summary>
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}

