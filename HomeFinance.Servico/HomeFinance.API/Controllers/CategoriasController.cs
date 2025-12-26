using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinance.API.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas a categorias.
/// Fornece endpoints para criação e listagem de categorias.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly ILogger<CategoriasController> _logger;
    
    /// <summary>
    /// Construtor que recebe o serviço de categorias e o logger via injeção de dependência.
    /// </summary>
    public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService;
        _logger = logger;
    }
    
    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de categorias</returns>
    /// <response code="200">Retorna a lista de categorias com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObterTodas()
    {
        try
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            return Ok(categorias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as categorias");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter categorias." });
        }
    }
    
    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da categoria</param>
    /// <returns>Dados da categoria</returns>
    /// <response code="200">Retorna a categoria encontrada</response>
    /// <response code="404">Categoria não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaDto>> ObterPorId(Guid id)
    {
        try
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            
            if (categoria == null)
            {
                return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });
            }
            
            return Ok(categoria);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter categoria com ID {CategoriaId}", id);
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao obter categoria." });
        }
    }
    
    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// Validações aplicadas:
    /// - Descrição é obrigatória e não pode ser vazia
    /// - Finalidade deve ser um valor válido (Despesa, Receita ou Ambas)
    /// </summary>
    /// <param name="dto">DTO com os dados da categoria a ser criada</param>
    /// <returns>Dados da categoria criada</returns>
    /// <response code="201">Categoria criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoriaDto>> Criar([FromBody] CriarCategoriaDto dto)
    {
        try
        {
            var categoria = await _categoriaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos ao criar categoria");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar categoria");
            return StatusCode(500, new { mensagem = "Erro interno do servidor ao criar categoria." });
        }
    }
}

