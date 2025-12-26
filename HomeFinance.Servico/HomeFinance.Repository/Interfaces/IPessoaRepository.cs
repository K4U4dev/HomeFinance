using HomeFinance.Domain.Entities;

namespace HomeFinance.Repository.Interfaces;

/// <summary>
/// Interface do repositório para operações relacionadas à entidade Pessoa.
/// Define os contratos para acesso e manipulação de dados de pessoas no banco de dados.
/// </summary>
public interface IPessoaRepository
{
    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    Task<IEnumerable<Pessoa>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da pessoa</param>
    /// <returns>Pessoa encontrada ou null se não existir</returns>
    Task<Pessoa?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Adiciona uma nova pessoa ao banco de dados.
    /// </summary>
    /// <param name="pessoa">Entidade Pessoa a ser adicionada</param>
    /// <returns>Pessoa adicionada com o Id gerado</returns>
    Task<Pessoa> AdicionarAsync(Pessoa pessoa);
    
    /// <summary>
    /// Remove uma pessoa do banco de dados.
    /// As transações associadas serão removidas automaticamente devido ao cascade delete.
    /// </summary>
    /// <param name="pessoa">Entidade Pessoa a ser removida</param>
    Task RemoverAsync(Pessoa pessoa);
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    /// <returns>Número de registros afetados</returns>
    Task<int> SalvarAlteracoesAsync();
}

