import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export const partsApi = {
  getAll: () => api.get('/parts'),
  getById: (id) => api.get(`/parts/${id}`),
  create: (part) => api.post('/parts', part),
  update: (id, part) => api.put(`/parts/${id}`, part),
  delete: (id) => api.delete(`/parts/${id}`),
}

export const drawersApi = {
  getAll: () => api.get('/drawers'),
  getById: (id) => api.get(`/drawers/${id}`),
  create: (drawer) => api.post('/drawers', drawer),
  update: (id, drawer) => api.put(`/drawers/${id}`, drawer),
  delete: (id) => api.delete(`/drawers/${id}`),
}

export const categoriesApi = {
  getAll: () => api.get('/categories'),
  getById: (id) => api.get(`/categories/${id}`),
  create: (category) => api.post('/categories', category),
  update: (id, category) => api.put(`/categories/${id}`, category),
  delete: (id) => api.delete(`/categories/${id}`),
}

export default api
