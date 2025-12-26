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
/// Testes unitários para o serviço de transações.
/// Testa todas as validações e regras de negócio relacionadas a transações,
/// incluindo a regra crítica de menores de 18 anos só poderem ter despesas.
/// </summary>
public class TransacaoServiceTests
{
    private readonly Mock<ITransacaoRepository> _mockTransacaoRepository;
    private readonly Mock<IPessoaRepository> _mockPessoaRepository;
    private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
    private readonly TransacaoService _service;
    
    public TransacaoServiceTests()
    {
        _mockTransacaoRepository = new Mock<ITransacaoRepository>();
        _mockPessoaRepository = new Mock<IPessoaRepository>();
        _mockCategoriaRepository = new Mock<ICategoriaRepository>();
        
        _service = new TransacaoService(
            _mockTransacaoRepository.Object,
            _mockPessoaRepository.Object,
            _mockCategoriaRepository.Object);
    }
    
    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveCriarTransacao()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Compra no supermercado",
            Valor = 150.50m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        var transacaoCriada = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Compra no supermercado",
            Valor = 150.50m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Categoria = categoria,
            Pessoa = pessoa
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        _mockTransacaoRepository.Setup(r => r.AdicionarAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(transacaoCriada);
        _mockTransacaoRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        _mockTransacaoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transacaoCriada);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Descricao.Should().Be("Compra no supermercado");
        resultado.Valor.Should().Be(150.50m);
        resultado.Tipo.Should().Be(TipoTransacao.Despesa);
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Once);
        _mockTransacaoRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CriarAsync_ComDescricaoVazia_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComValorZero_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 0m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComValorNegativo_DeveLancarArgumentException()
    {
        // Arrange
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = -100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComPessoaInexistente_DeveLancarArgumentException()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync((Pessoa?)null);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        exception.Message.Should().Contain("não encontrada");
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComCategoriaInexistente_DeveLancarArgumentException()
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync((Categoria?)null);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));
        exception.Message.Should().Contain("não encontrada");
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_MenorDe18AnosComReceita_DeveLancarInvalidOperationException()
    {
        // Arrange - REGRA DE NEGÓCIO CRÍTICA: Menores de 18 só podem ter despesas
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "Maria", Idade = 17 }; // Menor de idade
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Salário", 
            Finalidade = FinalidadeCategoria.Receita 
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Salário",
            Valor = 1000m,
            Tipo = TipoTransacao.Receita, // Tentando criar receita para menor
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));
        exception.Message.Should().Contain("menores de 18 anos");
        exception.Message.Should().Contain("não podem ter receitas");
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_MenorDe18AnosComDespesa_DevePermitir()
    {
        // Arrange - Menor de idade pode ter despesas
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "Maria", Idade = 17 };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Lanche",
            Valor = 25m,
            Tipo = TipoTransacao.Despesa, // Despesa permitida para menor
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        var transacaoCriada = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Lanche",
            Valor = 25m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Categoria = categoria,
            Pessoa = pessoa
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        _mockTransacaoRepository.Setup(r => r.AdicionarAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(transacaoCriada);
        _mockTransacaoRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        _mockTransacaoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transacaoCriada);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        resultado.Tipo.Should().Be(TipoTransacao.Despesa);
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Once);
    }
    
    [Fact]
    public async Task CriarAsync_ComCategoriaIncompativelDespesa_DeveLancarInvalidOperationException()
    {
        // Arrange - Categoria de receita não pode ser usada em despesa
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Salário", 
            Finalidade = FinalidadeCategoria.Receita // Categoria só para receitas
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa, // Tentando usar categoria de receita em despesa
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));
        exception.Message.Should().Contain("não pode ser utilizada");
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComCategoriaIncompativelReceita_DeveLancarInvalidOperationException()
    {
        // Arrange - Categoria de despesa não pode ser usada em receita
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa // Categoria só para despesas
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Receita, // Tentando usar categoria de despesa em receita
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));
        exception.Message.Should().Contain("não pode ser utilizada");
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Never);
    }
    
    [Fact]
    public async Task CriarAsync_ComCategoriaAmbas_DevePermitirQualquerTipo()
    {
        // Arrange - Categoria "Ambas" pode ser usada para qualquer tipo
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 25 };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Outros", 
            Finalidade = FinalidadeCategoria.Ambas 
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        var transacaoCriada = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Categoria = categoria,
            Pessoa = pessoa
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        _mockTransacaoRepository.Setup(r => r.AdicionarAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(transacaoCriada);
        _mockTransacaoRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        _mockTransacaoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transacaoCriada);
        
        // Act
        var resultado = await _service.CriarAsync(dto);
        
        // Assert
        resultado.Should().NotBeNull();
        _mockTransacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Transacao>()), Times.Once);
    }
    
    [Theory]
    [InlineData(18, TipoTransacao.Receita, true)] // 18 anos pode ter receita
    [InlineData(19, TipoTransacao.Receita, true)] // Maior de 18 pode ter receita
    [InlineData(17, TipoTransacao.Receita, false)] // Menor de 18 não pode ter receita
    [InlineData(17, TipoTransacao.Despesa, true)] // Menor de 18 pode ter despesa
    [InlineData(18, TipoTransacao.Despesa, true)] // Maior de 18 pode ter despesa
    public async Task CriarAsync_ValidarRegraIdadeETipo_DeveRespeitarRegras(int idade, TipoTransacao tipo, bool devePermitir)
    {
        // Arrange
        var pessoaId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var pessoa = new Pessoa { Id = pessoaId, Nome = "Teste", Idade = idade };
        var categoria = new Categoria 
        { 
            Id = categoriaId, 
            Descricao = "Teste", 
            Finalidade = tipo == TipoTransacao.Despesa 
                ? FinalidadeCategoria.Despesa 
                : FinalidadeCategoria.Receita
        };
        
        var dto = new CriarTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = tipo,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };
        
        var transacaoCriada = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Teste",
            Valor = 100m,
            Tipo = tipo,
            CategoriaId = categoriaId,
            PessoaId = pessoaId,
            Categoria = categoria,
            Pessoa = pessoa
        };
        
        _mockPessoaRepository.Setup(r => r.ObterPorIdAsync(pessoaId))
            .ReturnsAsync(pessoa);
        _mockCategoriaRepository.Setup(r => r.ObterPorIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        _mockTransacaoRepository.Setup(r => r.AdicionarAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(transacaoCriada);
        _mockTransacaoRepository.Setup(r => r.SalvarAlteracoesAsync())
            .ReturnsAsync(1);
        _mockTransacaoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transacaoCriada);
        
        // Act & Assert
        if (devePermitir)
        {
            var resultado = await _service.CriarAsync(dto);
            resultado.Should().NotBeNull();
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));
        }
    }
}

