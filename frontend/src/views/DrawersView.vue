<template>
  <div class="drawers-view">
    <div class="header">
      <h1>Drawers</h1>
      <button @click="showAddModal = true" class="btn-primary">Add Drawer</button>
    </div>

    <div v-if="loading" class="loading">Loading...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else class="drawers-grid">
      <div v-for="drawer in drawers" :key="drawer.id" class="drawer-card">
        <h3>{{ drawer.name }}</h3>
        <p class="type-badge">{{ getTypeLabel(drawer.type) }}</p>
        <p class="location">{{ drawer.location || 'No location' }}</p>
        <p class="grid-position" v-if="drawer.type === 0">
          Grid: ({{ drawer.gridX }},{{ drawer.gridY }}) Size: {{ drawer.gridWidth }}×{{ drawer.gridHeight }}
        </p>
        <p class="description">{{ drawer.description || 'No description' }}</p>
        <p class="parts-count">Parts: {{ drawer.parts?.length || 0 }}</p>
        <div class="actions">
          <button @click="editDrawer(drawer)" class="btn-edit">Edit</button>
          <button @click="deleteDrawer(drawer.id)" class="btn-delete">Delete</button>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <div v-if="showAddModal || showEditModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <h2>{{ showEditModal ? 'Edit Drawer' : 'Add Drawer' }}</h2>
        <form @submit.prevent="saveDrawer">
          <div class="form-group">
            <label>Name *</label>
            <input v-model="currentDrawer.name" type="text" required />
          </div>
          <div class="form-group">
            <label>Location</label>
            <input v-model="currentDrawer.location" type="text" />
          </div>
          <div class="form-group">
            <label>Type *</label>
            <select v-model.number="currentDrawer.type" required>
              <option :value="0">Gridfinity Drawer</option>
              <option :value="1">Shelf</option>
              <option :value="2">Box</option>
              <option :value="3">Cabinet</option>
              <option :value="4">Off-Site</option>
              <option :value="5">Other</option>
            </select>
          </div>
          <div v-if="currentDrawer.type === 0" class="grid-fields">
            <div class="form-group">
              <label>Grid X (1-9) *</label>
              <input v-model.number="currentDrawer.gridX" type="number" required min="1" max="9" />
            </div>
            <div class="form-group">
              <label>Grid Y (1-5) *</label>
              <input v-model.number="currentDrawer.gridY" type="number" required min="1" max="5" />
            </div>
            <div class="form-group">
              <label>Width *</label>
              <input v-model.number="currentDrawer.gridWidth" type="number" required min="1" max="9" />
            </div>
            <div class="form-group">
              <label>Height *</label>
              <input v-model.number="currentDrawer.gridHeight" type="number" required min="1" max="5" />
            </div>
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="currentDrawer.description" rows="3"></textarea>
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
import { drawersApi } from '../services/api'

export default {
  name: 'DrawersView',
  data() {
    return {
      drawers: [],
      loading: true,
      error: null,
      showAddModal: false,
      showEditModal: false,
      currentDrawer: this.getEmptyDrawer(),
    }
  },
  mounted() {
    this.loadDrawers()
  },
  methods: {
    async loadDrawers() {
      try {
        this.loading = true
        const response = await drawersApi.getAll()
        this.drawers = response.data
      } catch (err) {
        this.error = 'Failed to load drawers: ' + err.message
      } finally {
        this.loading = false
      }
    },
    getEmptyDrawer() {
      return {
        name: '',
        location: '',
        type: 0,
        gridX: 1,
        gridY: 1,
        gridWidth: 1,
        gridHeight: 1,
        description: '',
      }
    },
    getTypeLabel(type) {
      const types = ['Gridfinity Drawer', 'Shelf', 'Box', 'Cabinet', 'Off-Site', 'Other']
      return types[type] || 'Unknown'
    },
    editDrawer(drawer) {
      this.currentDrawer = { ...drawer }
      this.showEditModal = true
    },
    async saveDrawer() {
      try {
        if (this.showEditModal) {
          await drawersApi.update(this.currentDrawer.id, this.currentDrawer)
        } else {
          await drawersApi.create(this.currentDrawer)
        }
        await this.loadDrawers()
        this.closeModal()
      } catch (err) {
        alert('Failed to save drawer: ' + err.message)
      }
    },
    async deleteDrawer(id) {
      if (confirm('Are you sure you want to delete this drawer?')) {
        try {
          await drawersApi.delete(id)
          await this.loadDrawers()
        } catch (err) {
          alert('Failed to delete drawer: ' + err.message)
        }
      }
    },
    closeModal() {
      this.showAddModal = false
      this.showEditModal = false
      this.currentDrawer = this.getEmptyDrawer()
    },
  },
}
</script>

<style scoped>
.drawers-view {
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

.drawers-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(min(100%, 280px), 1fr));
  gap: clamp(1rem, 3vw, 1.5rem);
}

.drawer-card {
  background: white;
  padding: clamp(1rem, 3vw, 1.25rem);
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.2s;
}

.drawer-card:hover {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
}

.drawer-card h3 {
  margin: 0 0 0.625rem 0;
  color: #333;
  font-size: clamp(1.125rem, 3vw, 1.25rem);
}

.type-badge {
  display: inline-block;
  background: #e3f2fd;
  color: #1976d2;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 500;
  margin: 0.3125rem 0;
}

.location {
  color: #666;
  margin: 0.3125rem 0;
  font-size: 0.875rem;
}

.grid-position {
  color: #42b983;
  font-weight: 500;
  margin: 0.3125rem 0;
  font-size: 0.875rem;
}

.grid-fields {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  gap: 0.625rem;
}

.description {
  color: #666;
  margin: 0.625rem 0;
  font-size: 0.875rem;
  line-height: 1.4;
}

.parts-count {
  color: #4a90e2;
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
.form-group select,
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

  .grid-fields {
    grid-template-columns: 1fr 1fr;
  }
}
</style>
