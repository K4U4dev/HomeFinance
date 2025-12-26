using HomeFinance.Domain.Enums;

namespace HomeFinance.Service.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para representar uma transação nas operações da API.
/// Utilizado para transferir dados entre a camada de apresentação e a camada de serviço.
/// </summary>
public class TransacaoDto
{
    /// <summary>
    /// Identificador único da transação.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Descrição da transação.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação.
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação (Despesa ou Receita).
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    public Guid CategoriaId { get; set; }
    
    /// <summary>
    /// Descrição da categoria associada (para facilitar visualização).
    /// </summary>
    public string? CategoriaDescricao { get; set; }
    
    /// <summary>
    /// Identificador da pessoa associada.
    /// </summary>
    public Guid PessoaId { get; set; }
    
    /// <summary>
    /// Nome da pessoa associada (para facilitar visualização).
    /// </summary>
    public string? PessoaNome { get; set; }
}

/// <summary>
/// DTO para criação de uma nova transação.
/// Não inclui o Id, pois será gerado automaticamente.
/// </summary>
public class CriarTransacaoDto
{
    /// <summary>
    /// Descrição da transação.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação. Deve ser um número decimal positivo.
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo da transação (Despesa ou Receita).
    /// </summary>
    public TipoTransacao Tipo { get; set; }
    
    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    public Guid CategoriaId { get; set; }
    
    /// <summary>
    /// Identificador da pessoa associada.
    /// </summary>
    public Guid PessoaId { get; set; }
}

