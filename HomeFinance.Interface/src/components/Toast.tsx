// Toast de notificação que desaparece automaticamente
// Usado para feedback de ações do usuário

import { useEffect } from 'react'
import { FaCheck, FaTimes, FaInfoCircle, FaExclamationTriangle } from 'react-icons/fa'
import './Toast.css'

export type ToastType = 'success' | 'error' | 'info' | 'warning'

interface ToastProps {
  message: string
  type: ToastType
  onClose: () => void
  duration?: number
}

export default function Toast({ message, type, onClose, duration = 5000 }: ToastProps) {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose()
    }, duration)

    return () => clearTimeout(timer)
  }, [onClose, duration])

  const icons = {
    success: FaCheck,
    error: FaTimes,
    info: FaInfoCircle,
    warning: FaExclamationTriangle,
  }

  const Icon = icons[type]

  return (
    <div className={`toast toast-${type}`} role="alert">
      <span className="toast-icon">
        <Icon />
      </span>
      <span className="toast-message">{message}</span>
      <button className="toast-close" onClick={onClose} aria-label="Fechar">
        <FaTimes />
      </button>
    </div>
  )
}

