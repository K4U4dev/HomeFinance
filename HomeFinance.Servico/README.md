# HomeFinance API

Sistema de controle de gastos residenciais desenvolvido em C# com .NET 10.0 e PostgreSQL.

## Descrição do Projeto

API RESTful completa para gerenciamento de finanças residenciais, permitindo cadastro de pessoas, categorias de gastos, registro de transações financeiras e geração de relatórios consolidados por pessoa e categoria.

## Arquitetura

O projeto implementa arquitetura em camadas com separação clara de responsabilidades e baixo acoplamento:

- **HomeFinance.Domain**: Contém entidades de domínio, enums e modelos
- **HomeFinance.Repository**: Implementa padrão Repository com Entity Framework Core
- **HomeFinance.Service**: Camada de serviços com lógica de negócio e validações
- **HomeFinance.API**: Camada de apresentação com controllers REST

## Stack Tecnológico

- **.NET 10.0**: Framework principal
- **Entity Framework Core 10.0**: ORM para acesso a dados
- **PostgreSQL**: Sistema de gerenciamento de banco de dados
- **Npgsql**: Provider .NET para PostgreSQL
- **Swagger/OpenAPI**: Documentação interativa da API

## Pré-requisitos

- .NET SDK 10.0 ou superior
- PostgreSQL 12 ou superior
- IDE: Visual Studio 2022, JetBrains Rider ou Visual Studio Code

## Configuração do Ambiente

### Configuração do Banco de Dados PostgreSQL

**Importante:** O PostgreSQL deve estar instalado e em execução antes de iniciar a aplicação.

#### Opção A: Instalação Local

1. Instale o PostgreSQL através do site oficial: https://www.postgresql.org/download/

2. Crie o banco de dados através do psql ou pgAdmin:
   ```sql
   CREATE DATABASE HomeFinanceDB;
   ```

3. Configure a string de conexão em `HomeFinance.API/appsettings.json` ou `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=HomeFinanceDB;Username=postgres;Password=SUA_SENHA"
     }
   }
   ```

#### Opção B: Container Docker (Recomendado)

Execute o seguinte comando para criar um container PostgreSQL:

```bash
docker run --name homefinance-postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=HomeFinanceDB \
  -p 5432:5432 \
  -d postgres:15
```

A string de conexão padrão do projeto já está configurada para esta opção.

Para informações detalhadas sobre a configuração do PostgreSQL, consulte: [SETUP_POSTGRESQL.md](SETUP_POSTGRESQL.md)

### Instalação de Dependências

```bash
dotnet restore
```

### Aplicação de Migrações

```bash
cd HomeFinance.API
dotnet ef migrations add InitialCreate --project ../HomeFinance.Repository
dotnet ef database update --project ../HomeFinance.Repository
```

Caso não possua o Entity Framework Tools instalado globalmente:

```bash
dotnet tool install --global dotnet-ef
```

**Nota:** Em ambiente de desenvolvimento, as migrações são aplicadas automaticamente na inicialização da aplicação.

## Execução da Aplicação

```bash
cd HomeFinance.API
dotnet run
```

A API estará disponível nos seguintes endereços:

- **HTTPS**: `https://localhost:5001` ou `https://localhost:7000`
- **HTTP**: `http://localhost:5000` ou `http://localhost:5001`

Documentação Swagger disponível em:
- `https://localhost:5001/swagger`

## Endpoints da API

### Pessoas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/pessoas` | Lista todas as pessoas cadastradas |
| GET | `/api/pessoas/{id}` | Retorna dados de uma pessoa específica |
| POST | `/api/pessoas` | Cadastra nova pessoa |
| DELETE | `/api/pessoas/{id}` | Remove pessoa e suas transações |

### Categorias

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/categorias/{id}` | Retorna dados de uma categoria |
| POST | `/api/categorias` | Cadastra nova categoria |

### Transações

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/transacoes` | Lista todas as transações |
| GET | `/api/transacoes/{id}` | Retorna dados de uma transação |
| POST | `/api/transacoes` | Registra nova transação |

### Consultas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/consultas/totais-por-pessoa` | Retorna totais agregados por pessoa |
| GET | `/api/consultas/totais-por-categoria` | Retorna totais agregados por categoria |

## Regras de Negócio

### Pessoas

- Nome é campo obrigatório e não pode ser vazio
- Idade deve ser número inteiro positivo
- Remoção em cascata: ao deletar pessoa, todas transações associadas são removidas

### Categorias

- Descrição é campo obrigatório e não pode ser vazia
- Finalidade pode assumir valores: Despesa (1), Receita (2) ou Ambas (3)
- Categorias com transações associadas não podem ser removidas

### Transações

- Descrição é campo obrigatório e não pode ser vazia
- Valor deve ser decimal positivo
- Pessoa e categoria devem existir no sistema
- Validação de compatibilidade categoria/tipo:
  - Categoria "Despesa" → apenas transações tipo Despesa
  - Categoria "Receita" → apenas transações tipo Receita
  - Categoria "Ambas" → aceita ambos os tipos
- **Restrição de idade**: Pessoas menores de 18 anos só podem ter transações do tipo Despesa

## Exemplos de Requisições

### Cadastrar Pessoa

```json
POST /api/pessoas
Content-Type: application/json

{
  "nome": "João Silva",
  "idade": 25
}
```

### Cadastrar Categoria

```json
POST /api/categorias
Content-Type: application/json

{
  "descricao": "Alimentação",
  "finalidade": 1
}
```

**Valores de Finalidade:**
- `1` - Despesa
- `2` - Receita
- `3` - Ambas

### Registrar Transação

```json
POST /api/transacoes
Content-Type: application/json

{
  "descricao": "Compra no supermercado",
  "valor": 150.50,
  "tipo": 1,
  "categoriaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "pessoaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Tipos de Transação:**
- `1` - Despesa
- `2` - Receita

## Testes da API

A API pode ser testada através de:

- **Swagger UI**: Interface interativa disponível em `/swagger`
- **Postman**: Importe a especificação OpenAPI
- **Insomnia**: Ferramenta alternativa para testes de API
- **curl**: Para testes via linha de comando

## Estrutura do Projeto

```
HomeFinance/
├── HomeFinance.API/
│   ├── Controllers/           # Endpoints REST
│   ├── Program.cs            # Configuração da aplicação
│   └── appsettings.json      # Configurações
├── HomeFinance.Service/
│   ├── Services/             # Lógica de negócio
│   └── Interfaces/           # Contratos de serviços
├── HomeFinance.Repository/
│   ├── Context/              # DbContext EF Core
│   ├── Repositories/         # Implementação repositories
│   └── Migrations/           # Migrações do banco
└── HomeFinance.Domain/
    ├── Entities/             # Entidades de domínio
    └── Enums/                # Enumeradores
```

## Aspectos de Segurança

- Configuração de CORS implementada para permitir requisições do frontend React
- Validação de entrada em todas as camadas
- Tratamento centralizado de exceções
- **Atenção**: Em ambiente de produção, ajuste as políticas de CORS para domínios específicos

## Observações Técnicas

O projeto foi desenvolvido seguindo boas práticas de engenharia de software:

- **Código Limpo**: Nomes descritivos e autoexplicativos
- **Modularização**: Separação clara de responsabilidades
- **Documentação**: Comentários XML em classes e métodos principais
- **Padrões**: Repository pattern, Dependency Injection, RESTful design
- **Validações**: Implementadas na camada de serviço
- **Baixo Acoplamento**: Uso de interfaces e injeção de dependências

## Autor

Desenvolvido por [K4U4dev](https://github.com/K4U4dev)

## Licença

Este projeto foi desenvolvido como parte da resolução de prova técnica do sistema HomeFinance.