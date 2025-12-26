// Página pra cadastrar e gerenciar pessoas
// Lista todas e permite criar/remover

import { useState, useEffect } from 'react'
import { pessoasService } from '../services/api'
import { useToast } from '../hooks/useToast'
import Modal from '../components/Modal'
import type { Pessoa, CriarPessoa } from '../types'
import './PessoasPage.css'

export default function PessoasPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [loading, setLoading] = useState(true)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [formData, setFormData] = useState<CriarPessoa>({ nome: '', idade: 0 })
  const [deletingId, setDeletingId] = useState<string | null>(null)
  const { showToast, ToastContainer } = useToast()

  useEffect(() => {
    loadPessoas()
  }, [])

  const loadPessoas = async () => {
    try {
      setLoading(true)
      const data = await pessoasService.obterTodas()
      setPessoas(data)
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao carregar pessoas', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    
    if (!formData.nome.trim()) {
      showToast('Nome é obrigatório', 'error')
      return
    }

    if (formData.idade <= 0) {
      showToast('Idade deve ser um número positivo', 'error')
      return
    }

    try {
      await pessoasService.criar(formData)
      showToast('Pessoa cadastrada com sucesso!', 'success')
      setIsModalOpen(false)
      setFormData({ nome: '', idade: 0 })
      loadPessoas()
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao cadastrar pessoa', 'error')
    }
  }

  const handleDelete = async (id: string) => {
    if (!confirm('Tem certeza que deseja excluir esta pessoa? Todas as transações associadas serão removidas.')) {
      return
    }

    try {
      setDeletingId(id)
      await pessoasService.remover(id)
      showToast('Pessoa removida com sucesso!', 'success')
      loadPessoas()
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao remover pessoa', 'error')
    } finally {
      setDeletingId(null)
    }
  }

  if (loading) {
    return (
      <>
        <div className="page-loading">
          <div className="spinner"></div>
          <p>Carregando pessoas...</p>
        </div>
        <ToastContainer />
      </>
    )
  }

  return (
    <>
      <div className="pessoas-page">
        <div className="page-header">
          <div>
            <h1>Gerenciar Pessoas</h1>
            <p className="page-subtitle">Cadastre e gerencie as pessoas do sistema</p>
          </div>
          <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
            + Nova Pessoa
          </button>
        </div>

        {pessoas.length === 0 ? (
          <div className="empty-state">
            <p>Nenhuma pessoa cadastrada ainda.</p>
            <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
              Cadastrar Primeira Pessoa
            </button>
          </div>
        ) : (
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>Idade</th>
                  <th className="text-right">Ações</th>
                </tr>
              </thead>
              <tbody>
                {pessoas.map((pessoa) => (
                  <tr key={pessoa.id}>
                    <td><span style={{ fontWeight: 600 }}>{pessoa.nome}</span></td>
                    <td>{pessoa.idade} anos</td>
                    <td className="text-right">
                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() => handleDelete(pessoa.id)}
                        disabled={deletingId === pessoa.id}
                      >
                        {deletingId === pessoa.id ? 'Removendo...' : 'Remover'}
                      </button>
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
          setFormData({ nome: '', idade: 0 })
        }}
        title="Nova Pessoa"
      >
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <label className="input-label" htmlFor="nome">
              Nome *
            </label>
            <input
              id="nome"
              type="text"
              className="input"
              value={formData.nome}
              onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
              required
              placeholder="Digite o nome completo"
            />
          </div>

          <div className="input-group">
            <label className="input-label" htmlFor="idade">
              Idade *
            </label>
            <input
              id="idade"
              type="number"
              className="input"
              value={formData.idade || ''}
              onChange={(e) => setFormData({ ...formData, idade: parseInt(e.target.value) || 0 })}
              required
              min="1"
              placeholder="Digite a idade"
            />
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setIsModalOpen(false)
                setFormData({ nome: '', idade: 0 })
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

