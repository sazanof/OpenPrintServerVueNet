<script>
import Logo from '@/components/chunks/Logo.vue'

export default {
    name: 'AppNavigationDrawer',
    components: { Logo },
    data() {
        return {
            drawer: true
        }
    }
}
</script>

<template>
    <VNavigationDrawer
        v-model="drawer"
        class="pa-1"
        permanent
        rail
        rail-width="56"
        border="0">
        <VBtn
            :to="{name:'dashboard'}"
            icon="mdi-view-dashboard" />
        <VBtn
            :to="{name:'printers'}"
            icon="mdi-printer" />
        <VBtn
            :to="{name:'jobs'}"
            icon="mdi-tray-full" />
        <template #prepend>
            <Logo
                :width="40"
                class="px-1 py-3" />
        </template>
    </VNavigationDrawer>
</template>

<style scoped lang="scss">

</style>