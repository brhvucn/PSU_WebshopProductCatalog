---
domain: frontend
capabilities:
  - examples
keywords:
  - title
  - component
  - vue
priority: medium
cost: low
---

<template>
    <div class="level mb-4">
        <!-- Venstre side: titel -->
        <div class="level-left">
            <div class="level-item">
                <h1 class="title is-4">{{ props.title }}</h1>
            </div>
        </div>

        <!-- Højre side: knap (kun hvis showAdd er true) -->
        <div class="level-right" v-if="showAdd">
            <div class="level-item">
                <button class="button is-primary" @click="emitAddElementEvent">
                    <span class="material-symbols-outlined" style="vertical-align: middle;">add</span>
                    <span>Tilføj ny</span>
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
const props = defineProps({
    title: { type: String, default: 'Ukendt overskrift' },
    showAdd: { type: Boolean, default: false },
})

const emit = defineEmits(['addElement'])

function emitAddElementEvent() {
    emit('addElement');
}
</script>