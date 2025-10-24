<template>
  <div class="categories-view">
    <div class="header">
      <h1>Categories</h1>
      <button @click="showAddModal = true" class="btn-primary">Add Category</button>
    </div>

    <div v-if="loading" class="loading">Loading...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else class="categories-list">
      <div v-for="category in categories" :key="category.id" class="category-card">
        <h3>{{ category.name }}</h3>
        <p class="description">{{ category.description || 'No description' }}</p>
        <p class="parts-count">Parts: {{ category.parts?.length || 0 }}</p>
        <div class="actions">
          <button @click="editCategory(category)" class="btn-edit">Edit</button>
          <button @click="deleteCategory(category.id)" class="btn-delete">Delete</button>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <div v-if="showAddModal || showEditModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <h2>{{ showEditModal ? 'Edit Category' : 'Add Category' }}</h2>
        <form @submit.prevent="saveCategory">
          <div class="form-group">
            <label>Name *</label>
            <input v-model="currentCategory.name" type="text" required />
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="currentCategory.description" rows="3"></textarea>
          </div>
          <div class="form-actions">
            <button type="submit" class="btn-primary">Save</button>
            <button type="button" @click="closeModal" class="btn-secondary">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { categoriesApi } from '../services/api'

export default {
  name: 'CategoriesView',
  data() {
    return {
      categories: [],
      loading: true,
      error: null,
      showAddModal: false,
      showEditModal: false,
      currentCategory: this.getEmptyCategory(),
    }
  },
  mounted() {
    this.loadCategories()
  },
  methods: {
    async loadCategories() {
      try {
        this.loading = true
        const response = await categoriesApi.getAll()
        this.categories = response.data
      } catch (err) {
        this.error = 'Failed to load categories: ' + err.message
      } finally {
        this.loading = false
      }
    },
    getEmptyCategory() {
      return {
        name: '',
        description: '',
      }
    },
    editCategory(category) {
      this.currentCategory = { ...category }
      this.showEditModal = true
    },
    async saveCategory() {
      try {
        if (this.showEditModal) {
          await categoriesApi.update(this.currentCategory.id, this.currentCategory)
        } else {
          await categoriesApi.create(this.currentCategory)
        }
        await this.loadCategories()
        this.closeModal()
      } catch (err) {
        alert('Failed to save category: ' + err.message)
      }
    },
    async deleteCategory(id) {
      if (confirm('Are you sure you want to delete this category?')) {
        try {
          await categoriesApi.delete(id)
          await this.loadCategories()
        } catch (err) {
          alert('Failed to delete category: ' + err.message)
        }
      }
    },
    closeModal() {
      this.showAddModal = false
      this.showEditModal = false
      this.currentCategory = this.getEmptyCategory()
    },
  },
}
</script>

<style scoped>
.categories-view {
  padding: clamp(1rem, 3vw, 1.5rem);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: clamp(1rem, 3vw, 1.5rem);
  gap: 1rem;
  flex-wrap: wrap;
}

.header h1 {
  font-size: clamp(1.5rem, 4vw, 2rem);
}

.categories-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(min(100%, 280px), 1fr));
  gap: clamp(1rem, 3vw, 1.5rem);
}

.category-card {
  background: white;
  padding: clamp(1rem, 3vw, 1.25rem);
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.2s;
}

.category-card:hover {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
}

.category-card h3 {
  margin: 0 0 0.625rem 0;
  color: #333;
  font-size: clamp(1.125rem, 3vw, 1.25rem);
}

.description {
  color: #666;
  margin: 0.625rem 0;
  font-size: 0.875rem;
  line-height: 1.4;
}

.parts-count {
  color: #42b983;
  font-weight: 500;
  margin: 0.625rem 0;
  font-size: 0.875rem;
}

.actions {
  display: flex;
  gap: 0.625rem;
  margin-top: 0.9375rem;
}

.btn-primary {
  background: #42b983;
  color: white;
  border: none;
  padding: 0.625rem 1.25rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background 0.2s;
  white-space: nowrap;
}

.btn-primary:hover {
  background: #359268;
}

.btn-edit {
  flex: 1;
  background: #4a90e2;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.875rem;
  transition: opacity 0.2s;
}

.btn-edit:hover {
  opacity: 0.9;
}

.btn-delete {
  flex: 1;
  background: #e74c3c;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.875rem;
  transition: opacity 0.2s;
}

.btn-delete:hover {
  opacity: 0.9;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal {
  background: white;
  padding: clamp(1.25rem, 4vw, 1.875rem);
  border-radius: 8px;
  width: 100%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
}

.modal h2 {
  font-size: clamp(1.25rem, 4vw, 1.5rem);
  margin-bottom: 1rem;
}

.form-group {
  margin-bottom: 0.9375rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.3125rem;
  font-weight: 500;
  font-size: 0.9375rem;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-sizing: border-box;
  font-size: 1rem;
}

.form-actions {
  display: flex;
  gap: 0.625rem;
  margin-top: 1.25rem;
  flex-wrap: wrap;
}

.form-actions button {
  flex: 1;
  min-width: 100px;
}

.btn-secondary {
  background: #95a5a6;
  color: white;
  border: none;
  padding: 0.625rem 1.25rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background 0.2s;
}

.btn-secondary:hover {
  background: #7f8c8d;
}

.loading,
.error {
  text-align: center;
  padding: 1.25rem;
  font-size: 1rem;
}

.error {
  color: #e74c3c;
}

/* Mobile adjustments */
@media (max-width: 480px) {
  .header {
    flex-direction: column;
    align-items: stretch;
  }

  .header h1 {
    text-align: center;
  }

  .form-actions {
    flex-direction: column;
  }

  .form-actions button {
    width: 100%;
  }
}
</style>
