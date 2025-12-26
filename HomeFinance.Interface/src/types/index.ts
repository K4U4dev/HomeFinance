// Enums e interfaces usados em toda a aplicação

export enum TipoTransacao {
  Despesa = 1,
  Receita = 2
}

export enum FinalidadeCategoria {
  Despesa = 1,
  Receita = 2,
  Ambas = 3
}

export interface Pessoa {
  id: string
  nome: string
  idade: number
}

export interface CriarPessoa {
  nome: string
  idade: number
}

export interface Categoria {
  id: string
  descricao: string
  finalidade: FinalidadeCategoria
}

export interface CriarCategoria {
  descricao: string
  finalidade: FinalidadeCategoria
}

export interface Transacao {
  id: string
  descricao: string
  valor: number
  tipo: TipoTransacao
  categoriaId: string
  categoriaDescricao?: string
  pessoaId: string
  pessoaNome?: string
}

export interface CriarTransacao {
  descricao: string
  valor: number
  tipo: TipoTransacao
  categoriaId: string
  pessoaId: string
}

export interface TotaisPorPessoa {
  pessoaId: string
  pessoaNome: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface ConsultaTotaisPorPessoa {
  totaisPorPessoa: TotaisPorPessoa[]
  totalGeralReceitas: number
  totalGeralDespesas: number
  saldoLiquidoGeral: number
}

export interface TotaisPorCategoria {
  categoriaId: string
  categoriaDescricao: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface ConsultaTotaisPorCategoria {
  totaisPorCategoria: TotaisPorCategoria[]
  totalGeralReceitas: number
  totalGeralDespesas: number
  saldoLiquidoGeral: number
}

export interface ApiError {
  mensagem: string
}

