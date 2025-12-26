using FluentAssertions;
using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Services;
using Moq;
using Xunit;

namespace HomeFinance.Tests.Services;

/// <summary>
/// Testes unitários para o serviço de categorias.
/// Testa todas as validações e regras de negócio relacionadas a categorias.
/// </summary>
public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _mockRepository;
    private readonly CategoriaService _service;
    
    public CategoriaServiceTests()
    {
        _mockRepository = new Mock<ICategoriaRepository>();
        _service = new CategoriaService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task ObterTodasAsync_DeveRetornarListaDeCategorias()
    {
        // Arrange
        var categorias = new List<Categoria>
        {
            new() { Id = Guid.NewGuid(), Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa },
            new() { Id = Guid.NewGuid(), Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita }
        };
        
        _mockRepository.Setup(r => r.ObterTodasAsync())
            .ReturnsAsync(categorias);
        
        // Act
        var resultado = await _service.ObterTodasAsync();
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado.First().Descricao.Should().Be("Alimentação");
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoCategoriaExiste_DeveRetornarCategoria()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        
        // Act
        var resultado = await _service.ObterPorIdAsync(categoriaId);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(categoriaId);
        resultado.Descricao.Should().Be("Alimentação");
        resultado.Finalidade.Should().Be(FinalidadeCategoria.Despesa);
    }
    
    [Fact]
    public async Task ObterPorIdAsync_QuandoCategoriaNaoExiste_DeveRetornarNull()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        
        _mockRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync((Categoria?)null);
        
        // Act
        var resultado = await _service.ObterPorIdAsync(categoriaId);
        
        // Assert
        resultado.Should().BeNull();
    }
    
    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveCriarCategoria()
    {
        // Arrange
        var dto = new CriarCategoriaDto 
        { 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        var categoriaCriada = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Categoria>()))
            .ReturnsAsync(categoriaCriada);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Descricao.Should().Be("Alimentação");
        resultado.Finalidade.Should().Be(FinalidadeCategoria.Despesa);
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Once);
        _mockRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CriarAsync_ComDescricaoVazia_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarCategoriaDto { Descricao = "", Finalidade = FinalidadeCategoria.Despesa };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComDescricaoNula_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarCategoriaDto { Descricao = null!, Finalidade = FinalidadeCategoria.Despesa };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComDescricaoApenasEspacos_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarCategoriaDto { Descricao = "   ", Finalidade = FinalidadeCategoria.Despesa };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
    }
    
    [Theory]
    [InlineData(FinalidadeCategoria.Despesa)]
    [InlineData(FinalidadeCategoria.Receita)]
    [InlineData(FinalidadeCategoria.Ambas)]
    public async Task CriarAsync_ComFinalidadesValidas_DeveCriarCategoria(FinalidadeCategoria finalidade)
    {
        // Arrange
        var dto = new CriarCategoriaDto { Descricao = "Teste", Finalidade = finalidade };
        var categoriaCriada = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Teste", 
            Finalidade = finalidade 
        };
        
        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Categoria>()))
            .ReturnsAsync(categoriaCriada);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Finalidade.Should().Be(finalidade);
    }
    
    [Fact]
    public async Task CriarAsync_ComDescricaoComEspacosExtras_DeveRemoverEspacos()
    {
        // Arrange
        var dto = new CriarCategoriaDto 
        { 
            Descricao = "  Alimentação  ", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        var categoriaCriada = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Categoria>()))
            .ReturnsAsync((Categoria c) => categoriaCriada);
        _mockRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        _mockRepository.Verify(r => r.AdicionarAsync(It.Is<Categoria>(c => c.Descricao == "Alimentação")), Times.Once);
    }
}

