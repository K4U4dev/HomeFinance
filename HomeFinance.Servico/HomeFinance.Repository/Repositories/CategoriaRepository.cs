using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Data;
using HomeFinance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Repository.Repositories;

/// <summary>
/// Implementação do repositório para operações relacionadas à entidade Categoria.
/// Fornece métodos para acesso e manipulação de dados de categorias no banco de dados.
/// </summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly HomeFinanceDbContext _context;
    
    /// <summary>
    /// Construtor que recebe o contexto do Entity Framework.
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public CategoriaRepository(HomeFinanceDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    public async Task<IEnumerable<Categoria>> ObterTodasAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Descricao)
            .ToListAsync();
    }
    
    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador único.
    /// </summary>
    public async Task<Categoria?> ObterPorIdAsync(Guid id)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    /// <summary>
    /// Adiciona uma nova categoria ao banco de dados.
    /// </summary>
    public async Task<Categoria> AdicionarAsync(Categoria categoria)
    {
        // Gera um novo Guid se não foi fornecido
        if (categoria.Id == Guid.Empty)
        {
            categoria.Id = Guid.NewGuid();
        }
        
        await _context.Categorias.AddAsync(categoria);
        return categoria;
    }
    
    /// <summary>
    /// Salva todas as alterações pendentes no banco de dados.
    /// </summary>
    public async Task<int> SalvarAlteracoesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

