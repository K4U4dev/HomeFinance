using HomeFinance.Service.DTOs;

namespace HomeFinance.Service.Interfaces;

/// <summary>
/// Interface do serviço de consultas e relatórios financeiros.
/// Define os contratos para consultas agregadas de dados financeiros.
/// </summary>
public interface IConsultaService
{
    /// <summary>
    /// Consulta os totais financeiros agrupados por pessoa.
    /// Retorna o total de receitas, despesas e saldo de cada pessoa,
    /// além dos totais gerais de todas as pessoas.
    /// </summary>
    /// <returns>DTO com totais por pessoa e totais gerais</returns>
    Task<ConsultaTotaisPorPessoaDto> ObterTotaisPorPessoaAsync();
    
    /// <summary>
    /// Consulta os totais financeiros agrupados por categoria.
    /// Retorna o total de receitas, despesas e saldo de cada categoria,
    /// além dos totais gerais de todas as categorias.
    /// </summary>
    /// <returns>DTO com totais por categoria e totais gerais</returns>
    Task<ConsultaTotaisPorCategoriaDto> ObterTotaisPorCategoriaAsync();
}

