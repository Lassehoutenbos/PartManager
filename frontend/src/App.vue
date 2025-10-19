<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div id="app">
    <nav class="main-nav">
      <div class="nav-container">
        <h1 class="app-title">PartManager</h1>
        <div class="nav-links">
          <RouterLink to="/">Parts</RouterLink>
          <RouterLink to="/drawers">Drawers</RouterLink>
          <RouterLink to="/categories">Categories</RouterLink>
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
}

.nav-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.app-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #42b983;
  margin: 0;
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
}

.nav-links a:hover {
  color: #42b983;
}

.nav-links a.router-link-exact-active {
  color: #42b983;
  border-bottom: 2px solid #42b983;
  padding-bottom: 4px;
}

main {
  max-width: 1200px;
  margin: 0 auto;
}
</style>
