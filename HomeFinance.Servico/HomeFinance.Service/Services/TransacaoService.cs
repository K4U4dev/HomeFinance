using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;

namespace HomeFinance.Service.Services;

/// <summary>
/// Serviço de negócio para operações relacionadas a transações.
/// Implementa a lógica de negócio e validações para o cadastro de transações.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    
    /// <summary>
    /// Construtor que recebe os repositórios via injeção de dependência.
    /// </summary>
    /// <param name="transacaoRepository">Repositório para acesso aos dados de transações</param>
    /// <param name="pessoaRepository">Repositório para acesso aos dados de pessoas</param>
    /// <param name="categoriaRepository">Repositório para acesso aos dados de categorias</param>
    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
        _categoriaRepository = categoriaRepository;
    }
    
    /// <summary>
    /// Obtém todas as transações cadastradas e converte para DTOs.
    /// </summary>
    public async Task<IEnumerable<TransacaoDto>> ObterTodasAsync()
    {
        var transacoes = await _transacaoRepository.ObterTodasAsync();
        return transacoes.Select(t => MapearParaDto(t));
    }
    
    /// <summary>
    /// Obtém uma transação específica pelo seu identificador e converte para DTO.
    /// </summary>
    public async Task<TransacaoDto?> ObterPorIdAsync(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        return transacao != null ? MapearParaDto(transacao) : null;
    }
    
    /// <summary>
    /// Cria uma nova transação após validar todas as regras de negócio:
    /// - Descrição não pode ser vazia ou nula
    /// - Valor deve ser positivo
    /// - Pessoa deve existir
    /// - Categoria deve existir e ser compatível com o tipo da transação
    /// - Menores de 18 anos só podem ter despesas
    /// </summary>
    public async Task<TransacaoDto> CriarAsync(CriarTransacaoDto dto)
    {
        // Validação: Descrição não pode ser vazia ou nula
        if (string.IsNullOrWhiteSpace(dto.Descricao))
        {
            throw new ArgumentException("A descrição da transação é obrigatória e não pode ser vazia.", nameof(dto));
        }
        
        // Validação: Valor deve ser positivo
        if (dto.Valor <= 0)
        {
            throw new ArgumentException("O valor da transação deve ser um número decimal positivo.", nameof(dto));
        }
        
        // Validação: Pessoa deve existir
        var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);
        if (pessoa == null)
        {
            throw new ArgumentException($"Pessoa com ID {dto.PessoaId} não encontrada.", nameof(dto));
        }
        
        // Validação: Categoria deve existir
        var categoria = await _categoriaRepository.ObterPorIdAsync(dto.CategoriaId);
        if (categoria == null)
        {
            throw new ArgumentException($"Categoria com ID {dto.CategoriaId} não encontrada.", nameof(dto));
        }
        
        // Validação: Categoria deve ser compatível com o tipo da transação
        // Se a transação é despesa, a categoria não pode ter finalidade apenas receita
        // Se a transação é receita, a categoria não pode ter finalidade apenas despesa
        if (!ValidarCompatibilidadeCategoria(categoria.Finalidade, dto.Tipo))
        {
            throw new InvalidOperationException(
                $"A categoria '{categoria.Descricao}' não pode ser utilizada para transações do tipo {dto.Tipo}. " +
                $"A categoria tem finalidade '{categoria.Finalidade}'.");
        }
        
        // Validação: Menores de 18 anos só podem ter despesas
        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
        {
            throw new InvalidOperationException(
                $"Pessoas menores de 18 anos não podem ter receitas. A pessoa '{pessoa.Nome}' tem {pessoa.Idade} anos.");
        }
        
        // Cria a entidade Transacao a partir do DTO
        var transacao = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            CategoriaId = dto.CategoriaId,
            PessoaId = dto.PessoaId
        };
        
        // Adiciona a transação ao repositório
        await _transacaoRepository.AdicionarAsync(transacao);
        await _transacaoRepository.SalvarAlteracoesAsync();
        
        // Recarrega a transação com as relações para retornar dados completos
        var transacaoCompleta = await _transacaoRepository.ObterPorIdAsync(transacao.Id);
        
        // Retorna o DTO da transação criada
        return MapearParaDto(transacaoCompleta!);
    }
    
    /// <summary>
    /// Valida se a finalidade da categoria é compatível com o tipo da transação.
    /// Regras:
    /// - Categoria com finalidade "Despesa" só pode ser usada em transações do tipo Despesa
    /// - Categoria com finalidade "Receita" só pode ser usada em transações do tipo Receita
    /// - Categoria com finalidade "Ambas" pode ser usada em qualquer tipo de transação
    /// </summary>
    /// <param name="finalidadeCategoria">Finalidade da categoria</param>
    /// <param name="tipoTransacao">Tipo da transação</param>
    /// <returns>True se são compatíveis, False caso contrário</returns>
    private static bool ValidarCompatibilidadeCategoria(FinalidadeCategoria finalidadeCategoria, TipoTransacao tipoTransacao)
    {
        return finalidadeCategoria switch
        {
            FinalidadeCategoria.Ambas => true, // Categoria pode ser usada para ambos os tipos
            FinalidadeCategoria.Despesa => tipoTransacao == TipoTransacao.Despesa,
            FinalidadeCategoria.Receita => tipoTransacao == TipoTransacao.Receita,
            _ => false
        };
    }
    
    /// <summary>
    /// Método auxiliar para mapear uma entidade Transacao para um DTO.
    /// </summary>
    private static TransacaoDto MapearParaDto(Transacao transacao)
    {
        return new TransacaoDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            CategoriaId = transacao.CategoriaId,
            CategoriaDescricao = transacao.Categoria?.Descricao,
            PessoaId = transacao.PessoaId,
            PessoaNome = transacao.Pessoa?.Nome
        };
    }
}

