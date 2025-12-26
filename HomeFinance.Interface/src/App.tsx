import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { useState, useEffect } from 'react'
import HomePage from './pages/HomePage'
import Dashboard from './pages/Dashboard'
import PessoasPage from './pages/PessoasPage'
import CategoriasPage from './pages/CategoriasPage'
import TransacoesPage from './pages/TransacoesPage'
import ConsultasPage from './pages/ConsultasPage'
import Layout from './components/Layout'
import './App.css'

function App() {
  // Gerencia tema claro/escuro
  const [theme, setTheme] = useState<'light' | 'dark'>('light')

  useEffect(() => {
    // Pega tema salvo ou usa o do sistema
    const savedTheme = localStorage.getItem('theme') as 'light' | 'dark' | null
    const systemTheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    const initialTheme = savedTheme || systemTheme
    
    setTheme(initialTheme)
    document.documentElement.setAttribute('data-theme', initialTheme)
  }, [])

  const toggleTheme = () => {
    const newTheme = theme === 'light' ? 'dark' : 'light'
    setTheme(newTheme)
    document.documentElement.setAttribute('data-theme', newTheme)
    localStorage.setItem('theme', newTheme)
  }

  return (
    <Router
      future={{
        v7_startTransition: true,
        v7_relativeSplatPath: true,
      }}
    >
      <Routes>
        <Route path="/" element={<HomePage onEnter={() => {}} />} />
        <Route path="/app" element={<Layout theme={theme} toggleTheme={toggleTheme} />}>
          <Route index element={<Dashboard />} />
          <Route path="pessoas" element={<PessoasPage />} />
          <Route path="categorias" element={<CategoriasPage />} />
          <Route path="transacoes" element={<TransacoesPage />} />
          <Route path="consultas" element={<ConsultasPage />} />
        </Route>
      </Routes>
    </Router>
  )
}

export default App

