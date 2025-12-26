using FluentAssertions;
using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.Services;
using Moq;
using Xunit;

namespace HomeFinance.Tests.Services;

/// <summary>
/// Testes unitários para o serviço de consultas.
/// Testa os cálculos de totais por pessoa e por categoria.
/// </summary>
public class ConsultaServiceTests
{
    private readonly Mock<IPessoaRepository> _mockPessoaRepository;
    private readonly Mock<ITransacaoRepository> _mockTransacaoRepository;
    private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
    private readonly ConsultaService _service;
    
    public ConsultaServiceTests()
    {
        _mockPessoaRepository = new Mock<IPessoaRepository>();
        _mockTransacaoRepository = new Mock<ITransacaoRepository>();
        _mockCategoriaRepository = new Mock<ICategoriaRepository>();
        
        _service = new ConsultaService(
            _mockPessoaRepository.Object,
            _mockTransacaoRepository.Object,
            _mockCategoriaRepository.Object);
    }
    
    [Fact]
    public async Task ObterTotaisPorPessoaAsync_DeveCalcularTotaisCorretamente()
    {
        // Arrange
        var pessoa1Id = Guid.NewGuid();
        var pessoa2Id = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        
        var pessoas = new List<Pessoa>
        {
            new() { Id = pessoa1Id, Nome = "João", Idade = 25 },
            new() { Id = pessoa2Id, Nome = "Maria", Idade = 30 }
        };
        
        var transacoes = new List<Transacao>
        {
            // Pessoa 1: Receita 5000, Despesa 150.50 = Saldo 4849.50
            new() 
            { 
                Id = Guid.NewGuid(), 
                PessoaId = pessoa1Id, 
                Tipo = TipoTransacao.Receita, 
                Valor = 5000m 
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                PessoaId = pessoa1Id, 
                Tipo = TipoTransacao.Despesa, 
                Valor = 150.50m 
            },
            // Pessoa 2: Receita 3000, Despesa 500 = Saldo 2500
            new() 
            { 
                Id = Guid.NewGuid(), 
                PessoaId = pessoa2Id, 
                Tipo = TipoTransacao.Receita, 
                Valor = 3000m 
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                PessoaId = pessoa2Id, 
                Tipo = TipoTransacao.Despesa, 
                Valor = 500m 
            }
        };
        
        _mockPessoaRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(pessoas);
        _mockTransacaoRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(transacoes);
        
        // Act
        var resultado = await _service.ObterTotaisPorPessoaAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.TotaisPorPessoa.Should().HaveCount(2);
        
        var pessoa1 = resultado.TotaisPorPessoa.First(p => p.PessoaId == pessoa1Id);
        pessoa1.TotalReceitas.Should().Be(5000m);
        pessoa1.TotalDespesas.Should().Be(150.50m);
        pessoa1.Saldo.Should().Be(4849.50m);
        
        var pessoa2 = resultado.TotaisPorPessoa.First(p => p.PessoaId == pessoa2Id);
        pessoa2.TotalReceitas.Should().Be(3000m);
        pessoa2.TotalDespesas.Should().Be(500m);
        pessoa2.Saldo.Should().Be(2500m);
        
        // Totais gerais
        resultado.TotalGeralReceitas.Should().Be(8000m);
        resultado.TotalGeralDespesas.Should().Be(650.50m);
        resultado.SaldoLiquidoGeral.Should().Be(7349.50m);
    }
    
    [Fact]
    public async Task ObterTotaisPorPessoaAsync_ComPessoaSemTransacoes_DeveRetornarZeros()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var pessoas = new List<Pessoa>
        {
            new() { Id = pessoaId, Nome = "João", Idade = 25 }
        };
        
        var transacoes = new List<Transacao>(); // Nenhuma transação
        
        _mockPessoaRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(pessoas);
        _mockTransacaoRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(transacoes);
        
        // Act
        var resultado = await _service.ObterTotaisPorPessoaAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.TotaisPorPessoa.Should().HaveCount(1);
        
        var pessoa = resultado.TotaisPorPessoa.First();
        pessoa.TotalReceitas.Should().Be(0m);
        pessoa.TotalDespesas.Should().Be(0m);
        pessoa.Saldo.Should().Be(0m);
        
        resultado.TotalGeralReceitas.Should().Be(0m);
        resultado.TotalGeralDespesas.Should().Be(0m);
        resultado.SaldoLiquidoGeral.Should().Be(0m);
    }
    
    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_DeveCalcularTotaisCorretamente()
    {
        // Arrange
        var categoria1Id = Guid.NewGuid();
        var categoria2Id = Guid.NewGuid();
        
        var categorias = new List<Categoria>
        {
            new() { Id = categoria1Id, Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa },
            new() { Id = categoria2Id, Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita }
        };
        
        var transacoes = new List<Transacao>
        {
            // Categoria 1: Receita 0, Despesa 200 = Saldo -200
            new() 
            { 
                Id = Guid.NewGuid(), 
                CategoriaId = categoria1Id, 
                Tipo = TipoTransacao.Despesa, 
                Valor = 200m 
            },
            // Categoria 2: Receita 5000, Despesa 0 = Saldo 5000
            new() 
            { 
                Id = Guid.NewGuid(), 
                CategoriaId = categoria2Id, 
                Tipo = TipoTransacao.Receita, 
                Valor = 5000m 
            }
        };
        
        _mockCategoriaRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(categorias);
        _mockTransacaoRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(transacoes);
        
        // Act
        var resultado = await _service.ObterTotaisPorCategoriaAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.TotaisPorCategoria.Should().HaveCount(2);
        
        var categoria1 = resultado.TotaisPorCategoria.First(c => c.CategoriaId == categoria1Id);
        categoria1.TotalReceitas.Should().Be(0m);
        categoria1.TotalDespesas.Should().Be(200m);
        categoria1.Saldo.Should().Be(-200m);
        
        var categoria2 = resultado.TotaisPorCategoria.First(c => c.CategoriaId == categoria2Id);
        categoria2.TotalReceitas.Should().Be(5000m);
        categoria2.TotalDespesas.Should().Be(0m);
        categoria2.Saldo.Should().Be(5000m);
        
        // Totais gerais
        resultado.TotalGeralReceitas.Should().Be(5000m);
        resultado.TotalGeralDespesas.Should().Be(200m);
        resultado.SaldoLiquidoGeral.Should().Be(4800m);
    }
    
    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_ComCategoriaSemTransacoes_DeveRetornarZeros()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var categorias = new List<Categoria>
        {
            new() { Id = categoriaId, Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa }
        };
        
        var transacoes = new List<Transacao>(); // Nenhuma transação
        
        _mockCategoriaRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(categorias);
        _mockTransacaoRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(transacoes);
        
        // Act
        var resultado = await _service.ObterTotaisPorCategoriaAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.TotaisPorCategoria.Should().HaveCount(1);
        
        var categoria = resultado.TotaisPorCategoria.First();
        categoria.TotalReceitas.Should().Be(0m);
        categoria.TotalDespesas.Should().Be(0m);
        categoria.Saldo.Should().Be(0m);
        
        resultado.TotalGeralReceitas.Should().Be(0m);
        resultado.TotalGeralDespesas.Should().Be(0m);
        resultado.SaldoLiquidoGeral.Should().Be(0m);
    }
}

