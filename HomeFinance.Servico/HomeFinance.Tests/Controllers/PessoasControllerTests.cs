using FluentAssertions;
using HomeFinance.API.Controllers;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HomeFinance.Tests.Controllers;

/// <summary>
/// Testes unitários para o controller de pessoas.
/// Testa os endpoints da API e tratamento de erros.
/// </summary>
public class PessoasControllerTests
{
    private readonly Mock<IPessoaService> _mockService;
    private readonly Mock<ILogger<PessoasController>> _mockLogger;
    private readonly PessoasController _controller;
    
    public PessoasControllerTests()
    {
        _mockService = new Mock<IPessoaService>();
        _mockLogger = new Mock<ILogger<PessoasController>>();
        _controller = new PessoasController(_mockService.Object, _mockLogger.Object);
    }
    
    [Fact]
    public async Task ObterTodas_DeveRetornarOkComListaDePessoas()
    {
        // Arrange
        var pessoas = new List<PessoaDto>
        {
            new() { Id = Guid.NewGuid(), Nome = "João", Idade = 25 },
            new() { Id = Guid.NewGuid(), Nome = "Maria", Idade = 30 }
        };
        
        _mockService.Setup(s => s.ObterTodasAsync())
            .ReturnsAsync(pessoas);
        
        // Act
        var resultado = await _controller.ObterTodas();
        
        // Assert
        var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
        var pessoasRetornadas = okResult.Value.Should().BeAssignableTo<IEnumerable<PessoaDto>>().Subject;
        pessoasRetornadas.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task ObterPorId_QuandoPessoaExiste_DeveRetornarOk()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var pessoa = new PessoaDto { Id = pessoaId, Nome = "João", Idade = 25 };
        
        _mockService.Setup(s => s.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        
        // Act
        var resultado = await _controller.ObterPorId(pessoaId);
        
        // Assert
        var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
        var pessoaRetornada = okResult.Value.Should().BeOfType<PessoaDto>().Subject;
        pessoaRetornada.Id.Should().Be(pessoaId);
    }
    
    [Fact]
    public async Task ObterPorId_QuandoPessoaNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        _mockService.Setup(s => s.ObterPorIdAsync(pessoaId))
            .ReturnsAsync((PessoaDto?)null);
        
        // Act
        var resultado = await _controller.ObterPorId(pessoaId);
        
        // Assert
        resultado.Result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Fact]
    public async Task Criar_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "João", Idade = 25 };
        var pessoaCriada = new PessoaDto { Id = Guid.NewGuid(), Nome = "João", Idade = 25 };
        
        _mockService.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(pessoaCriada);
        
        // Act
        var resultado = await _controller.Criar(dto);
        
        // Assert
        var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().BeOfType<PessoaDto>();
    }
    
    [Fact]
    public async Task Criar_ComDadosInvalidos_DeveRetornarBadRequest()
    {
        // Arrange
        var dto = new CriarPessoaDto { Nome = "", Idade = 25 };
        
        _mockService.Setup(s => s.CriarAsync(dto))
            .ThrowsAsync(new ArgumentException("Nome inválido"));
        
        // Act
        var resultado = await _controller.Criar(dto);
        
        // Assert
        resultado.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task Remover_QuandoPessoaExiste_DeveRetornarNoContent()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        _mockService.Setup(s => s.RemoverAsync(pessoaId))
            .ReturnsAsync(true);
        
        // Act
        var resultado = await _controller.Remover(pessoaId);
        
        // Assert
        resultado.Should().BeOfType<NoContentResult>();
    }
    
    [Fact]
    public async Task Remover_QuandoPessoaNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        
        _mockService.Setup(s => s.RemoverAsync(pessoaId))
            .ReturnsAsync(false);
        
        // Act
        var resultado = await _controller.Remover(pessoaId);
        
        // Assert
        resultado.Should().BeOfType<NotFoundObjectResult>();
    }
}

