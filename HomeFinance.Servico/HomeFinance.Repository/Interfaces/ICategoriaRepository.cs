using HomeFinance.Domain.Entities;

namespace HomeFinance.Repository.Interfaces;

/// <summary>
/// Interface do repositório para operações relacionadas à entidade Categoria.
/// Define os contratos para acesso e manipulação de dados de categorias no banco de dados.
/// </summary>
public interface ICategoriaRepository
{
    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    Task<IEnumerable<Categoria>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da categoria</param>
    /// <returns>Categoria encontrada ou null se não existir</returns>
    Task<Categoria?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Adiciona uma nova categoria ao banco de dados.
    /// </summary>
    /// <param name="categoria">Entidade Categoria a ser adicionada</param>
    /// <returns>Categoria adicionada com o Id gerado</returns>
    Task<Categoria> AdicionarAsync(Categoria categoria);
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    /// <returns>Número de registros afetados</returns>
    Task<int> SalvarAlteracoesAsync();
}

