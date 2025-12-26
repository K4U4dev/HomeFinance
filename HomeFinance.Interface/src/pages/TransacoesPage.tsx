// Página principal pra registrar receitas e despesas
// Filtra categorias automaticamente baseado no tipo selecionado

import { useState, useEffect } from 'react'
import { transacoesService, pessoasService, categoriasService } from '../services/api'
import { useToast } from '../hooks/useToast'
import Modal from '../components/Modal'
import type { Transacao, CriarTransacao, Pessoa, Categoria } from '../types'
import { TipoTransacao, FinalidadeCategoria } from '../types'
import './TransacoesPage.css'

export default function TransacoesPage() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([])
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [loading, setLoading] = useState(true)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [formData, setFormData] = useState<CriarTransacao>({
    descricao: '',
    valor: 0,
    tipo: TipoTransacao.Despesa,
    categoriaId: '',
    pessoaId: '',
  })
  const { showToast, ToastContainer } = useToast()

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      setLoading(true)
      const [transacoesData, pessoasData, categoriasData] = await Promise.all([
        transacoesService.obterTodas(),
        pessoasService.obterTodas(),
        categoriasService.obterTodas(),
      ])
      setTransacoes(transacoesData)
      setPessoas(pessoasData)
      setCategorias(categoriasData)
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao carregar dados', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!formData.descricao.trim()) {
      showToast('Descrição é obrigatória', 'error')
      return
    }

    if (formData.valor <= 0) {
      showToast('Valor deve ser positivo', 'error')
      return
    }

    if (!formData.pessoaId) {
      showToast('Selecione uma pessoa', 'error')
      return
    }

    if (!formData.categoriaId) {
      showToast('Selecione uma categoria', 'error')
      return
    }

    try {
      await transacoesService.criar(formData)
      showToast('Transação cadastrada com sucesso!', 'success')
      setIsModalOpen(false)
      setFormData({
        descricao: '',
        valor: 0,
        tipo: TipoTransacao.Despesa,
        categoriaId: '',
        pessoaId: '',
      })
      loadData()
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao cadastrar transação', 'error')
    }
  }

  const getCategoriasDisponiveis = (tipo: TipoTransacao): Categoria[] => {
    return categorias.filter((cat) => {
      if (cat.finalidade === FinalidadeCategoria.Ambas) return true
      if (tipo === TipoTransacao.Despesa && cat.finalidade === FinalidadeCategoria.Despesa) return true
      if (tipo === TipoTransacao.Receita && cat.finalidade === FinalidadeCategoria.Receita) return true
      return false
    })
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value)
  }

  if (loading) {
    return (
      <>
        <div className="page-loading">
          <div className="spinner"></div>
          <p>Carregando transações...</p>
        </div>
        <ToastContainer />
      </>
    )
  }

  return (
    <>
      <div className="transacoes-page">
        <div className="page-header">
          <div>
            <h1>Gerenciar Transações</h1>
            <p className="page-subtitle">Registre receitas e despesas</p>
          </div>
          <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
            + Nova Transação
          </button>
        </div>

        {transacoes.length === 0 ? (
          <div className="empty-state">
            <p>Nenhuma transação registrada ainda.</p>
            <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
              Registrar Primeira Transação
            </button>
          </div>
        ) : (
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Descrição</th>
                  <th>Pessoa</th>
                  <th>Categoria</th>
                  <th>Tipo</th>
                  <th className="text-right">Valor</th>
                </tr>
              </thead>
              <tbody>
                {transacoes.map((transacao) => (
                  <tr key={transacao.id}>
                    <td><span style={{ fontWeight: 600 }}>{transacao.descricao}</span></td>
                    <td>{transacao.pessoaNome}</td>
                    <td>{transacao.categoriaDescricao}</td>
                    <td>
                      <span className={`badge ${transacao.tipo === TipoTransacao.Receita ? 'badge-success' : 'badge-error'}`}>
                        {transacao.tipo === TipoTransacao.Receita ? 'Receita' : 'Despesa'}
                      </span>
                    </td>
                    <td className={`text-right ${transacao.tipo === TipoTransacao.Receita ? 'revenue-text' : 'expense-text'}`}>
                      <span style={{ fontWeight: 600 }}>{formatCurrency(transacao.valor)}</span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false)
          setFormData({
            descricao: '',
            valor: 0,
            tipo: TipoTransacao.Despesa,
            categoriaId: '',
            pessoaId: '',
          })
        }}
        title="Nova Transação"
        size="lg"
      >
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <label className="input-label" htmlFor="descricao">
              Descrição *
            </label>
            <input
              id="descricao"
              type="text"
              className="input"
              value={formData.descricao}
              onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
              required
              placeholder="Ex: Compra no supermercado, Salário mensal"
            />
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="valor">
              Valor *
            </label>
            <input
              id="valor"
              type="number"
              className="input"
              value={formData.valor || ''}
              onChange={(e) => setFormData({ ...formData, valor: parseFloat(e.target.value) || 0 })}
              required
              min="0.01"
              step="0.01"
              placeholder="0.00"
            />
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="tipo">
              Tipo *
            </label>
            <div className="tipo-buttons">
              <button
                type="button"
                className={`btn ${formData.tipo === TipoTransacao.Receita ? 'btn-revenue' : 'btn-secondary'}`}
                onClick={() => setFormData({ ...formData, tipo: TipoTransacao.Receita, categoriaId: '' })}
              >
                Receita
              </button>
              <button
                type="button"
                className={`btn ${formData.tipo === TipoTransacao.Despesa ? 'btn-expense' : 'btn-secondary'}`}
                onClick={() => setFormData({ ...formData, tipo: TipoTransacao.Despesa, categoriaId: '' })}
              >
                Despesa
              </button>
            </div>
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="pessoaId">
              Pessoa *
            </label>
            <select
              id="pessoaId"
              className="select"
              value={formData.pessoaId}
              onChange={(e) => setFormData({ ...formData, pessoaId: e.target.value })}
              required
            >
              <option value="">Selecione uma pessoa</option>
              {pessoas.map((pessoa) => (
                <option key={pessoa.id} value={pessoa.id}>
                  {pessoa.nome} ({pessoa.idade} anos)
                </option>
              ))}
            </select>
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="categoriaId">
              Categoria *
            </label>
            <select
              id="categoriaId"
              className="select"
              value={formData.categoriaId}
              onChange={(e) => setFormData({ ...formData, categoriaId: e.target.value })}
              required
              disabled={getCategoriasDisponiveis(formData.tipo).length === 0}
            >
              <option value="">
                {getCategoriasDisponiveis(formData.tipo).length === 0
                  ? 'Nenhuma categoria disponível para este tipo'
                  : 'Selecione uma categoria'}
              </option>
              {getCategoriasDisponiveis(formData.tipo).map((categoria) => (
                <option key={categoria.id} value={categoria.id}>
                  {categoria.descricao}
                </option>
              ))}
            </select>
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setIsModalOpen(false)
                setFormData({
                  descricao: '',
                  valor: 0,
                  tipo: TipoTransacao.Despesa,
                  categoriaId: '',
                  pessoaId: '',
                })
              }}
            >
              Cancelar
            </button>
            <button type="submit" className="btn btn-primary">
              Salvar
            </button>
          </div>
        </form>
      </Modal>

      <ToastContainer />
    </>
  )
}

