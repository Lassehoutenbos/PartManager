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
        <p class="location">{{ drawer.location || 'No location' }}</p>
        <p class="grid-position">
          Grid: Row {{ drawer.gridRow }}, Col {{ drawer.gridColumn }}
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
            <label>Grid Row *</label>
            <input v-model.number="currentDrawer.gridRow" type="number" required min="1" />
          </div>
          <div class="form-group">
            <label>Grid Column *</label>
            <input v-model.number="currentDrawer.gridColumn" type="number" required min="1" />
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
        gridRow: 1,
        gridColumn: 1,
        description: '',
      }
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
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.drawers-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 20px;
}

.drawer-card {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.drawer-card h3 {
  margin: 0 0 10px 0;
  color: #333;
}

.location {
  color: #666;
  margin: 5px 0;
}

.grid-position {
  color: #42b983;
  font-weight: 500;
  margin: 5px 0;
}

.description {
  color: #666;
  margin: 10px 0;
  font-size: 14px;
}

.parts-count {
  color: #4a90e2;
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
