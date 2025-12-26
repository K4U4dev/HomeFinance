namespace HomeFinance.Service.DTOs;

/// <summary>
/// DTO que representa os totais financeiros de uma pessoa.
/// Utilizado na consulta de totais por pessoa.
/// </summary>
public class TotaisPorPessoaDto
{
    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    public Guid PessoaId { get; set; }
    
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string PessoaNome { get; set; } = string.Empty;
    
    /// <summary>
    /// Total de receitas da pessoa.
    /// </summary>
    public decimal TotalReceitas { get; set; }
    
    /// <summary>
    /// Total de despesas da pessoa.
    /// </summary>
    public decimal TotalDespesas { get; set; }
    
    /// <summary>
    /// Saldo da pessoa (receitas - despesas).
    /// </summary>
    public decimal Saldo { get; set; }
}

/// <summary>
/// DTO que representa o resultado completo da consulta de totais por pessoa.
/// Inclui a lista de totais por pessoa e os totais gerais.
/// </summary>
public class ConsultaTotaisPorPessoaDto
{
    /// <summary>
    /// Lista de totais por pessoa.
    /// </summary>
    public List<TotaisPorPessoaDto> TotaisPorPessoa { get; set; } = new();
    
    /// <summary>
    /// Total geral de receitas de todas as pessoas.
    /// </summary>
    public decimal TotalGeralReceitas { get; set; }
    
    /// <summary>
    /// Total geral de despesas de todas as pessoas.
    /// </summary>
    public decimal TotalGeralDespesas { get; set; }
    
    /// <summary>
    /// Saldo líquido geral (total receitas - total despesas).
    /// </summary>
    public decimal SaldoLiquidoGeral { get; set; }
}

