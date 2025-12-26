using HomeFinance.Domain.Enums;

namespace HomeFinance.Service.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para representar uma categoria nas operações da API.
/// Utilizado para transferir dados entre a camada de apresentação e a camada de serviço.
/// </summary>
public class CategoriaDto
{
    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria (Despesa, Receita ou Ambas).
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
}

/// <summary>
/// DTO para criação de uma nova categoria.
/// Não inclui o Id, pois será gerado automaticamente.
/// </summary>
public class CriarCategoriaDto
{
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria (Despesa, Receita ou Ambas).
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
}

