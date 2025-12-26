using FluentAssertions;
using HomeFinance.Domain.Entities;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Services;
using Moq;
using Xunit;

namespace HomeFinance.Tests.Services;

/// <summary>
/// Testes unitários para o serviço de pessoas.
/// Testa todas as validações e regras de negócio relacionadas a pessoas.
/// </summary>
public class PessoaServiceTests
{
    private readonly Mock<IPessoaRepository> _mockRepository;
    private readonly PessoaService _service;
    
    public PessoaServiceTests()
    {
        _mockRepository = new Mock<IPessoaRepository>();
        _service = new PessoaService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task ObterTodasAsync_DeveRetornarListaDePessoas()
    {
        // Arrange
        var pessoas = new List<Pessoa>
        {
            new() { Id = Guid.NewGuid(), Nome = "João", Idade = 25 },
            new() { Id = Guid.NewGuid(), Nome = "Maria", Idade = 30 }
        };
        
        _mockRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(pessoas);
        
        // Act
        var resultado = await _service.ObterTodasAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado.First().Nome.Should().Be("João");
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoPessoaExiste_DeveRetornarPessoa()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        
        // Act
        var resultado = await _service.ObterPorIdAsync(pessoaId);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(pessoaId);
        resultado.Nome.Should().Be("João");
        resultado.Idade.Should().Be(25);
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoPessoaNaoExiste_DeveRetornarNull()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync((Pessoa?)null);
        
        // Act
        var resultado = await _service.ObterPorIdAsync(pessoaId);
        
        // Assert
        resultado.Should().BeNull();
    }
    
    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveCriarPessoa()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "João Silva", Idade = 25 };
        var pessoaCriada = new Pessoa { Id = Guid.NewGuid(), Nome = "João Silva", Idade = 25 };
        
        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Pessoa>()))
            .ReturnsAsync(pessoaCriada);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("João Silva");
        resultado.Idade.Should().Be(25);
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Once);
        _mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CriarAsync_ComNomeVazio_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "", Idade = 25 };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComNomeNulo_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = null!, Idade = 25 };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComNomeApenasEspacos_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "   ", Idade = 25 };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComIdadeZero_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "João", Idade = 0 };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComIdadeNegativa_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "João", Idade = -1 };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pessoa>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComNomeComEspacosExtras_DeveRemoverEspacos()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "  João Silva  ", Idade = 25 };
        var pessoaCriada = new Pessoa { Id = Guid.NewGuid(), Nome = "João Silva", Idade = 25 };
        
        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Pessoa>()))
            .ReturnsAsync((Pessoa p) => pessoaCriada);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        _mockRepository.Verify(r => r.AdicionarAsync(It.Is<Pessoa>(p => p.Nome == "João Silva")), Times.Once);
    }
    
    [Fact]
    public async Task RemoverAsync_QuandoPessoaExiste_DeveRemoverPessoa()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.RemoverAsync(pessoaId);
        
        // Assert
        resultado.Should().BeTrue();
        _mockRepository.Verify(r => r.RemoverAsync(pessoa), Times.Once);
        _mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task RemoverAsync_QuandoPessoaNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync((Pessoa?)null);
        
        // Act
        var resultado = await _service.RemoverAsync(pessoaId);
        
        // Assert
        resultado.Should().BeFalse();
        _mockRepository.Verify(r => r.RemoverAsync(It.IsAny<Pessoa>()), Times.Never);
        _mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }
}

