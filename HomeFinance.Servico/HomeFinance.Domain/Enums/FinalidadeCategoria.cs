namespace HomeFinance.Domain.Enums;

/// <summary>
/// Enum que representa a finalidade de uma categoria.
/// Define para quais tipos de transações a categoria pode ser utilizada.
/// </summary>
public enum FinalidadeCategoria
{
    /// <summary>
    /// Categoria pode ser usada apenas para despesas
    /// </summary>
    Despesa = 1,
    
    /// <summary>
    /// Categoria pode ser usada apenas para receitas
    /// </summary>
    Receita = 2,
    
    /// <summary>
    /// Categoria pode ser usada tanto para despesas quanto para receitas
    /// </summary>
    Ambas = 3
}

