using FluentAssertions;
using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Data;
using HomeFinance.Repository.Repositories;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Services;
using HomeFinance.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeFinance.Tests.Integration;

/// <summary>
/// Testes de integração end-to-end para transações.
/// Testa o fluxo completo desde o serviço até o banco de dados,
/// incluindo todas as regras de negócio críticas.
/// </summary>
public class TransacaoIntegrationTests : IDisposable
{
    private readonly HomeFinanceDbContext _context;
    private readonly PessoaRepository _pessoaRepository;
    private readonly CategoriaRepository _categoriaRepository;
    private readonly TransacaoRepository _transacaoRepository;
    private readonly PessoaService _pessoaService;
    private readonly CategoriaService _categoriaService;
    private readonly TransacaoService _transacaoService;
    
    public TransacaoIntegrationTests()
    {
        _context = DbContextHelper.CreateInMemoryContext($"IntegrationDb_{Guid.NewGuid()}");
        _pessoaRepository = new PessoaRepository(_context);
        _categoriaRepository = new CategoriaRepository(_context);
        _transacaoRepository = new TransacaoRepository(_context);
        
        _pessoaService = new PessoaService(_pessoaRepository);
        _categoriaService = new CategoriaService(_categoriaRepository);
        _transacaoService = new TransacaoService(
            _transacaoRepository,
            _pessoaRepository,
            _categoriaRepository);
        
        DbContextHelper.SeedTestData(_context);
    }
    
    [Fact]
    public async Task CriarTransacao_ComMenorDe18AnosEReceita_DeveFalhar()
    {
        // Arrange - Busca pessoa menor de idade dos dados de teste
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        var pessoaMenor = pessoas.First(p => p.Idade < 18);
        
        var categorias = await _categoriaRepository.ObterTodasAsync();
        var categoriaReceita = categorias.First(c => c.Finalidade == FinalidadeCategoria.Receita);
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Tentativa de receita",
            Valor = 1000m,
            Tipo = TipoTransacao.Receita,
            CategoriaId = categoriaReceita.Id,
            PessoaId = pessoaMenor.Id
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _transacaoService.CriarAsync(dto));
    }
    
    [Fact]
    public async Task CriarTransacao_ComMenorDe18AnosEDespesa_DevePermitir()
    {
        // Arrange - Menor de idade pode ter despesas
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        var pessoaMenor = pessoas.First(p => p.Idade < 18);
        
        var categorias = await _categoriaRepository.ObterTodasAsync();
        var categoriaDespesa = categorias.First(c => c.Finalidade == FinalidadeCategoria.Despesa);
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Lanche",
            Valor = 25m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaDespesa.Id,
            PessoaId = pessoaMenor.Id
        };
        
        // Act
        var resultado = await _transacaoService.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Tipo.Should().Be(TipoTransacao.Despesa);
        
        var transacaoNoBanco = await _transacaoRepository.ObterPorIdAsync(resultado.Id);
        transacaoNoBanco.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CriarTransacao_ComCategoriaIncompativel_DeveFalhar()
    {
        // Arrange
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        var pessoa = pessoas.First(p => p.Idade >= 18);
        
        var categorias = await _categoriaRepository.ObterTodasAsync();
        var categoriaReceita = categorias.First(c => c.Finalidade == FinalidadeCategoria.Receita);
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa, // Tentando usar categoria de receita em despesa
            CategoriaId = categoriaReceita.Id,
            PessoaId = pessoa.Id
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _transacaoService.CriarAsync(dto));
    }
    
    [Fact]
    public async Task RemoverPessoa_DeveRemoverTransacoesEmCascata()
    {
        // Arrange
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        var pessoa = pessoas.First();
        
        var transacoesAntes = await _transacaoRepository.ObterPorPessoaIdAsync(pessoa.Id);
        transacoesAntes.Should().NotBeEmpty(); // Deve ter transações
        
        // Act
        await _pessoaService.RemoverAsync(pessoa.Id);
        
        // Assert
        var pessoaRemovida = await _pessoaRepository.ObterPorIdAsync(pessoa.Id);
        pessoaRemovida.Should().BeNull();
        
        var transacoesDepois = await _transacaoRepository.ObterPorPessoaIdAsync(pessoa.Id);
        transacoesDepois.Should().BeEmpty(); // Cascade delete deve ter removido
    }
    
    [Fact]
    public async Task ObterTotaisPorPessoa_DeveCalcularCorretamente()
    {
        // Arrange
        var consultaService = new ConsultaService(
            _pessoaRepository,
            _transacaoRepository,
            _categoriaRepository);
        
        // Act
        var resultado = await consultaService.ObterTotaisPorPessoaAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.TotaisPorPessoa.Should().NotBeEmpty();
        
        // Verifica se os cálculos estão corretos
        foreach (var total in resultado.TotaisPorPessoa)
        {
            total.Saldo.Should().Be(total.TotalReceitas - total.TotalDespesas);
        }
        
        // Verifica totais gerais
        var totalReceitasCalculado = resultado.TotaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalDespesasCalculado = resultado.TotaisPorPessoa.Sum(t => t.TotalDespesas);
        
        resultado.TotalGeralReceitas.Should().Be(totalReceitasCalculado);
        resultado.TotalGeralDespesas.Should().Be(totalDespesasCalculado);
        resultado.SaldoLiquidoGeral.Should().Be(resultado.TotalGeralReceitas - resultado.TotalGeralDespesas);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}

