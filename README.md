# HomeFinance

Sistema completo de controle de gastos residenciais desenvolvido com .NET 10.0 e React 18.

## Sobre o Projeto

HomeFinance é uma solução moderna para gerenciamento de finanças domésticas, permitindo o controle detalhado de receitas e despesas por pessoa e categoria. O sistema oferece uma interface intuitiva e uma API robusta para registro e consulta de transações financeiras.

## Funcionalidades Principais

- **Gestão de Pessoas**: Cadastro e controle de todos os membros da residência
- **Categorização**: Organização de gastos através de categorias customizáveis
- **Controle de Transações**: Registro completo de receitas e despesas
- **Relatórios Consolidados**: Visualização de totais por pessoa e por categoria
- **Dashboard Financeiro**: Visão geral do estado financeiro residencial
- **Validações Inteligentes**: Regras de negócio aplicadas automaticamente

## Arquitetura

O projeto é dividido em dois módulos principais:

### Backend - API REST (.NET 10.0)

Arquitetura em camadas com separação clara de responsabilidades:

```
HomeFinance.API/          # Controllers e endpoints REST
HomeFinance.Service/      # Lógica de negócio e validações
HomeFinance.Repository/   # Acesso a dados com Entity Framework Core
HomeFinance.Domain/       # Entidades e modelos de domínio
```

**Stack Tecnológico:**
- .NET 10.0
- Entity Framework Core 10.0
- PostgreSQL
- Swagger/OpenAPI

### Frontend - Interface Web (React 18)

Single Page Application com design responsivo:

```
src/
├── components/       # Componentes reutilizáveis
├── pages/           # Páginas da aplicação
├── services/        # Integração com API
├── hooks/           # Custom hooks React
└── styles/          # Sistema de temas e estilos
```

**Stack Tecnológico:**
- React 18 com TypeScript
- Vite
- React Router
- Axios
- CSS Modules

## Recursos Técnicos

### Regras de Negócio Implementadas

- **Compatibilidade de Categorias**: Validação automática entre tipo de transação e finalidade da categoria
- **Restrição por Idade**: Menores de 18 anos limitados a registro de despesas
- **Integridade Referencial**: Remoção em cascata de transações ao deletar pessoa
- **Validações de Entrada**: Verificação de campos obrigatórios e tipos de dados em todas as camadas

### Padrões e Boas Práticas

- **Clean Architecture**: Separação em camadas com baixo acoplamento
- **Repository Pattern**: Abstração da camada de dados
- **Dependency Injection**: Inversão de controle nativa do .NET
- **REST API**: Design consistente de endpoints
- **TypeScript**: Tipagem estática no frontend para maior segurança

## Requisitos

### Backend
- .NET SDK 10.0+
- PostgreSQL 12+

### Frontend
- Node.js 16+
- npm ou yarn

## Instalação e Execução

### Configuração do Banco de Dados


**Instalação Local:**
```sql
CREATE DATABASE HomeFinanceDB;
```

Configure a string de conexão em `HomeFinance.API/appsettings.json`

### Backend

```bash
# Navegar para o diretório da API
cd HomeFinance.API

# Restaurar dependências
dotnet restore

# Aplicar migrações (se necessário)
dotnet ef database update --project ../HomeFinance.Repository

# Executar aplicação
dotnet run
```

API disponível em: `https://localhost:7196` ou `http://localhost:5146`

Documentação Swagger: `https://localhost:7196/swagger/index.html`

### Frontend

```bash
# Navegar para o diretório da interface
cd HomeFinance.Interface

# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm run dev
```

Interface disponível em: `http://localhost:5173`

## Documentação Detalhada

- [README - API Backend](./HomeFinance.Servico/README.md)
- [README - Interface Frontend](./HomeFinance.Interface/README.md)

## Capturas de Tela

### Dashboard
![Dashboard](docs/images/dashboard.png)

### Gestão de Transações
![Transações](docs/images/transacoes.png)

### Relatórios
![Relatórios](docs/images/relatorios.png)

## Autor

Desenvolvido por [K4U4dev](https://github.com/K4U4dev)

---

**HomeFinance** - Controle financeiro simples e eficiente para sua casa.