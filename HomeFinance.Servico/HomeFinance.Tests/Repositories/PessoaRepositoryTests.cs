using FluentAssertions;
using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Data;
using HomeFinance.Repository.Repositories;
using HomeFinance.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeFinance.Tests.Repositories;

/// <summary>
/// Testes de integração para o repositório de pessoas.
/// Testa as operações de banco de dados usando banco em memória.
/// </summary>
public class PessoaRepositoryTests : IDisposable
{
    private readonly HomeFinanceDbContext _context;
    private readonly PessoaRepository _repository;
    
    public PessoaRepositoryTests()
    {
        _context = DbContextHelper.CreateInMemoryContext($"PessoaDb_{Guid.NewGuid()}");
        _repository = new PessoaRepository(_context);
    }
    
    [Fact]
    public async Task ObterTodasAsync_DeveRetornarTodasAsPessoas()
    {
        // Arrange
        var pessoa1 = new Pessoa { Id = Guid.NewGuid(), Nome = "João", Idade = 25 };
        var pessoa2 = new Pessoa { Id = Guid.NewGuid(), Nome = "Maria", Idade = 30 };
        
        _context.Pessoas.AddRange(pessoa1, pessoa2);
        await _context.SaveChangesAsync();
        
        // Act
        var resultado = await _repository.ObterTodasAsync();
        
        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().Contain(p => p.Nome == "João");
        resultado.Should().Contain(p => p.Nome == "Maria");
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoPessoaExiste_DeveRetornarPessoa()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        
        // Act
        var resultado = await _repository.ObterPorIdAsync(pessoaId);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(pessoaId);
        resultado.Nome.Should().Be("João");
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoPessoaNaoExiste_DeveRetornarNull()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        // Act
        var resultado = await _repository.ObterPorIdAsync(pessoaId);
        
        // Assert
        resultado.Should().BeNull();
    }
    
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarPessoa()
    {
        // Arrange
        var pessoa = new Pessoa { Id = Guid.NewGuid(), Nome = "João", Idade = 25 };
        
        // Act
        await _repository.AdicionarAsync(pessoa);
        await _repository.SalvarAlteracoesAsync();
        
        // Assert
        var pessoaNoBanco = await _context.Pessoas.FindAsync(pessoa.Id);
        pessoaNoBanco.Should().NotBeNull();
        pessoaNoBanco!.Nome.Should().Be("João");
    }
    
    [Fact]
    public async Task RemoverAsync_DeveRemoverPessoa()
    {
        // Arrange
        var pessoa = new Pessoa { Id = Guid.NewGuid(), Nome = "João", Idade = 25 };
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        
        // Act
        await _repository.RemoverAsync(pessoa);
        await _repository.SalvarAlteracoesAsync();
        
        // Assert
        var pessoaNoBanco = await _context.Pessoas.FindAsync(pessoa.Id);
        pessoaNoBanco.Should().BeNull();
    }
    
    [Fact]
    public async Task RemoverAsync_DeveRemoverTransacoesEmCascata()
    {
        // Arrange - Testa o cascade delete
        var pessoa = new Pessoa { Id = Guid.NewGuid(), Nome = "João", Idade = 25 };
        var categoria = new Domain.Entities.Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Teste", 
            Finalidade = Domain.Enums.FinalidadeCategoria.Despesa 
        };
        var transacao = new Domain.Entities.Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Teste",
            Valor = 100m,
            Tipo = Domain.Enums.TipoTransacao.Despesa,
            CategoriaId = categoria.Id,
            PessoaId = pessoa.Id
        };
        
        _context.Pessoas.Add(pessoa);
        _context.Categorias.Add(categoria);
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        
        // Act
        await _repository.RemoverAsync(pessoa);
        await _repository.SalvarAlteracoesAsync();
        
        // Assert
        var pessoaNoBanco = await _context.Pessoas.FindAsync(pessoa.Id);
        pessoaNoBanco.Should().BeNull();
        
        var transacaoNoBanco = await _context.Transacoes.FindAsync(transacao.Id);
        transacaoNoBanco.Should().BeNull(); // Cascade delete deve ter removido
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}

