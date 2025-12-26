using HomeFinance.Domain.Entities;
using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeFinance.Tests.Helpers;

/// <summary>
/// Classe auxiliar para criar um DbContext em memória para testes.
/// Facilita a criação de contextos de teste isolados.
/// </summary>
public static class DbContextHelper
{
    /// <summary>
    /// Cria um novo DbContext configurado para usar banco em memória.
    /// </summary>
    /// <param name="databaseName">Nome único para o banco em memória</param>
    /// <returns>DbContext configurado para testes</returns>
    public static HomeFinanceDbContext CreateInMemoryContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<HomeFinanceDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;
        
        return new HomeFinanceDbContext(options);
    }
    
    /// <summary>
    /// Cria dados de teste básicos no contexto fornecido.
    /// </summary>
    /// <param name="context">DbContext onde os dados serão inseridos</param>
    public static void SeedTestData(HomeFinanceDbContext context)
    {
        // Limpa o banco antes de inserir dados
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Cria pessoas de teste
        var pessoa1 = new Pessoa { Id = Guid.NewGuid(), Nome = "João Silva", Idade = 25 };
        var pessoa2 = new Pessoa { Id = Guid.NewGuid(), Nome = "Maria Santos", Idade = 17 }; // Menor de idade
        var pessoa3 = new Pessoa { Id = Guid.NewGuid(), Nome = "Pedro Oliveira", Idade = 30 };
        
        context.Pessoas.AddRange(pessoa1, pessoa2, pessoa3);
        
        // Cria categorias de teste
        var categoriaDespesa = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Alimentação", 
            Finalidade = FinalidadeCategoria.Despesa 
        };
        
        var categoriaReceita = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Salário", 
            Finalidade = FinalidadeCategoria.Receita 
        };
        
        var categoriaAmbas = new Categoria 
        { 
            Id = Guid.NewGuid(), 
            Descricao = "Outros", 
            Finalidade = FinalidadeCategoria.Ambas 
        };
        
        context.Categorias.AddRange(categoriaDespesa, categoriaReceita, categoriaAmbas);
        
        // Cria transações de teste
        var transacao1 = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Compra no supermercado",
            Valor = 150.50m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaDespesa.Id,
            PessoaId = pessoa1.Id
        };
        
        var transacao2 = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Salário mensal",
            Valor = 5000.00m,
            Tipo = TipoTransacao.Receita,
            CategoriaId = categoriaReceita.Id,
            PessoaId = pessoa1.Id
        };
        
        var transacao3 = new Transacao
        {
            Id = Guid.NewGuid(),
            Descricao = "Lanche",
            Valor = 25.00m,
            Tipo = TipoTransacao.Despesa,
            CategoriaId = categoriaDespesa.Id,
            PessoaId = pessoa2.Id
        };
        
        context.Transacoes.AddRange(transacao1, transacao2, transacao3);
        
        context.SaveChanges();
    }
}

