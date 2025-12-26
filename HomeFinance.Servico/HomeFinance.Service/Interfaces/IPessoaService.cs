using HomeFinance.Service.DTOs;

namespace HomeFinance.Service.Interfaces;

/// <summary>
/// Interface do serviço de negócio para operações relacionadas a pessoas.
/// Define os contratos para lógica de negócio e validações relacionadas a pessoas.
/// </summary>
public interface IPessoaService
{
    /// <summary>
    /// Obtém todas as pessoas cadastradas.
    /// </summary>
    /// <returns>Lista de DTOs de pessoas</returns>
    Task<IEnumerable<PessoaDto>> ObterTodasAsync();
    
    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da pessoa</param>
    /// <returns>DTO da pessoa ou null se não encontrada</returns>
    Task<PessoaDto?> ObterPorIdAsync(Guid id);
    
    /// <summary>
    /// Cria uma nova pessoa após validar os dados de entrada.
    /// </summary>
    /// <param name="dto">DTO com os dados da pessoa a ser criada</param>
    /// <returns>DTO da pessoa criada</returns>
    Task<PessoaDto> CriarAsync(CriarPessoaDto dto);
    
    /// <summary>
    /// Remove uma pessoa do sistema.
    /// As transações associadas serão removidas automaticamente.
    /// </summary>
    /// <param name="id">Identificador único da pessoa a ser removida</param>
    /// <returns>True se a pessoa foi removida, False se não foi encontrada</returns>
    Task<bool> RemoverAsync(Guid id);
}

