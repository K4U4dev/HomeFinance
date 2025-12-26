// Dashboard com resumo financeiro geral
// Mostra totais de receitas, despesas e saldo líquido

import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { FaWallet, FaMoneyBillWave, FaCheckCircle, FaExclamationTriangle, FaUsers, FaChartBar, FaCreditCard, FaChartLine } from 'react-icons/fa'
import { consultasService } from '../services/api'
import { useToast } from '../hooks/useToast'
import type { ConsultaTotaisPorPessoa } from '../types'
import './Dashboard.css'

export default function Dashboard() {
  const [totais, setTotais] = useState<ConsultaTotaisPorPessoa | null>(null)
  const [loading, setLoading] = useState(true)
  const { showToast, ToastContainer } = useToast()

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      setLoading(true)
      const data = await consultasService.obterTotaisPorPessoa()
      setTotais(data)
    } catch (error) {
      showToast(error instanceof Error ? error.message : 'Erro ao carregar dados', 'error')
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
        <div className="dashboard-loading">
          <div className="spinner"></div>
          <p>Carregando dados...</p>
        </div>
        <ToastContainer />
      </>
    )
  }

  return (
    <>
      <div className="dashboard">
        <div className="dashboard-header">
          <h1>Dashboard</h1>
          <p className="dashboard-subtitle">Visão geral das suas finanças</p>
        </div>

        {totais && (
          <>
            {/* Cards com resumo financeiro */}
            <div className="dashboard-summary">
              <div className="summary-card revenue">
                <div className="summary-icon">
                  <FaWallet />
                </div>
                <div className="summary-content">
                  <h3>Total de Receitas</h3>
                  <p className="summary-value">{formatCurrency(totais.totalGeralReceitas)}</p>
                </div>
              </div>
              <div className="summary-card expense">
                <div className="summary-icon">
                  <FaMoneyBillWave />
                </div>
                <div className="summary-content">
                  <h3>Total de Despesas</h3>
                  <p className="summary-value">{formatCurrency(totais.totalGeralDespesas)}</p>
                </div>
              </div>
              <div className={`summary-card ${totais.saldoLiquidoGeral >= 0 ? 'balance-positive' : 'balance-negative'}`}>
                <div className="summary-icon">
                  {totais.saldoLiquidoGeral >= 0 ? <FaCheckCircle /> : <FaExclamationTriangle />}
                </div>
                <div className="summary-content">
                  <h3>Saldo Líquido</h3>
                  <p className="summary-value">{formatCurrency(totais.saldoLiquidoGeral)}</p>
                </div>
              </div>
            </div>

            {/* Tabela com totais por pessoa */}
            <div className="dashboard-section">
              <h2>Totais por Pessoa</h2>
              {totais.totaisPorPessoa.length === 0 ? (
                <div className="empty-state">
                  <p>Nenhuma pessoa cadastrada ainda.</p>
                  <Link to="/app/pessoas" className="btn btn-primary">
                    Cadastrar Pessoa
                  </Link>
                </div>
              ) : (
                <div className="table-container">
                  <table className="table">
                    <thead>
                      <tr>
                        <th>Pessoa</th>
                        <th className="text-right">Receitas</th>
                        <th className="text-right">Despesas</th>
                        <th className="text-right">Saldo</th>
                      </tr>
                    </thead>
                    <tbody>
                      {totais.totaisPorPessoa.map((total) => (
                        <tr key={total.pessoaId}>
                          <td><span style={{ fontWeight: 600 }}>{total.pessoaNome}</span></td>
                          <td className="text-right revenue-text">{formatCurrency(total.totalReceitas)}</td>
                          <td className="text-right expense-text">{formatCurrency(total.totalDespesas)}</td>
                          <td className={`text-right ${total.saldo >= 0 ? 'balance-positive-text' : 'balance-negative-text'}`}>
                            <span style={{ fontWeight: 700 }}>{formatCurrency(total.saldo)}</span>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>

            {/* Cards de navegação rápida */}
            <div className="dashboard-actions">
              <h2>Ações Rápidas</h2>
              <div className="actions-grid">
                <Link to="/app/pessoas" className="action-card">
                  <div className="action-icon">
                    <FaUsers />
                  </div>
                  <h3>Gerenciar Pessoas</h3>
                  <p>Cadastrar e gerenciar pessoas</p>
                </Link>
                <Link to="/app/categorias" className="action-card">
                  <div className="action-icon">
                    <FaChartBar />
                  </div>
                  <h3>Gerenciar Categorias</h3>
                  <p>Organizar categorias de transações</p>
                </Link>
                <Link to="/app/transacoes" className="action-card">
                  <div className="action-icon">
                    <FaCreditCard />
                  </div>
                  <h3>Nova Transação</h3>
                  <p>Registrar receita ou despesa</p>
                </Link>
                <Link to="/app/consultas" className="action-card">
                  <div className="action-icon">
                    <FaChartLine />
                  </div>
                  <h3>Relatórios</h3>
                  <p>Visualizar relatórios detalhados</p>
                </Link>
              </div>
            </div>
          </>
        )}
      </div>
      <ToastContainer />
    </>
  )
}

