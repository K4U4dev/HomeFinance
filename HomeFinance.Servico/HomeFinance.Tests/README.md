# HomeFinance.Tests

Projeto de testes unitários e de integração para o sistema HomeFinance.

## 📋 Visão Geral

Este projeto contém uma suíte completa de testes que cobre todas as funcionalidades e regras de negócio do sistema, garantindo qualidade e confiabilidade do código.

## 🧪 Estrutura de Testes

### Testes Unitários de Serviços

- **PessoaServiceTests**: Testa validações e operações de pessoas
  - Validação de nome obrigatório
  - Validação de idade positiva
  - Criação e remoção de pessoas
  - Tratamento de espaços em branco

- **CategoriaServiceTests**: Testa validações e operações de categorias
  - Validação de descrição obrigatória
  - Validação de finalidade (Despesa, Receita, Ambas)
  - Criação de categorias

- **TransacaoServiceTests**: Testa todas as regras de negócio críticas
  - Validação de descrição e valor positivo
  - Validação de pessoa e categoria existentes
  - **Regra crítica: Menores de 18 anos só podem ter despesas**
  - Validação de compatibilidade categoria/tipo de transação
  - Testes parametrizados para diferentes idades e tipos

- **ConsultaServiceTests**: Testa cálculos agregados
  - Totais por pessoa (receitas, despesas, saldo)
  - Totais por categoria
  - Totais gerais
  - Tratamento de casos sem transações

### Testes de Repositórios

- **PessoaRepositoryTests**: Testa operações de banco de dados
  - CRUD completo
  - Cascade delete (remover pessoa remove transações)

### Testes de Controllers

- **PessoasControllerTests**: Testa endpoints da API
  - GET, POST, DELETE
  - Tratamento de erros HTTP
  - Códigos de status corretos

- **TransacoesControllerTests**: Testa endpoints de transações
  - Validação de regras de negócio via API
  - Tratamento de exceções

### Testes de Integração

- **TransacaoIntegrationTests**: Testes end-to-end
  - Fluxo completo serviço → repositório → banco
  - Validação de regras de negócio em ambiente real
  - Cascade delete funcional
  - Cálculos agregados corretos

## 🎯 Cobertura de Testes

### Regras de Negócio Testadas

✅ **Validações de Pessoa**
- Nome obrigatório e não vazio
- Idade deve ser positiva
- Remoção de espaços em branco

✅ **Validações de Categoria**
- Descrição obrigatória
- Finalidade válida (Despesa, Receita, Ambas)

✅ **Validações de Transação**
- Descrição obrigatória
- Valor deve ser positivo
- Pessoa deve existir
- Categoria deve existir
- **Menores de 18 anos só podem ter despesas** ⚠️ REGRA CRÍTICA
- Compatibilidade categoria/tipo de transação

✅ **Cálculos Agregados**
- Totais por pessoa
- Totais por categoria
- Totais gerais
- Saldos corretos

✅ **Cascade Delete**
- Remover pessoa remove transações automaticamente

## 🚀 Executando os Testes

### Executar todos os testes

```bash
dotnet test
```

### Executar com detalhes

```bash
dotnet test --verbosity normal
```

### Executar testes específicos

```bash
dotnet test --filter "FullyQualifiedName~TransacaoServiceTests"
```

### Executar com cobertura de código

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📊 Estatísticas

- **Total de Testes**: 64
- **Taxa de Sucesso**: 100%
- **Tipos de Teste**:
  - Testes Unitários: ~50
  - Testes de Integração: ~14

## 🛠️ Tecnologias Utilizadas

- **xUnit**: Framework de testes
- **Moq**: Framework de mocking
- **FluentAssertions**: Assertions mais legíveis
- **Entity Framework InMemory**: Banco em memória para testes
- **Microsoft.AspNetCore.Mvc.Testing**: Testes de controllers

## 📝 Convenções

- Nomes de testes seguem o padrão: `Metodo_Cenario_ResultadoEsperado`
- Testes são organizados por camada (Services, Controllers, Repositories, Integration)
- Cada teste é independente e isolado
- Testes de integração usam banco em memória para isolamento

## ✅ Garantias

Os testes garantem que:

1. Todas as validações de entrada funcionam corretamente
2. Regras de negócio críticas são respeitadas (especialmente menores de 18)
3. Operações de banco de dados funcionam corretamente
4. Cascade delete funciona como esperado
5. Cálculos agregados são precisos
6. APIs retornam códigos HTTP corretos
7. Tratamento de erros funciona adequadamente

