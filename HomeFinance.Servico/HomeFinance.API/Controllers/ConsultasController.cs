using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinance.API.Controllers;

/// <summary>
/// Controller responsável por fornecer consultas e relatórios financeiros.
/// Fornece endpoints para consultas agregadas de dados financeiros.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaService _consultaService;
    private readonly ILogger<ConsultasController> _logger;
    
    /// <summary>
    /// Construtor que recebe o serviço de consultas e o logger via injeção de dependência.
    /// </summary>
    public ConsultasController(IConsultaService consultaService, ILogger<ConsultasController> logger)
    {
        _consultaService = consultaService;
        _logger = logger;
    }
    
    /// <summary>
    /// Consulta os totais financeiros agrupados por pessoa.
    /// Retorna o total de receitas, despesas e saldo de cada pessoa,
    /// além dos totais gerais de todas as pessoas.
    /// </summary>
    /// <returns>Totais por pessoa e totais gerais</returns>
    /// <response code="200">Retorna os totais por pessoa com sucesso</response>
    [HttpGet("totais-por-pessoa")]
    [ProducesResponseType(typeof(Service.DTOs.ConsultaTotaisPorPessoaDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<Service.DTOs.ConsultaTotaisPorPessoaDto>> ObterTotaisPorPessoa()
    {
        try
        {
            var resultado = await _consultaService.ObterTotaisPorPessoaAsync();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter totais por pessoa");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter totais por pessoa." });
        }
    }
    
    /// <summary>
    /// Consulta os totais financeiros agrupados por categoria.
    /// Retorna o total de receitas, despesas e saldo de cada categoria,
    /// além dos totais gerais de todas as categorias.
    /// </summary>
    /// <returns>Totais por categoria e totais gerais</returns>
    /// <response code="200">Retorna os totais por categoria com sucesso</response>
    [HttpGet("totais-por-categoria")]
    [ProducesResponseType(typeof(Service.DTOs.ConsultaTotaisPorCategoriaDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<Service.DTOs.ConsultaTotaisPorCategoriaDto>> ObterTotaisPorCategoria()
    {
        try
        {
            var resultado = await _consultaService.ObterTotaisPorCategoriaAsync();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter totais por categoria");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter totais por categoria." });
        }
    }
}

