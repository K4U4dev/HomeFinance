using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Data;
using HomeFinance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Repository.Repositories;

/// <summary>
/// Implementação do repositório para operações relacionadas à entidade Transacao.
/// Fornece métodos para acesso e manipulação de dados de transações no banco de dados.
/// </summary>
public class TransacaoRepository : ITransacaoRepository
{
    private readonly HomeFinanceDbContext _context;
    
    /// <summary>
    /// Construtor que recebe o contexto do Entity Framework.
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public TransacaoRepository(HomeFinanceDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Obtém todas as transações cadastradas no sistema, incluindo relações com Pessoa e Categoria.
    /// </summary>
    public async Task<IEnumerable<Transacao>> ObterTodasAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .OrderByDescending(t => t.Descricao)
            .ToListAsync();
    }
    
    /// <summary>
    /// Obtém uma transação específica pelo seu identificador único.
    /// </summary>
    public async Task<Transacao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    public async Task<IEnumerable<Transacao>> ObterPorPessoaIdAsync(Guid pessoaId)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Where(t => t.PessoaId == pessoaId)
            .OrderByDescending(t => t.Descricao)
            .ToListAsync();
    }
    
    /// <summary>
    /// Obtém todas as transações de uma categoria específica.
    /// </summary>
    public async Task<IEnumerable<Transacao>> ObterPorCategoriaIdAsync(Guid categoriaId)
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Where(t => t.CategoriaId == categoriaId)
            .OrderByDescending(t => t.Descricao)
            .ToListAsync();
    }
    
    /// <summary>
    /// Adiciona uma nova transação ao banco de dados.
    /// </summary>
    public async Task<Transacao> AdicionarAsync(Transacao transacao)
    {
        // Gera um novo Guid se não foi fornecido
        if (transacao.Id == Guid.Empty)
        {
            transacao.Id = Guid.NewGuid();
        }
        
        await _context.Transacoes.AddAsync(transacao);
        return transacao;
    }
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    public async Task<int> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

