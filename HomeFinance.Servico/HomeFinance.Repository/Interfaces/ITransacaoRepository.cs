using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;

namespace HomeFinance.Repository.Interfaces;

/// <summary>
/// Interface do repositório para operações relacionadas à entidade Transacao.
/// Define os contratos para acesso e manipulação de dados de transações no banco de dados.
/// </summary>
public interface ITransacaoRepository
{
    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as transações com suas relações (Pessoa e Categoria)</returns>
    Task<IEnumerable<Transacao>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma transação específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da transação</param>
    /// <returns>Transação encontrada ou null se não existir</returns>
    Task<Transacao?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="pessoaId">Identificador único da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    Task<IEnumerable<Transacao>> ObterPorPessoaIdAsync(Guid pessoaId);
    
    /// <summary>
    /// Obtém todas as transações de uma categoria específica.
    /// </summary>
    /// <param name="categoriaId">Identificador único da categoria</param>
    /// <returns>Lista de transações da categoria</returns>
    Task<IEnumerable<Transacao>> ObterPorCategoriaIdAsync(Guid categoriaId);
    
    /// <summary>
    /// Adiciona uma nova transação ao banco de dados.
    /// </summary>
    /// <param name="transacao">Entidade Transacao a ser adicionada</param>
    /// <returns>Transação adicionada com o Id gerado</returns>
    Task<Transacao> AdicionarAsync(Transacao transacao);
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    /// <returns>Número de registros afetados</returns>
    Task<int> SalvarAlteracoesAsync();
}

