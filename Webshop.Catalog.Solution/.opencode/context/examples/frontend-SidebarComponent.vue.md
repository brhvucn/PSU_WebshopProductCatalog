---
domain: frontend
capabilities:
  - examples
keywords:
  - sidebar
  - component
  - vue
priority: medium
cost: low
---

<template>
<div class="sidebar-wrapper">
<div class="overlay" :class="{'is-visible':$props.isOpen}" @click="$emit('closeSidebar')"></div>
<aside class="sidebar" :class="{'is-visible':$props.isOpen,collapsed:isCollapsed}">
<h1 class="title">StudyFlow</h1>
<button class="collapse-btn" @click="isCollapsed=!isCollapsed"><span class="material-symbols-outlined">{{isCollapsed?'chevron_right':'chevron_left'}}</span></button>
<RouterLink to="/" class="sidebar-link" active-class="is-active"><span class="material-symbols-outlined">dashboard</span><span>Overview</span></RouterLink>
<a href="#" class="logout-btn" @click="useAuthStore().logout()"><span class="material-symbols-outlined">logout</span><span>Logout</span></a>
</aside>
</div>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'
defineEmits(['closeSidebar'])
defineProps({ isOpen: Boolean })
const isCollapsed = ref(false)
</script>

<style scoped>
.sidebar { width:240px;background:#1e2a38;color:#fff;display:flex;flex-direction:column;height:100vh;position:fixed;left:0;top:0;padding:1rem;transform:none;z-index:1001 }
.sidebar.collapsed { width:80px;align-items:center }
.sidebar.collapsed .sidebar-link,.sidebar.collapsed .logout-btn { justify-content:center }
.sidebar.collapsed .sidebar-link span:not(.material-symbols-outlined) { display:none }
.sidebar-link { color:#d1d9e6;display:flex;align-items:center;padding:.5rem .75rem;text-decoration:none }
.sidebar-link:hover,.sidebar-link.is-active { background:#273548;color:#00d1b2 }
.logout-btn { margin-top:auto;background:#273548;color:#d1d9e6;padding:.75rem 1rem;display:flex;align-items:center;text-decoration:none }
.overlay { position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,.4);z-index:1000;opacity:0;visibility:hidden }
.overlay.is-visible { opacity:1;visibility:visible }
@media(max-width:1024px){ .sidebar { transform:translateX(-100%) } .sidebar.is-visible { transform:translateX(0) } }
</style>
