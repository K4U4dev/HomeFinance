namespace HomeFinance.Domain.Enums;

/// <summary>
/// Enum que representa o tipo de transação financeira.
/// Despesa: saída de dinheiro
/// Receita: entrada de dinheiro
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Representa uma despesa (saída de dinheiro)
    /// </summary>
    Despesa = 1,
    
    /// <summary>
    /// Representa uma receita (entrada de dinheiro)
    /// </summary>
    Receita = 2
}

