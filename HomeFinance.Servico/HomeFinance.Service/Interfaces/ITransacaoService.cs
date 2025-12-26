using HomeFinance.Service.DTOs;

namespace HomeFinance.Service.Interfaces;

/// <summary>
/// Interface do serviço de negócio para operações relacionadas a transações.
/// Define os contratos para lógica de negócio e validações relacionadas a transações.
/// </summary>
public interface ITransacaoService
{
    /// <summary>
    /// Obtém todas as transações cadastradas.
    /// </summary>
    /// <returns>Lista de DTOs de transações</returns>
    Task<IEnumerable<TransacaoDto>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da transação</param>
    /// <returns>DTO da transação ou null se não encontrada</returns>
    Task<TransacaoDto?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Cria uma nova transação após validar todas as regras de negócio:
    /// - Valor deve ser positivo
    /// - Pessoa deve existir
    /// - Categoria deve existir e ser compatível com o tipo da transação
    /// - Menores de 18 anos só podem ter despesas
    /// </summary>
    /// <param name="dto">DTO com os dados da transação a ser criada</param>
    /// <returns>DTO da transação criada</returns>
    Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto);
}

