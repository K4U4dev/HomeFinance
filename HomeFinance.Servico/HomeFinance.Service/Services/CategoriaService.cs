using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;

namespace HomeFinance.Service.Services;

/// <summary>
/// Serviço de negócio para operações relacionadas a categorias.
/// Implementa a lógica de negócio e validações para o cadastro de categorias.
/// </summary>
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    
    /// <summary>
    /// Construtor que recebe o repositório de categorias via injeção de dependência.
    /// </summary>
    /// <param name="categoriaRepository">Repositório para acesso aos dados de categorias</param>
    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }
    
    /// <summary>
    /// Obtém todas as categorias cadastradas e converte para DTOs.
    /// </summary>
    public async Task<IEnumerable<CategoriaDto>> ObterTodasAsync()
    {
        var categorias = await _categoriaRepository.ObterTodasAsync();
        return categorias.Select(c => MapearParaDto(c));
    }
    
    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador e converte para DTO.
    /// </summary>
    public async Task<CategoriaDto?> ObterPorIdAsync(Guid id)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id);
        return categoria != null ? MapearParaDto(categoria) : null;
    }
    
    /// <summary>
    /// Cria uma nova categoria após validar os dados de entrada.
    /// Validações:
    /// - Descrição não pode ser vazia ou nula
    /// - Finalidade deve ser um valor válido do enum
    /// </summary>
    public async Task<CategoriaDto> CriarAsync(CriarCategoriaDto dto)
    {
        // Validação: Descrição não pode ser vazia ou nula
        if (string.IsNullOrWhiteSpace(dto.Descricao))
        {
            throw new ArgumentException("A descrição da categoria é obrigatória e não pode ser vazia.", nameof(dto));
        }
        
        // Validação: Finalidade deve ser um valor válido do enum
        if (!Enum.IsDefined(typeof(Domain.Enums.FinalidadeCategoria), dto.Finalidade))
        {
            throw new ArgumentException("A finalidade da categoria deve ser Despesa, Receita ou Ambas.", nameof(dto));
        }
        
        // Cria a entidade Categoria a partir do DTO
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Descricao = dto.Descricao.Trim(),
            Finalidade = dto.Finalidade
        };
        
        // Adiciona a categoria ao repositório
        await _categoriaRepository.AdicionarAsync(categoria);
        await _categoriaRepository.SalvarAlteracoesAsync();
        
        // Retorna o DTO da categoria criada
        return MapearParaDto(categoria);
    }
    
    /// <summary>
    /// Método auxiliar para mapear uma entidade Categoria para um DTO.
    /// </summary>
    private static CategoriaDto MapearParaDto(Categoria categoria)
    {
        return new CategoriaDto
        {
            Id = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade
        };
    }
}

