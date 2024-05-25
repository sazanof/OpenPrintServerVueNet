<script>
import AppHeader from '@/components/chunks/AppHeader.vue'
import AppNavigationDrawer from '@/components/chunks/AppNavigationDrawer.vue'

export default {
    name: 'Page',
    components: {
        AppNavigationDrawer,
        AppHeader
    },
    computed: {
        signal() {
            return this.$store.getters['getSignalR']
        }
    },
    created() {
        this.$store.dispatch('createSignalR')
    }

}
</script>

<template>
    <VMain>
        <AppNavigationDrawer />
        <AppHeader />
        <VLayout
            class="pa-4"
            full-height>
            <VSheet
                rounded
                color="bg-blue-grey-lighten-5"
                class="pa-6"
                height="100%"
                width="100%">
                <router-view />
            </VSheet>
        </VLayout>
    </VMain>
</template>

<style scoped lang="scss">
.content {
    position: absolute;
    left: 10px;
    top: 10px;
    right: 10px;
    bottom: 10px;
}
</style>