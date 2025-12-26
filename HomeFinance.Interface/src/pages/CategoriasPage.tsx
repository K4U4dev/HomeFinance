// Página pra criar e visualizar categorias
// Cada categoria pode ser usada pra despesa, receita ou ambas

import { useState, useEffect } from 'react'
import { categoriasService } from '../services/api'
import { useToast } from '../hooks/useToast'
import Modal from '../components/Modal'
import type { Categoria, CriarCategoria } from '../types'
import { FinalidadeCategoria } from '../types'
import './CategoriasPage.css'

export default function CategoriasPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [loading, setLoading] = useState(true)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [formData, setFormData] = useState<CriarCategoria>({
    descricao: '',
    finalidade: FinalidadeCategoria.Ambas,
  })
  const { showToast, ToastContainer } = useToast()

  useEffect(() => {
    loadCategorias()
  }, [])

  const loadCategorias = async () => {
    try {
      setLoading(true)
      const data = await categoriasService.obterTodas()
      setCategorias(data)
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao carregar categorias', 'error')
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

    try {
      await categoriasService.criar(formData)
      showToast('Categoria cadastrada com sucesso!', 'success')
      setIsModalOpen(false)
      setFormData({ descricao: '', finalidade: FinalidadeCategoria.Ambas })
      loadCategorias()
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao cadastrar categoria', 'error')
    }
  }

  const getFinalidadeLabel = (finalidade: FinalidadeCategoria): string => {
    switch (finalidade) {
      case FinalidadeCategoria.Despesa:
        return 'Despesa'
      case FinalidadeCategoria.Receita:
        return 'Receita'
      case FinalidadeCategoria.Ambas:
        return 'Ambas'
      default:
        return ''
    }
  }

  const getFinalidadeBadge = (finalidade: FinalidadeCategoria): string => {
    switch (finalidade) {
      case FinalidadeCategoria.Despesa:
        return 'badge-error'
      case FinalidadeCategoria.Receita:
        return 'badge-success'
      case FinalidadeCategoria.Ambas:
        return 'badge-info'
      default:
        return ''
    }
  }

  if (loading) {
    return (
      <>
        <div className="page-loading">
          <div className="spinner"></div>
          <p>Carregando categorias...</p>
        </div>
        <ToastContainer />
      </>
    )
  }

  return (
    <>
      <div className="categorias-page">
        <div className="page-header">
          <div>
            <h1>Gerenciar Categorias</h1>
            <p className="page-subtitle">Organize suas transações por categorias</p>
          </div>
          <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
            + Nova Categoria
          </button>
        </div>

        {categorias.length === 0 ? (
          <div className="empty-state">
            <p>Nenhuma categoria cadastrada ainda.</p>
            <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
              Cadastrar Primeira Categoria
            </button>
          </div>
        ) : (
          <div className="categorias-grid">
            {categorias.map((categoria) => (
              <div key={categoria.id} className="categoria-card card">
                <div className="categoria-header">
                  <h3 style={{ fontWeight: 600 }}>{categoria.descricao}</h3>
                  <span className={`badge ${getFinalidadeBadge(categoria.finalidade)}`}>
                    {getFinalidadeLabel(categoria.finalidade)}
                  </span>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false)
          setFormData({ descricao: '', finalidade: FinalidadeCategoria.Ambas })
        }}
        title="Nova Categoria"
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
              placeholder="Ex: Alimentação, Salário, Transporte"
            />
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="finalidade">
              Finalidade *
            </label>
            <select
              id="finalidade"
              className="select"
              value={formData.finalidade}
              onChange={(e) => setFormData({ ...formData, finalidade: parseInt(e.target.value) as FinalidadeCategoria })}
              required
            >
              <option value={FinalidadeCategoria.Despesa}>Despesa</option>
              <option value={FinalidadeCategoria.Receita}>Receita</option>
              <option value={FinalidadeCategoria.Ambas}>Ambas</option>
            </select>
            <p className="input-help">
              Define para quais tipos de transações esta categoria pode ser utilizada
            </p>
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setIsModalOpen(false)
                setFormData({ descricao: '', finalidade: FinalidadeCategoria.Ambas })
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

