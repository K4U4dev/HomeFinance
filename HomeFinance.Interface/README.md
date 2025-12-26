# HomeFinance Interface

Interface React moderna e responsiva para o sistema de controle de gastos residenciais HomeFinance.

## Características Principais

O sistema oferece uma experiência de usuário completa e profissional:

- **Design Minimalista**: Interface limpa e focada na usabilidade
- **Sistema de Temas**: Alternância entre tema claro e escuro com persistência de preferências
- **Responsividade Completa**: Adaptação automática para dispositivos móveis, tablets e desktop
- **Página de Apresentação**: Tela inicial explicativa sobre o sistema
- **Feedback Visual**: Sistema de notificações para ações do usuário
- **Esquema de Cores Profissional**: Paleta adequada para aplicações financeiras

## Stack Tecnológico

A aplicação foi desenvolvida utilizando tecnologias modernas e consolidadas:

- **React 18** com TypeScript para tipagem estática e maior segurança
- **Vite** como bundler para build otimizado e desenvolvimento rápido
- **React Router** para navegação entre páginas
- **Axios** para comunicação HTTP com a API
- **CSS Modules** para encapsulamento de estilos

## Instalação e Execução

### Instalação de Dependências

```bash
npm install
```

### Modo Desenvolvimento

```bash
npm run dev
```

### Build de Produção

```bash
npm run build
```

### Preview do Build

```bash
npm run preview
```

## Configuração

A interface está configurada para se comunicar com a API na porta `5146` através do proxy configurado no `vite.config.ts`.

Caso sua API esteja executando em porta diferente, ajuste o arquivo de configuração:

```typescript
server: {
  port: 5173,
  proxy: {
    '/api': {
      target: 'http://localhost:SUA_PORTA',
      changeOrigin: true,
    },
  },
}
```

## Sistema de Temas

A aplicação utiliza CSS Variables para implementação de temas, facilitando customização e manutenção.

### Variáveis Principais

As cores do sistema estão definidas em `src/styles/themes.css`:

- **Primary** (#2563eb): Ações principais e elementos interativos
- **Revenue** (#10b981): Indicadores de receitas e valores positivos
- **Expense** (#ef4444): Indicadores de despesas e valores negativos
- **Info** (#3b82f6): Elementos informativos

### Alternância de Tema

O usuário pode alternar entre temas claro e escuro através do botão localizado no header. A preferência é persistida no localStorage do navegador.

## Estrutura de Páginas

A aplicação é composta pelas seguintes páginas principais:

1. **HomePage**: Página inicial com apresentação do sistema
2. **Dashboard**: Visão geral com resumo financeiro consolidado
3. **Pessoas**: Interface de gerenciamento de pessoas
4. **Categorias**: Interface de gerenciamento de categorias
5. **Transações**: Formulário e listagem de receitas e despesas
6. **Consultas**: Relatórios agregados por pessoa e categoria

## Funcionalidades Implementadas

### Gestão de Pessoas

- Cadastro com validação de campos obrigatórios (nome e idade)
- Listagem completa de pessoas cadastradas
- Exclusão com modal de confirmação para evitar erros

### Gestão de Categorias

- Cadastro com seleção de finalidade (Despesa, Receita ou Ambas)
- Listagem com badges visuais indicando a finalidade
- Validação automática de compatibilidade entre categoria e tipo de transação

### Gestão de Transações

- Registro de receitas e despesas
- Filtragem inteligente de categorias baseada no tipo selecionado
- Validação de regras de negócio (exemplo: menores de 18 anos restritos a despesas)
- Listagem com diferenciação visual por tipo de transação

### Relatórios e Consultas

- Totais agregados por pessoa
- Totais agregados por categoria
- Visualização de saldos e totais gerais
- Organização através de componente de tabs

## Arquitetura de Código

```
src/
├── components/       # Componentes reutilizáveis
│   ├── Layout.tsx   # Layout principal com navegação
│   ├── Modal.tsx    # Componente modal genérico
│   └── Toast.tsx    # Sistema de notificações
├── hooks/           # Custom hooks
│   └── useToast.tsx # Hook para gerenciamento de toasts
├── pages/           # Páginas da aplicação
│   ├── HomePage.tsx
│   ├── Dashboard.tsx
│   ├── PessoasPage.tsx
│   ├── CategoriasPage.tsx
│   ├── TransacoesPage.tsx
│   └── ConsultasPage.tsx
├── services/        # Camada de serviços
│   └── api.ts       # Configuração Axios e endpoints
├── styles/          # Estilos globais
│   ├── themes.css   # Definições de temas
│   └── global.css   # Estilos e utilitários globais
└── types/           # Definições TypeScript
    └── index.ts     # Interfaces e tipos
```

## Customização

### Modificação de Cores

Para alterar o esquema de cores, edite as variáveis CSS em `src/styles/themes.css`:

```css
:root {
  --primary: #2563eb;
  --revenue: #10b981;
  --expense: #ef4444;
  /* Demais variáveis */
}
```

### Adição de Componentes

Ao criar novos componentes em `src/components/`, siga os padrões estabelecidos:

- Utilização de TypeScript com tipagem explícita
- Estilos através de CSS Modules com arquivo separado
- Documentação inline através de comentários JSDoc

## Responsividade

A interface implementa layout responsivo através de media queries e flexbox:

- **Desktop**: Layout horizontal completo com sidebar de navegação
- **Tablet**: Layout adaptado com navegação otimizada
- **Mobile**: Layout vertical com navegação colapsável

## Observações Técnicas

- A aplicação não utiliza localStorage, sessionStorage ou outras APIs de armazenamento do navegador para dados de aplicação (exceto para preferências de tema)
- Todo gerenciamento de estado é feito através de React state
- Validações são realizadas tanto no frontend quanto no backend
- A interface consome exclusivamente a API REST desenvolvida

## Autor

Desenvolvido por [K4U4dev](https://github.com/K4U4dev)

## Licença

Este projeto foi desenvolvido como parte da resolução de prova técnica do sistema HomeFinance.