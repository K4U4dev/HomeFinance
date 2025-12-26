// Landing page do sistema
// Primeira tela que o usuário vê antes de entrar no app

import { useNavigate } from 'react-router-dom'
import { FaUsers, FaChartBar, FaWallet, FaLock, FaArrowRight } from 'react-icons/fa'
import './HomePage.css'

interface HomePageProps {
  onEnter: () => void
}

export default function HomePage({ onEnter }: HomePageProps) {
  const navigate = useNavigate()

  const handleEnter = () => {
    onEnter()
    navigate('/app')
  }

  return (
    <div className="homepage">
      <div className="homepage-hero">
        <div className="homepage-content">
          <h1 className="homepage-title">
            Controle Total dos Seus
            <span className="homepage-title-highlight"> Gastos Residenciais</span>
          </h1>
          <p className="homepage-description">
            Gerencie suas finanças pessoais e familiares de forma inteligente.
            Acompanhe receitas, despesas e mantenha suas finanças organizadas
            com uma solução completa e intuitiva.
          </p>

          <div className="homepage-features">
            <div className="feature-card">
              <div className="feature-icon">
                <FaUsers />
              </div>
              <h3>Gestão de Pessoas</h3>
              <p>Controle individual de gastos por pessoa</p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">
                <FaChartBar />
              </div>
              <h3>Categorização</h3>
              <p>Organize suas transações por categorias</p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">
                <FaWallet />
              </div>
              <h3>Relatórios</h3>
              <p>Visualize totais e saldos detalhados</p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">
                <FaLock />
              </div>
              <h3>Seguro</h3>
              <p>Dados protegidos e privados</p>
            </div>
          </div>

          <button className="btn btn-primary btn-lg homepage-cta" onClick={handleEnter}>
            Começar Agora <FaArrowRight />
          </button>
        </div>
      </div>

      <footer className="homepage-footer">
        <div className="container">
          <div className="homepage-footer-content">
            <p>
              Desenvolvido com por <strong>K4U4dev</strong>
            </p>
            <a
              href="https://github.com/K4U4dev"
              target="_blank"
              rel="noopener noreferrer"
              className="homepage-footer-link"
            >
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22"/>
              </svg>
              <span>GitHub</span>
            </a>
          </div>
        </div>
      </footer>
    </div>
  )
}

