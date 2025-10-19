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
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.categories-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 20px;
}

.category-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.category-card h3 {
  margin: 0 0 10px 0;
  color: #333;
}

.description {
  color: #666;
  margin: 10px 0;
  font-size: 14px;
}

.parts-count {
  color: #42b983;
  font-weight: 500;
  margin: 10px 0;
}

.actions {
  display: flex;
  gap: 10px;
  margin-top: 15px;
}

.btn-primary {
  background: #42b983;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary:hover {
  background: #359268;
}

.btn-edit {
  flex: 1;
  background: #4a90e2;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-delete {
  flex: 1;
  background: #e74c3c;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
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
}

.modal {
  background: white;
  padding: 30px;
  border-radius: 8px;
  width: 90%;
  max-width: 500px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-sizing: border-box;
}

.form-actions {
  display: flex;
  gap: 10px;
  margin-top: 20px;
}

.btn-secondary {
  background: #95a5a6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

.loading,
.error {
  text-align: center;
  padding: 20px;
}

.error {
  color: #e74c3c;
}
</style>
