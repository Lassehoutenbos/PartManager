import { createRouter, createWebHistory } from 'vue-router'
import PartsView from '../views/PartsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'parts',
      component: PartsView,
    },
    {
      path: '/drawers',
      name: 'drawers',
      component: () => import('../views/DrawersView.vue'),
    },
    {
      path: '/categories',
      name: 'categories',
      component: () => import('../views/CategoriesView.vue'),
    },
  ],
})

export default router
