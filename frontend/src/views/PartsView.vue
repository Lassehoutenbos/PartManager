<template>
  <div class="parts-view">
    <div class="header">
      <h1>Electronic Parts</h1>
      <button @click="showAddModal = true" class="btn-primary">Add Part</button>
    </div>

    <div class="filters">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search parts..."
        class="search-input"
      />
      <select v-model="filterCategory" class="filter-select">
        <option value="">All Categories</option>
        <option v-for="category in categories" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>
    </div>

    <div v-if="loading" class="loading">Loading...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else>
      <table class="parts-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Part Number</th>
            <th>Manufacturer</th>
            <th>Category</th>
            <th>Drawer</th>
            <th>Quantity</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="part in filteredParts" :key="part.id">
            <td>{{ part.name }}</td>
            <td>{{ part.partNumber || '-' }}</td>
            <td>{{ part.manufacturer || '-' }}</td>
            <td>{{ part.category?.name || '-' }}</td>
            <td>{{ part.drawer?.name || '-' }}</td>
            <td>{{ part.quantity }}</td>
            <td>
              <button @click="editPart(part)" class="btn-edit">Edit</button>
              <button @click="deletePart(part.id)" class="btn-delete">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Add/Edit Modal -->
    <div v-if="showAddModal || showEditModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <h2>{{ showEditModal ? 'Edit Part' : 'Add Part' }}</h2>
        <form @submit.prevent="savePart">
          <div class="form-group">
            <label>Name *</label>
            <input v-model="currentPart.name" type="text" required />
          </div>
          <div class="form-group">
            <label>Part Number</label>
            <input v-model="currentPart.partNumber" type="text" />
          </div>
          <div class="form-group">
            <label>Manufacturer</label>
            <input v-model="currentPart.manufacturer" type="text" />
          </div>
          <div class="form-group">
            <label>Category</label>
            <select v-model="currentPart.categoryId">
              <option :value="null">None</option>
              <option v-for="category in categories" :key="category.id" :value="category.id">
                {{ category.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Drawer</label>
            <select v-model="currentPart.drawerId">
              <option :value="null">None</option>
              <option v-for="drawer in drawers" :key="drawer.id" :value="drawer.id">
                {{ drawer.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Quantity *</label>
            <input v-model.number="currentPart.quantity" type="number" required min="0" />
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="currentPart.description" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Datasheet URL</label>
            <input v-model="currentPart.datasheet" type="url" />
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
import { partsApi, categoriesApi, drawersApi } from '../services/api'

export default {
  name: 'PartsView',
  data() {
    return {
      parts: [],
      categories: [],
      drawers: [],
      loading: true,
      error: null,
      searchQuery: '',
      filterCategory: '',
      showAddModal: false,
      showEditModal: false,
      currentPart: this.getEmptyPart(),
    }
  },
  computed: {
    filteredParts() {
      let filtered = this.parts

      if (this.searchQuery) {
        const query = this.searchQuery.toLowerCase()
        filtered = filtered.filter(
          (part) =>
            part.name.toLowerCase().includes(query) ||
            (part.partNumber && part.partNumber.toLowerCase().includes(query)) ||
            (part.manufacturer && part.manufacturer.toLowerCase().includes(query))
        )
      }

      if (this.filterCategory) {
        filtered = filtered.filter((part) => part.categoryId === this.filterCategory)
      }

      return filtered
    },
  },
  mounted() {
    this.loadData()
  },
  methods: {
    async loadData() {
      try {
        this.loading = true
        const [partsRes, categoriesRes, drawersRes] = await Promise.all([
          partsApi.getAll(),
          categoriesApi.getAll(),
          drawersApi.getAll(),
        ])
        this.parts = partsRes.data
        this.categories = categoriesRes.data
        this.drawers = drawersRes.data
      } catch (err) {
        this.error = 'Failed to load data: ' + err.message
      } finally {
        this.loading = false
      }
    },
    getEmptyPart() {
      return {
        name: '',
        partNumber: '',
        manufacturer: '',
        description: '',
        datasheet: '',
        quantity: 0,
        categoryId: null,
        drawerId: null,
      }
    },
    editPart(part) {
      this.currentPart = { ...part }
      this.showEditModal = true
    },
    async savePart() {
      try {
        if (this.showEditModal) {
          await partsApi.update(this.currentPart.id, this.currentPart)
        } else {
          await partsApi.create(this.currentPart)
        }
        await this.loadData()
        this.closeModal()
      } catch (err) {
        alert('Failed to save part: ' + err.message)
      }
    },
    async deletePart(id) {
      if (confirm('Are you sure you want to delete this part?')) {
        try {
          await partsApi.delete(id)
          await this.loadData()
        } catch (err) {
          alert('Failed to delete part: ' + err.message)
        }
      }
    },
    closeModal() {
      this.showAddModal = false
      this.showEditModal = false
      this.currentPart = this.getEmptyPart()
    },
  },
}
</script>

<style scoped>
.parts-view {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.filters {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

.search-input {
  flex: 1;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.filter-select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.parts-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.parts-table th,
.parts-table td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.parts-table th {
  background: #f5f5f5;
  font-weight: 600;
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
  background: #4a90e2;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 5px;
}

.btn-delete {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 6px 12px;
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
  max-height: 90vh;
  overflow-y: auto;
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
.form-group select,
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
