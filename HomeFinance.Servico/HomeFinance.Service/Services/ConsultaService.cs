using HomeFinance.Domain.Enums;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Service.DTOs;
using HomeFinance.Service.Interfaces;

namespace HomeFinance.Service.Services;

/// <summary>
/// Serviço de consultas e relatórios financeiros.
/// Implementa a lógica para consultas agregadas de dados financeiros.
/// </summary>
public class ConsultaService : IConsultaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    
    /// <summary>
    /// Construtor que recebe os repositórios via injeção de dependência.
    /// </summary>
    /// <param name="pessoaRepository">Repositório para acesso aos dados de pessoas</param>
    /// <param name="transacaoRepository">Repositório para acesso aos dados de transações</param>
    /// <param name="categoriaRepository">Repositório para acesso aos dados de categorias</param>
    public ConsultaService(
        IPessoaRepository pessoaRepository,
        ITransacaoRepository transacaoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
        _categoriaRepository = categoriaRepository;
    }
    
    /// <summary>
    /// Consulta os totais financeiros agrupados por pessoa.
    /// Calcula o total de receitas, despesas e saldo de cada pessoa,
    /// além dos totais gerais de todas as pessoas.
    /// </summary>
    public async Task<ConsultaTotaisPorPessoaDto> ObterTotaisPorPessoaAsync()
    {
        // Obtém todas as pessoas
        var pessoas = await _pessoaRepository.ObterTodasAsync();
        
        // Obtém todas as transações
        var transacoes = await _transacaoRepository.ObterTodasAsync();
        
        // Calcula os totais por pessoa
        var totaisPorPessoa = pessoas.Select(pessoa =>
        {
            var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();
            
            var totalReceitas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);
            
            var totalDespesas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);
            
            var saldo = totalReceitas - totalDespesas;
            
            return new TotaisPorPessoaDto
            {
                PessoaId = pessoa.Id,
                PessoaNome = pessoa.Nome,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = saldo
            };
        }).ToList();
        
        // Calcula os totais gerais
        var totalGeralReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalGeralDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas);
        var saldoLiquidoGeral = totalGeralReceitas - totalGeralDespesas;
        
        // Retorna o resultado completo
        return new ConsultaTotaisPorPessoaDto
        {
            TotaisPorPessoa = totaisPorPessoa,
            TotalGeralReceitas = totalGeralReceitas,
            TotalGeralDespesas = totalGeralDespesas,
            SaldoLiquidoGeral = saldoLiquidoGeral
        };
    }
    
    /// <summary>
    /// Consulta os totais financeiros agrupados por categoria.
    /// Calcula o total de receitas, despesas e saldo de cada categoria,
    /// além dos totais gerais de todas as categorias.
    /// </summary>
    public async Task<ConsultaTotaisPorCategoriaDto> ObterTotaisPorCategoriaAsync()
    {
        // Obtém todas as categorias
        var categorias = await _categoriaRepository.ObterTodasAsync();
        
        // Obtém todas as transações
        var transacoes = await _transacaoRepository.ObterTodasAsync();
        
        // Calcula os totais por categoria
        var totaisPorCategoria = categorias.Select(categoria =>
        {
            var transacoesDaCategoria = transacoes.Where(t => t.CategoriaId == categoria.Id).ToList();
            
            var totalReceitas = transacoesDaCategoria
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);
            
            var totalDespesas = transacoesDaCategoria
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);
            
            var saldo = totalReceitas - totalDespesas;
            
            return new TotaisPorCategoriaDto
            {
                CategoriaId = categoria.Id,
                CategoriaDescricao = categoria.Descricao,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = saldo
            };
        }).ToList();
        
        // Calcula os totais gerais
        var totalGeralReceitas = totaisPorCategoria.Sum(t => t.TotalReceitas);
        var totalGeralDespesas = totaisPorCategoria.Sum(t => t.TotalDespesas);
        var saldoLiquidoGeral = totalGeralReceitas - totalGeralDespesas;
        
        // Retorna o resultado completo
        return new ConsultaTotaisPorCategoriaDto
        {
            TotaisPorCategoria = totaisPorCategoria,
            TotalGeralReceitas = totalGeralReceitas,
            TotalGeralDespesas = totalGeralDespesas,
            SaldoLiquidoGeral = saldoLiquidoGeral
        };
    }
}

