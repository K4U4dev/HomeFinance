// Layout principal com sidebar lateral que expande/retrai
// Usa localStorage para lembrar o estado da sidebar

import { useState, useEffect } from 'react'
import { Link, useLocation, Outlet } from 'react-router-dom'
import { FaWallet, FaMoon, FaSun, FaHome, FaUsers, FaTags, FaExchangeAlt, FaChartBar, FaChevronLeft, FaChevronRight, FaBars } from 'react-icons/fa'
import './Layout.css'

interface LayoutProps {
  theme: 'light' | 'dark'
  toggleTheme: () => void
}

export default function Layout({ theme, toggleTheme }: LayoutProps) {
  const location = useLocation()
  const [sidebarExpanded, setSidebarExpanded] = useState(() => {
    const saved = localStorage.getItem('sidebarExpanded')
    return saved !== null ? saved === 'true' : true
  })

  useEffect(() => {
    localStorage.setItem('sidebarExpanded', String(sidebarExpanded))
  }, [sidebarExpanded])

  const isActive = (path: string) => {
    return location.pathname === `/app${path}` || (path === '' && location.pathname === '/app')
  }

  const toggleSidebar = () => {
    setSidebarExpanded(!sidebarExpanded)
  }

  const navItems = [
    { path: '', label: 'Dashboard', icon: FaHome },
    { path: '/pessoas', label: 'Pessoas', icon: FaUsers },
    { path: '/categorias', label: 'Categorias', icon: FaTags },
    { path: '/transacoes', label: 'Transações', icon: FaExchangeAlt },
    { path: '/consultas', label: 'Consultas', icon: FaChartBar },
  ]

  return (
    <div className={`layout ${sidebarExpanded ? 'sidebar-expanded' : 'sidebar-collapsed'}`}>
      <div className="sidebar-overlay" onClick={toggleSidebar} style={{ pointerEvents: 'none' }} />
      <aside className={`sidebar ${sidebarExpanded ? 'expanded' : 'collapsed'}`}>
        <div className="sidebar-header">
          <Link to="/app" className="logo">
            <FaWallet className="logo-icon" />
            {sidebarExpanded && <span className="logo-text">HomeFinance</span>}
          </Link>
        </div>
        
        <nav className="sidebar-nav">
          {navItems.map((item) => {
            const Icon = item.icon
            return (
              <Link
                key={item.path}
                to={`/app${item.path}`}
                className={`nav-link ${isActive(item.path) ? 'active' : ''}`}
                title={!sidebarExpanded ? item.label : ''}
              >
                <Icon className="nav-icon" />
                {sidebarExpanded && <span className="nav-label">{item.label}</span>}
              </Link>
            )
          })}
        </nav>

        <div className="sidebar-footer">
          <button
            className="sidebar-toggle"
            onClick={toggleSidebar}
            aria-label={sidebarExpanded ? 'Retrair sidebar' : 'Expandir sidebar'}
          >
            {sidebarExpanded ? <FaChevronLeft /> : <FaChevronRight />}
          </button>
          <button
            className="theme-toggle"
            onClick={toggleTheme}
            aria-label="Alternar tema"
            title={!sidebarExpanded ? 'Alternar tema' : ''}
          >
            {theme === 'light' ? <FaMoon /> : <FaSun />}
            {sidebarExpanded && <span className="theme-toggle-label">Alternar tema</span>}
          </button>
        </div>
      </aside>

      <div className="layout-main">
        <header className="header">
          <div className="container">
            <div className="header-content">
              <button className="mobile-menu-toggle" onClick={toggleSidebar} aria-label="Abrir menu">
                <FaBars />
              </button>
              <h1 className="page-title">HomeFinance</h1>
            </div>
          </div>
        </header>

        <main className="main-content">
          <div className="container">
            <Outlet />
          </div>
        </main>

        <footer className="footer">
          <div className="container">
            <div className="footer-content">
              <p>Desenvolvido por <strong>K4U4dev</strong></p>
              <a 
                href="https://github.com/K4U4dev" 
                target="_blank" 
                rel="noopener noreferrer"
                className="footer-link"
              >
                <span>GitHub</span>
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22"/>
                </svg>
              </a>
            </div>
          </div>
        </footer>
      </div>
    </div>
  )
}

