<script>

import UserDropdown from '@/components/chunks/UserDropdown.vue'

export default {
    name: 'AppHeader',
    components: { UserDropdown },
    computed: {
        isConnected() {
            return this.$store.getters['isConnected']
        },
        user() {
            return this.$store.getters['getUser']
        },
        config() {
            return this.$store.getters['getConfig'] ?? []
        },
        appTitle() {
            return this.config.find(c => c.Key === 'app.title')
        }
    }
}
</script>

<template>
    <VAppBar
        scroll-behavior="elevate">
        <template #title>
            {{ appTitle !== undefined && appTitle?.Value !== null ? appTitle.Value : $t('Open Print Server') }}
        </template>
        <template #append>
            <VBtn
                icon="mdi-connection"
                :color="isConnected ? 'success' : 'warning'" />
            <VBtn
                icon="mdi-logout"
                @click="$store.dispatch('logOut')" />
            <UserDropdown />
        </template>
    </VAppBar>
</template>

<style scoped lang="scss">

</style>