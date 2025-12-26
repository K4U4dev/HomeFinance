using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Data;
using HomeFinance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Repository.Repositories;

/// <summary>
/// Implementação do repositório para operações relacionadas à entidade Pessoa.
/// Fornece métodos para acesso e manipulação de dados de pessoas no banco de dados.
/// </summary>
public class PessoaRepository : IPessoaRepository
{
    private readonly HomeFinanceDbContext _context;
    
    /// <summary>
    /// Construtor que recebe o contexto do Entity Framework.
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public PessoaRepository(HomeFinanceDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    public async Task<IEnumerable<Pessoa>> ObterTodasAsync()
    {
        return await _context.Pessoas
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
    
    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador único.
    /// </summary>
    public async Task<Pessoa?> ObterPorIdAsync(Guid id)
    {
        return await _context.Pessoas
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    /// <summary>
    /// Adiciona uma nova pessoa ao banco de dados.
    /// </summary>
    public async Task<Pessoa> AdicionarAsync(Pessoa pessoa)
    {
        // Gera um novo Guid se não foi fornecido
        if (pessoa.Id == Guid.Empty)
        {
            pessoa.Id = Guid.NewGuid();
        }
        
        await _context.Pessoas.AddAsync(pessoa);
        return pessoa;
    }
    
    /// <summary>
    /// Remove uma pessoa do banco de dados.
    /// As transações associadas serão removidas automaticamente devido ao cascade delete.
    /// </summary>
    public async Task RemoverAsync(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
        await Task.CompletedTask;
    }
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    public async Task<int> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

