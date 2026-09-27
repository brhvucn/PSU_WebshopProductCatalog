---
domain: frontend
capabilities:
  - examples
keywords:
  - loading banner
  - component
  - vue
priority: medium
cost: low
---

<template>
    <transition name="fold-down">
        <div v-if="props.visible" class="loading-banner notification is-success is-light has-text-centered">
            <span class="loader"></span>
            <span class="loading-text">{{ props.message }}</span>
        </div>
    </transition>
</template>

<script setup>
const props = defineProps({
    visible: { type: Boolean, default: false },
    message: { type: String, default: 'Indlæser...' }
})
</script>

<style scoped>
.loading-banner {
    position: fixed;
    top: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 200px;
    padding: 0.5rem 1rem;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 0.5rem;
    z-index: 9999;
    border-radius: 8px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

/* Spinner */
.loader {
    border: 2px solid rgba(0, 0, 0, 0.1);
    border-top: 2px solid #48c78e;
    /* Bulma success grøn */
    border-radius: 50%;
    width: 14px;
    height: 14px;
    animation: spin 0.8s linear infinite;
}

@keyframes spin {
    to {
        transform: rotate(360deg);
    }
}

/* Fold-down animation */
.fold-down-enter-active,
.fold-down-leave-active {
    transition: transform 0.4s ease, opacity 0.4s ease;
    transform-origin: top center;
}

.fold-down-enter-from,
.fold-down-leave-to {
    transform: translateY(-50%) scaleY(0);
    opacity: 0;
}
</style>