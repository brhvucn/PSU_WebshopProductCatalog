---
domain: frontend
capabilities:
  - examples
keywords:
  - header
  - component
  - vue
priority: medium
cost: low
---

<template>
    <header class="header">
        <button class="burger-button" aria-label="Åbn menu" @click="$emit('toggleSidebar')">
            <span class="material-symbols-outlined">menu</span>
        </button>
    </header>
</template>

<script setup>
// ingen props lige nu
</script>

<style scoped>
.header {
    display: flex;
    align-items: center;
    /* padding: 1rem 1.5rem; */
    background-color: inherit;
    position: sticky;
    top: 0;
}

.burger-button {
    background: none;
    border: none;
    color: #1e2a38;
    font-size: 2rem;
    cursor: pointer;
}

.burger-button:hover {
    color: #00d1b2;
}

/* Vises kun på mobilskærme */
@media (min-width: 1025px) {
    .burger-button {
        display: none;
    }
}
</style>