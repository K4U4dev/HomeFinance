using FluentAssertions;
using HomeFinance.API.Controllers;
using HomeFinance.Domain.Enums;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HomeFinance.Tests.Controllers;

/// <summary>
/// Testes unitários para o controller de transações.
/// Testa os endpoints da API e tratamento de erros, incluindo regras de negócio.
/// </summary>
public class TransacoesControllerTests
{
    private readonly Mock<ITransacaoService> _mockService;
    private readonly Mock<ILogger<TransacoesController>> _mockLogger;
    private readonly TransacoesController _controller;
    
    public TransacoesControllerTests()
    {
        _mockService = new Mock<ITransacaoService>();
        _mockLogger = new Mock<ILogger<TransacoesController>>();
        _controller = new TransacoesController(_mockService.Object, _mockLogger.Object);
    }
    
    [Fact]
    public async Task Criar_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "Compra",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        var transacaoCriada = new TransacaoDto
        {
            Id = Guid.NewGuid(),
            Descricao = "Compra",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = dto.CategoriaId,
            PessoaId = dto.PessoaId
        };
        
        _mockService.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(transacaoCriada);
        
        // Act
        var resultado = await _controller.Criar(dto);
        
        // Assert
        var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().BeOfType<TransacaoDto>();
    }
    
    [Fact]
    public async Task Criar_ComMenorDe18AnosTentandoReceita_DeveRetornarBadRequest()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "Salário",
            Valor = 1000m,
            Tipo = TipoTransacao.Receita,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        _mockService.Setup(s => s.CriarAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Menores de 18 anos não podem ter receitas"));
        
        // Act
        var resultado = await _controller.Criar(dto);
        
        // Assert
        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task Criar_ComCategoriaIncompativel_DeveRetornarBadRequest()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        _mockService.Setup(s => s.CriarAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Categoria incompatível"));
        
        // Act
        var resultado = await _controller.Criar(dto);
        
        // Assert
        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}

