using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinance.API.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas a pessoas.
/// Fornece endpoints para criação, listagem e exclusão de pessoas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _pessoaService;
    private readonly ILogger<PessoasController> _logger;
    
    /// <summary>
    /// Construtor que recebe o serviço de pessoas e o logger via injeção de dependência.
    /// </summary>
    public PessoasController(IPessoaService pessoaService, ILogger<PessoasController> logger)
    {
        _pessoaService = pessoaService;
        _logger = logger;
    }
    
    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de pessoas</returns>
    /// <response code="200">Retorna a lista de pessoas com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PessoaDto>>> ObterTodas()
    {
        try
        {
            var pessoas = await _pessoaService.ObterTodasAsync();
            return Ok(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as pessoas");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter pessoas." });
        }
    }
    
    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da pessoa</param>
    /// <returns>Dados da pessoa</returns>
    /// <response code="200">Retorna a pessoa encontrada</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PessoaDto>> ObterPorId(Guid id)
    {
        try
        {
            var pessoa = await _pessoaService.ObterPorIdAsync(id);
            
            if (pessoa == null)
            {
                return NotFound(new { mensagem = $"Pessoa com ID {id} não encontrada." });
            }
            
            return Ok(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pessoa com ID {PessoaId}", id);
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter pessoa." });
        }
    }
    
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// Validações aplicadas:
    /// - Nome é obrigatório e não pode ser vazio
    /// - Idade deve ser um número inteiro positivo
    /// </summary>
    /// <param name="dto">DTO com os dados da pessoa a ser criada</param>
    /// <returns>Dados da pessoa criada</returns>
    /// <response code="201">Pessoa criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PessoaDto>> Criar([FromBody] CriarPessoaDto dto)
    {
        try
        {
            var pessoa = await _pessoaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar pessoa");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao criar pessoa." });
        }
    }
    
    /// <summary>
    /// Remove uma pessoa do sistema.
    /// Todas as transações associadas à pessoa serão removidas automaticamente.
    /// </summary>
    /// <param name="id">Identificador único da pessoa a ser removida</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="204">Pessoa removida com sucesso</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id)
    {
        try
        {
            var removida = await _pessoaService.RemoverAsync(id);
            
            if (!removida)
            {
                return NotFound(new { mensagem = $"Pessoa com ID {id} não encontrada." });
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover pessoa com ID {PessoaId}", id);
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao remover pessoa." });
        }
    }
}

