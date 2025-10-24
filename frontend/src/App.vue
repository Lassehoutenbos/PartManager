<script setup>
import { ref } from 'vue'
import { RouterLink, RouterView } from 'vue-router'

const mobileMenuOpen = ref(false)

const toggleMobileMenu = () => {
  mobileMenuOpen.value = !mobileMenuOpen.value
}

const closeMobileMenu = () => {
  mobileMenuOpen.value = false
}
</script>

<template>
  <div id="app">
    <nav class="main-nav">
      <div class="nav-container">
        <h1 class="app-title">PartManager</h1>
        <button class="mobile-menu-toggle" @click="toggleMobileMenu" aria-label="Toggle menu">
          <span></span>
          <span></span>
          <span></span>
        </button>
        <div class="nav-links" :class="{ 'mobile-open': mobileMenuOpen }">
          <RouterLink to="/" @click="closeMobileMenu">Parts</RouterLink>
          <RouterLink to="/drawers" @click="closeMobileMenu">Drawers</RouterLink>
          <RouterLink to="/categories" @click="closeMobileMenu">Categories</RouterLink>
        </div>
      </div>
    </nav>

    <main>
      <RouterView />
    </main>
  </div>
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial,
    sans-serif;
  background: #f5f5f5;
  color: #333;
}

#app {
  min-height: 100vh;
}

.main-nav {
  background: #2c3e50;
  color: white;
  padding: 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

.nav-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 1rem clamp(1rem, 3vw, 2rem);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.app-title {
  font-size: clamp(1.25rem, 4vw, 1.5rem);
  font-weight: 600;
  color: #42b983;
  margin: 0;
}

.mobile-menu-toggle {
  display: none;
  flex-direction: column;
  gap: 0.25rem;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.5rem;
}

.mobile-menu-toggle span {
  width: 1.5rem;
  height: 2px;
  background: white;
  transition: all 0.3s ease;
  border-radius: 2px;
}

.nav-links {
  display: flex;
  gap: 2rem;
}

.nav-links a {
  color: white;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s;
  white-space: nowrap;
}

.nav-links a:hover {
  color: #42b983;
}

.nav-links a.router-link-exact-active {
  color: #42b983;
  border-bottom: 2px solid #42b983;
  padding-bottom: 0.25rem;
}

main {
  max-width: 1400px;
  margin: 0 auto;
}

/* Mobile styles */
@media (max-width: 768px) {
  .mobile-menu-toggle {
    display: flex;
  }

  .nav-links {
    position: fixed;
    top: 4rem;
    right: -100%;
    width: 70%;
    max-width: 300px;
    background: #2c3e50;
    flex-direction: column;
    padding: 2rem;
    gap: 1.5rem;
    box-shadow: -2px 0 8px rgba(0, 0, 0, 0.2);
    transition: right 0.3s ease;
    height: calc(100vh - 4rem);
  }

  .nav-links.mobile-open {
    right: 0;
  }

  .nav-links a {
    font-size: 1.125rem;
    padding: 0.5rem 0;
  }

  .nav-links a.router-link-exact-active {
    border-bottom: none;
    border-left: 3px solid #42b983;
    padding-left: 1rem;
  }
}
</style>
