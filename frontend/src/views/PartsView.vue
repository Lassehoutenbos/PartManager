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
      <!-- Desktop Table View -->
      <table class="parts-table desktop-only">
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
              <button @click="viewPart(part)" class="btn-view">View</button>
              <button @click="editPart(part)" class="btn-edit">Edit</button>
              <button @click="deletePart(part.id)" class="btn-delete">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Mobile Card View -->
      <div class="parts-cards mobile-only">
        <div v-for="part in filteredParts" :key="part.id" class="part-card">
          <h3 class="part-name">{{ part.name }}</h3>
          <div class="part-details">
            <div class="detail-row" v-if="part.partNumber">
              <span class="detail-label">Part #:</span>
              <span class="detail-value">{{ part.partNumber }}</span>
            </div>
            <div class="detail-row" v-if="part.manufacturer">
              <span class="detail-label">Manufacturer:</span>
              <span class="detail-value">{{ part.manufacturer }}</span>
            </div>
            <div class="detail-row" v-if="part.category?.name">
              <span class="detail-label">Category:</span>
              <span class="detail-value">{{ part.category.name }}</span>
            </div>
            <div class="detail-row" v-if="part.drawer?.name">
              <span class="detail-label">Drawer:</span>
              <span class="detail-value">{{ part.drawer.name }}</span>
            </div>
            <div class="detail-row">
              <span class="detail-label">Quantity:</span>
              <span class="detail-value">{{ part.quantity }}</span>
            </div>
          </div>
          <div class="part-actions">
            <button @click="viewPart(part)" class="btn-view">View</button>
            <button @click="editPart(part)" class="btn-edit">Edit</button>
            <button @click="deletePart(part.id)" class="btn-delete">Delete</button>
          </div>
        </div>
      </div>
    </div>

    <!-- View Modal -->
    <div v-if="showViewModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal view-modal">
        <h2>{{ viewedPart?.name }}</h2>
        <div class="detail-grid">
          <div class="detail-item">
            <strong>Part Number:</strong> {{ viewedPart?.partNumber || '-' }}
          </div>
          <div class="detail-item">
            <strong>Manufacturer:</strong> {{ viewedPart?.manufacturer || '-' }}
          </div>
          <div class="detail-item">
            <strong>MPN:</strong> {{ viewedPart?.manufacturerPartNumber || '-' }}
          </div>
          <div class="detail-item">
            <strong>Package:</strong> {{ viewedPart?.package || '-' }}
          </div>
          <div class="detail-item">
            <strong>Footprint:</strong> {{ viewedPart?.footprint || '-' }}
          </div>
          <div class="detail-item">
            <strong>Value:</strong> {{ viewedPart?.value || '-' }}
          </div>
          <div class="detail-item">
            <strong>Voltage:</strong> {{ viewedPart?.voltage || '-' }}
          </div>
          <div class="detail-item">
            <strong>Quantity:</strong> {{ viewedPart?.quantity }}
          </div>
          <div class="detail-item">
            <strong>Category:</strong> {{ viewedPart?.category?.name || '-' }}
          </div>
          <div class="detail-item">
            <strong>Drawer:</strong> {{ viewedPart?.drawer?.name || '-' }}
          </div>
          <div class="detail-item full-width" v-if="viewedPart?.description">
            <strong>Description:</strong> {{ viewedPart?.description }}
          </div>
          <div class="detail-item full-width" v-if="viewedPart?.notes">
            <strong>Notes:</strong> {{ viewedPart?.notes }}
          </div>
          <div class="detail-item full-width" v-if="viewedPart?.nfcTagId">
            <strong>NFC Tag:</strong> {{ viewedPart?.nfcTagId }}
          </div>
          <div class="detail-item full-width" v-if="viewedPart?.qrCode">
            <strong>QR Code:</strong> {{ viewedPart?.qrCode }}
          </div>
        </div>
        <div class="form-actions">
          <button @click="closeModal" class="btn-secondary">Close</button>
        </div>
      </div>
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
            <label>Package</label>
            <input v-model="currentPart.package" type="text" placeholder="e.g., DIP-8, SOIC-16" />
          </div>
          <div class="form-group">
            <label>Footprint</label>
            <input v-model="currentPart.footprint" type="text" placeholder="e.g., SOT-23" />
          </div>
          <div class="form-group">
            <label>Value</label>
            <input v-model="currentPart.value" type="text" placeholder="e.g., 10K, 100nF" />
          </div>
          <div class="form-group">
            <label>Voltage/Current/Power</label>
            <input v-model="currentPart.voltage" type="text" placeholder="e.g., 5V" />
          </div>
          <div class="form-group">
            <label>Notes</label>
            <textarea v-model="currentPart.notes" rows="2"></textarea>
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
      showViewModal: false,
      currentPart: this.getEmptyPart(),
      viewedPart: null,
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
        manufacturerPartNumber: '',
        description: '',
        package: '',
        footprint: '',
        value: '',
        voltage: '',
        notes: '',
        quantity: 0,
        minQuantity: null,
        categoryId: null,
        drawerId: null,
      }
    },
    viewPart(part) {
      this.viewedPart = part
      this.showViewModal = true
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
      this.showViewModal = false
      this.currentPart = this.getEmptyPart()
      this.viewedPart = null
    },
  },
}
</script>

<style scoped>
.parts-view {
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

.filters {
  display: flex;
  gap: 0.625rem;
  margin-bottom: clamp(1rem, 3vw, 1.5rem);
  flex-wrap: wrap;
}

.search-input {
  flex: 1;
  min-width: 200px;
  padding: 0.5rem 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

.filter-select {
  padding: 0.5rem 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  min-width: 150px;
}

.parts-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  overflow: hidden;
}

.parts-table th,
.parts-table td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.parts-table th {
  background: #f5f5f5;
  font-weight: 600;
  font-size: 0.875rem;
}

.parts-table td {
  font-size: 0.875rem;
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

.btn-view {
  background: #9c27b0;
  color: white;
  border: none;
  padding: 0.375rem 0.75rem;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 0.3125rem;
  font-size: 0.875rem;
  transition: opacity 0.2s;
}

.btn-view:hover {
  opacity: 0.9;
}

.btn-edit {
  background: #4a90e2;
  color: white;
  border: none;
  padding: 0.375rem 0.75rem;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 0.3125rem;
  font-size: 0.875rem;
  transition: opacity 0.2s;
}

.btn-edit:hover {
  opacity: 0.9;
}

.btn-delete {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 0.375rem 0.75rem;
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

.view-modal {
  max-width: 700px;
}

.detail-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 0.9375rem;
  margin: 1.25rem 0;
}

.detail-item {
  padding: 0.625rem;
  background: #f9f9f9;
  border-radius: 4px;
}

.detail-item.full-width {
  grid-column: 1 / -1;
}

.detail-item strong {
  display: block;
  color: #555;
  font-size: 0.75rem;
  margin-bottom: 0.25rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* Show/hide based on screen size */
.desktop-only {
  display: table;
}

.mobile-only {
  display: none;
}

/* Mobile Card View Styles */
.parts-cards {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1rem;
}

.part-card {
  background: white;
  border-radius: 8px;
  padding: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.part-name {
  font-size: 1.125rem;
  margin: 0 0 0.75rem 0;
  color: #333;
}

.part-details {
  margin-bottom: 0.75rem;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  padding: 0.375rem 0;
  border-bottom: 1px solid #f0f0f0;
}

.detail-row:last-child {
  border-bottom: none;
}

.detail-label {
  font-weight: 500;
  color: #666;
  font-size: 0.875rem;
}

.detail-value {
  color: #333;
  font-size: 0.875rem;
  text-align: right;
}

.part-actions {
  display: flex;
  gap: 0.5rem;
  margin-top: 0.75rem;
}

.part-actions button {
  flex: 1;
}

/* Mobile responsive table - convert to cards */
@media (max-width: 768px) {
  .desktop-only {
    display: none;
  }

  .mobile-only {
    display: grid;
  }
}

/* Tablet adjustments */
@media (max-width: 1024px) {
  .parts-table td,
  .parts-table th {
    padding: 0.5rem;
    font-size: 0.8125rem;
  }

  .btn-view,
  .btn-edit,
  .btn-delete {
    padding: 0.25rem 0.5rem;
    font-size: 0.8125rem;
    margin-right: 0.25rem;
  }

  .detail-grid {
    grid-template-columns: 1fr;
  }
}

/* Small mobile adjustments */
@media (max-width: 480px) {
  .header {
    flex-direction: column;
    align-items: stretch;
  }

  .header h1 {
    text-align: center;
  }

  .filters {
    flex-direction: column;
  }

  .search-input,
  .filter-select {
    width: 100%;
  }

  .form-actions {
    flex-direction: column;
  }

  .form-actions button {
    width: 100%;
  }
}
</style>
