using HomeFinance.Service.DTOs;

namespace HomeFinance.Service.Interfaces;

/// <summary>
/// Interface do serviço de negócio para operações relacionadas a categorias.
/// Define os contratos para lógica de negócio e validações relacionadas a categorias.
/// </summary>
public interface ICategoriaService
{
    /// <summary>
    /// Obtém todas as categorias cadastradas.
    /// </summary>
    /// <returns>Lista de DTOs de categorias</returns>
    Task<IEnumerable<CategoriaDto>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da categoria</param>
    /// <returns>DTO da categoria ou null se não encontrada</returns>
    Task<CategoriaDto?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Cria uma nova categoria após validar os dados de entrada.
    /// </summary>
    /// <param name="dto">DTO com os dados da categoria a ser criada</param>
    /// <returns>DTO da categoria criada</returns>
    Task<CategoriaDto> CriarAsync(CriarCategoriaDto dto);
}

