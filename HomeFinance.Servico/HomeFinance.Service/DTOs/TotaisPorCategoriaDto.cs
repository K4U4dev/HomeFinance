namespace HomeFinance.Service.DTOs;

/// <summary>
/// DTO que representa os totais financeiros de uma categoria.
/// Utilizado na consulta de totais por categoria (opcional).
/// </summary>
public class TotaisPorCategoriaDto
{
    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    public Guid CategoriaId { get; set; }
    
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string CategoriaDescricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Total de receitas da categoria.
    /// </summary>
    public decimal TotalReceitas { get; set; }
    
    /// <summary>
    /// Total de despesas da categoria.
    /// </summary>
    public decimal TotalDespesas { get; set; }
    
    /// <summary>
    /// Saldo da categoria (receitas - despesas).
    /// </summary>
    public decimal Saldo { get; set; }
}

/// <summary>
/// DTO que representa o resultado completo da consulta de totais por categoria.
/// Inclui a lista de totais por categoria e os totais gerais.
/// </summary>
public class ConsultaTotaisPorCategoriaDto
{
    /// <summary>
    /// Lista de totais por categoria.
    /// </summary>
    public List<TotaisPorCategoriaDto> TotaisPorCategoria { get; set; } = new();
    
    /// <summary>
    /// Total geral de receitas de todas as categorias.
    /// </summary>
    public decimal TotalGeralReceitas { get; set; }
    
    /// <summary>
    /// Total geral de despesas de todas as categorias.
    /// </summary>
    public decimal TotalGeralDespesas { get; set; }
    
    /// <summary>
    /// Saldo líquido geral (total receitas - total despesas).
    /// </summary>
    public decimal SaldoLiquidoGeral { get; set; }
}

