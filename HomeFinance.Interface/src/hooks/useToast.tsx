// Hook para gerenciar toasts
// Evita mostrar toasts duplicados usando uma chave única

import { useState, useCallback, useRef } from 'react'
import Toast, { ToastType } from '../components/Toast'
import '../components/Toast.css'

interface ToastData {
  id: number
  message: string
  type: ToastType
}

export function useToast() {
  const [toasts, setToasts] = useState<ToastData[]>([])
  const shownToastsRef = useRef<Set<string>>(new Set())

  const showToast = useCallback((message: string, type: ToastType = 'info') => {
    // Cria uma chave única baseada no tipo e mensagem
    const toastKey = `${type}:${message}`
    
    // Se já mostrou esse toast antes, ignora
    if (shownToastsRef.current.has(toastKey)) {
      return
    }

    const id = Date.now()
    setToasts((prev) => [...prev, { id, message, type }])
    shownToastsRef.current.add(toastKey)
    
    // Limpa a chave depois de 10s pra poder mostrar de novo se necessário
    setTimeout(() => {
      shownToastsRef.current.delete(toastKey)
    }, 10000)
  }, [])

  const removeToast = useCallback((id: number) => {
    setToasts((prev) => {
      const toastToRemove = prev.find((t) => t.id === id)
      if (toastToRemove) {
        const toastKey = `${toastToRemove.type}:${toastToRemove.message}`
        shownToastsRef.current.delete(toastKey)
      }
      return prev.filter((toast) => toast.id !== id)
    })
  }, [])

  const ToastContainer = () => (
    <div className="toast-container">
      {toasts.map((toast) => (
        <Toast
          key={toast.id}
          message={toast.message}
          type={toast.type}
          onClose={() => removeToast(toast.id)}
        />
      ))}
    </div>
  )

  return { showToast, ToastContainer }
}

