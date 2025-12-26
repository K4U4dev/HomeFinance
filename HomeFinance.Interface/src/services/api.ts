// Configuração do axios e serviços de API
// Todos os endpoints ficam aqui organizados por entidade

import axios, { AxiosError } from 'axios'
import type {
  Pessoa,
  CriarPessoa,
  Categoria,
  CriarCategoria,
  Transacao,
  CriarTransacao,
  ConsultaTotaisPorPessoa,
  ConsultaTotaisPorCategoria,
  ApiError
} from '../types'

// Instância do axios apontando pro backend
const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor que pega erros da API e transforma em mensagens legíveis
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    const message = error.response?.data?.mensagem || error.message || 'Erro ao processar requisição'
    return Promise.reject(new Error(message))
  }
)

// CRUD de pessoas
export const pessoasService = {
  obterTodas: async (): Promise<Pessoa[]> => {
    const response = await api.get<Pessoa[]>('/pessoas')
    return response.data
  },

  obterPorId: async (id: string): Promise<Pessoa> => {
    const response = await api.get<Pessoa>(`/pessoas/${id}`)
    return response.data
  },

  criar: async (pessoa: CriarPessoa): Promise<Pessoa> => {
    const response = await api.post<Pessoa>('/pessoas', pessoa)
    return response.data
  },

  remover: async (id: string): Promise<void> => {
    await api.delete(`/pessoas/${id}`)
  },
}

// CRUD de categorias
export const categoriasService = {
  obterTodas: async (): Promise<Categoria[]> => {
    const response = await api.get<Categoria[]>('/categorias')
    return response.data
  },

  obterPorId: async (id: string): Promise<Categoria> => {
    const response = await api.get<Categoria>(`/categorias/${id}`)
    return response.data
  },

  criar: async (categoria: CriarCategoria): Promise<Categoria> => {
    const response = await api.post<Categoria>('/categorias', categoria)
    return response.data
  },
}

// CRUD de transações
export const transacoesService = {
  obterTodas: async (): Promise<Transacao[]> => {
    const response = await api.get<Transacao[]>('/transacoes')
    return response.data
  },

  obterPorId: async (id: string): Promise<Transacao> => {
    const response = await api.get<Transacao>(`/transacoes/${id}`)
    return response.data
  },

  criar: async (transacao: CriarTransacao): Promise<Transacao> => {
    const response = await api.post<Transacao>('/transacoes', transacao)
    return response.data
  },
}

// Endpoints de relatórios e consultas
export const consultasService = {
  obterTotaisPorPessoa: async (): Promise<ConsultaTotaisPorPessoa> => {
    const response = await api.get<ConsultaTotaisPorPessoa>('/consultas/totais-por-pessoa')
    return response.data
  },

  obterTotaisPorCategoria: async (): Promise<ConsultaTotaisPorCategoria> => {
    const response = await api.get<ConsultaTotaisPorCategoria>('/consultas/totais-por-categoria')
    return response.data
  },
}

export default api

