// Relatórios com totais por pessoa e por categoria

import { useState, useEffect } from 'react'
import { consultasService } from '../services/api'
import { useToast } from '../hooks/useToast'
import type { ConsultaTotaisPorPessoa, ConsultaTotaisPorCategoria } from '../types'
import './ConsultasPage.css'

export default function ConsultasPage() {
  const [totaisPessoa, setTotaisPessoa] = useState<ConsultaTotaisPorPessoa | null>(null)
  const [totaisCategoria, setTotaisCategoria] = useState<ConsultaTotaisPorCategoria | null>(null)
  const [loading, setLoading] = useState(true)
  const [activeTab, setActiveTab] = useState<'pessoa' | 'categoria'>('pessoa')
  const { showToast, ToastContainer } = useToast()

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      setLoading(true)
      const [pessoaData, categoriaData] = await Promise.all([
        consultasService.obterTotaisPorPessoa(),
        consultasService.obterTotaisPorCategoria(),
      ])
      setTotaisPessoa(pessoaData)
      setTotaisCategoria(categoriaData)
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao carregar consultas', 'error')
    } finally {
      setLoading(false)
    }
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
          <p>Carregando relatórios...</p>
        </div>
        <ToastContainer />
      </>
    )
  }

  return (
    <>
      <div className="consultas-page">
        <div className="page-header">
          <div>
            <h1>Consultas e Relatórios</h1>
            <p className="page-subtitle">Visualize totais e saldos detalhados</p>
          </div>
        </div>

        {/* Tabs */}
        <div className="tabs">
          <button
            className={`tab ${activeTab === 'pessoa' ? 'active' : ''}`}
            onClick={() => setActiveTab('pessoa')}
          >
            Totais por Pessoa
          </button>
          <button
            className={`tab ${activeTab === 'categoria' ? 'active' : ''}`}
            onClick={() => setActiveTab('categoria')}
          >
            Totais por Categoria
          </button>
        </div>

        {/* Conteúdo das Tabs */}
        {activeTab === 'pessoa' && totaisPessoa && (
          <div className="consulta-section">
            <div className="consulta-summary">
              <div className="summary-item">
                <span className="summary-label">Total de Receitas</span>
                <span className="summary-value revenue-text">{formatCurrency(totaisPessoa.totalGeralReceitas)}</span>
              </div>
              <div className="summary-item">
                <span className="summary-label">Total de Despesas</span>
                <span className="summary-value expense-text">{formatCurrency(totaisPessoa.totalGeralDespesas)}</span>
              </div>
              <div className="summary-item">
                <span className="summary-label">Saldo Líquido</span>
                <span className={`summary-value ${totaisPessoa.saldoLiquidoGeral >= 0 ? 'balance-positive-text' : 'balance-negative-text'}`}>
                  {formatCurrency(totaisPessoa.saldoLiquidoGeral)}
                </span>
              </div>
            </div>

            <div className="table-container">
              <table className="table">
                <thead>
                  <tr>
                    <th>Pessoa</th>
                    <th className="text-right">Total Receitas</th>
                    <th className="text-right">Total Despesas</th>
                    <th className="text-right">Saldo</th>
                  </tr>
                </thead>
                <tbody>
                  {totaisPessoa.totaisPorPessoa.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="text-center">
                        <p>Nenhum dado disponível</p>
                      </td>
                    </tr>
                  ) : (
                    totaisPessoa.totaisPorPessoa.map((total) => (
                      <tr key={total.pessoaId}>
                        <td><span style={{ fontWeight: 600 }}>{total.pessoaNome}</span></td>
                        <td className="text-right revenue-text">{formatCurrency(total.totalReceitas)}</td>
                        <td className="text-right expense-text">{formatCurrency(total.totalDespesas)}</td>
                        <td className={`text-right ${total.saldo >= 0 ? 'balance-positive-text' : 'balance-negative-text'}`}>
                          <strong>{formatCurrency(total.saldo)}</strong>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'categoria' && totaisCategoria && (
          <div className="consulta-section">
            <div className="consulta-summary">
              <div className="summary-item">
                <span className="summary-label">Total de Receitas</span>
                <span className="summary-value revenue-text">{formatCurrency(totaisCategoria.totalGeralReceitas)}</span>
              </div>
              <div className="summary-item">
                <span className="summary-label">Total de Despesas</span>
                <span className="summary-value expense-text">{formatCurrency(totaisCategoria.totalGeralDespesas)}</span>
              </div>
              <div className="summary-item">
                <span className="summary-label">Saldo Líquido</span>
                <span className={`summary-value ${totaisCategoria.saldoLiquidoGeral >= 0 ? 'balance-positive-text' : 'balance-negative-text'}`}>
                  {formatCurrency(totaisCategoria.saldoLiquidoGeral)}
                </span>
              </div>
            </div>

            <div className="table-container">
              <table className="table">
                <thead>
                  <tr>
                    <th>Categoria</th>
                    <th className="text-right">Total Receitas</th>
                    <th className="text-right">Total Despesas</th>
                    <th className="text-right">Saldo</th>
                  </tr>
                </thead>
                <tbody>
                  {totaisCategoria.totaisPorCategoria.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="text-center">
                        <p>Nenhum dado disponível</p>
                      </td>
                    </tr>
                  ) : (
                    totaisCategoria.totaisPorCategoria.map((total) => (
                      <tr key={total.categoriaId}>
                        <td><span style={{ fontWeight: 600 }}>{total.categoriaDescricao}</span></td>
                        <td className="text-right revenue-text">{formatCurrency(total.totalReceitas)}</td>
                        <td className="text-right expense-text">{formatCurrency(total.totalDespesas)}</td>
                        <td className={`text-right ${total.saldo >= 0 ? 'balance-positive-text' : 'balance-negative-text'}`}>
                          <strong>{formatCurrency(total.saldo)}</strong>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>
      <ToastContainer />
    </>
  )
}

