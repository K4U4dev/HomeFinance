using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinance.API.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas a transações.
/// Fornece endpoints para criação e listagem de transações financeiras.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;
    private readonly ILogger<TransacoesController> _logger;
    
    /// <summary>
    /// Construtor que recebe o serviço de transações e o logger via injeção de dependência.
    /// </summary>
    public TransacoesController(ITransacaoService transacaoService, ILogger<TransacoesController> logger)
    {
        _transacaoService = transacaoService;
        _logger = logger;
    }
    
    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de transações</returns>
    /// <response code="200">Retorna a lista de transações com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransacaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TransacaoDto>>> ObterTodas()
    {
        try
        {
            var transacoes = await _transacaoService.ObterTodasAsync();
            return Ok(transacoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as transações");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter transações." });
        }
    }
    
    /// <summary>
    /// Obtém uma transação específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da transação</param>
    /// <returns>Dados da transação</returns>
    /// <response code="200">Retorna a transação encontrada</response>
    /// <response code="404">Transação não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TransacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransacaoDto>> ObterPorId(Guid id)
    {
        try
        {
            var transacao = await _transacaoService.ObterPorIdAsync(id);
            
            if (transacao == null)
            {
                return NotFound(new { mensagem = $"Transação com ID {id} não encontrada." });
            }
            
            return Ok(transacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter transação com ID {TransacaoId}", id);
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter transação." });
        }
    }
    
    /// <summary>
    /// Cria uma nova transação no sistema.
    /// Validações aplicadas:
    /// - Descrição é obrigatória e não pode ser vazia
    /// - Valor deve ser um número decimal positivo
    /// - Pessoa deve existir
    /// - Categoria deve existir e ser compatível com o tipo da transação
    /// - Menores de 18 anos só podem ter despesas
    /// </summary>
    /// <param name="dto">DTO com os dados da transação a ser criada</param>
    /// <returns>Dados da transação criada</returns>
    /// <response code="201">Transação criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(TransacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransacaoDto>> Criar([FromBody] CriarTransacaoDto dto)
    {
        try
        {
            var transacao = await _transacaoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = transacao.Id }, transacao);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar transação");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regra de negócio violada ao criar transação");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao criar transação." });
        }
    }
}

