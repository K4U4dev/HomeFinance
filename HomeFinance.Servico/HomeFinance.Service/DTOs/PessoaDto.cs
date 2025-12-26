namespace HomeFinance.Service.DTOs;

/// <summary>
/// DTO (Data Transfer Object) para representar uma pessoa nas operações da API.
/// Utilizado para transferir dados entre a camada de apresentação e a camada de serviço.
/// </summary>
public class PessoaDto
{
    /// <summary>
    /// Identificador único da pessoa.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa em anos.
    /// </summary>
    public int Idade { get; set; }
}

/// <summary>
/// DTO para criação de uma nova pessoa.
/// Não inclui o Id, pois será gerado automaticamente.
/// </summary>
public class CriarPessoaDto
{
    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Idade da pessoa em anos. Deve ser um número inteiro positivo.
    /// </summary>
    public int Idade { get; set; }
}

