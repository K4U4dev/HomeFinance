using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;

namespace HomeFinance.Service.Services;

/// <summary>
/// Serviço de negócio para operações relacionadas a pessoas.
/// Implementa a lógica de negócio e validações para o cadastro de pessoas.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    
    /// <summary>
    /// Construtor que recebe o repositório de pessoas via injeção de dependência.
    /// </summary>
    /// <param name="pessoaRepository">Repositório para acesso aos dados de pessoas</param>
    public PessoaService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }
    
    /// <summary>
    /// Obtém todas as pessoas cadastradas e converte para DTOs.
    /// </summary>
    public async Task<IEnumerable<PessoaDto>> ObterTodasAsync()
    {
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        return pessoas.Select(p => MapearParaDto(p));
    }
    
    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador e converte para DTO.
    /// </summary>
    public async Task<PessoaDto?> ObterPorIdAsync(Guid id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        return pessoa != null ? MapearParaDto(pessoa) : null;
    }
    
    /// <summary>
    /// Cria uma nova pessoa após validar os dados de entrada.
    /// Validações:
    /// - Nome não pode ser vazio ou nulo
    /// - Idade deve ser um número inteiro positivo
    /// </summary>
    public async Task<PessoaDto> CriarAsync(CriarPessoaDto dto)
    {
        // Validação: Nome não pode ser vazio ou nulo
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            throw new ArgumentException("O nome da pessoa é obrigatório e não pode ser vazio.", nameof(dto));
        }
        
        // Validação: Idade deve ser um número inteiro positivo
        if (dto.Idade <= 0)
        {
            throw new ArgumentException("A idade deve ser um número inteiro positivo.", nameof(dto));
        }
        
        // Cria a entidade Pessoa a partir do DTO
        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };
        
        // Adiciona a pessoa ao repositório
        await _pessoaRepository.AdicionarAsync(pessoa);
        await _pessoaRepository.SalvarAlteracoesAsync();
        
        // Retorna o DTO da pessoa criada
        return MapearParaDto(pessoa);
    }
    
    /// <summary>
    /// Remove uma pessoa do sistema.
    /// As transações associadas serão removidas automaticamente devido ao cascade delete.
    /// </summary>
    public async Task<bool> RemoverAsync(Guid id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        
        if (pessoa == null)
        {
            return false;
        }
        
        // Remove a pessoa (as transações serão removidas automaticamente)
        await _pessoaRepository.RemoverAsync(pessoa);
        await _pessoaRepository.SalvarAlteracoesAsync();
        
        return true;
    }
    
    /// <summary>
    /// Método auxiliar para mapear uma entidade Pessoa para um DTO.
    /// </summary>
    private static PessoaDto MapearParaDto(Pessoa pessoa)
    {
        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        };
    }
}

