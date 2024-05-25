<script>

export default {
    name: 'AppHeader',
    components: {},
    computed: {
        isConnected() {
            return this.$store.getters['isConnected']
        }
    }
}
</script>

<template>
    <VAppBar
        scroll-behavior="elevate">
        <template #title>
            {{ $t('Open Print Server') }}
        </template>
        <template #append>
            <VBtn
                icon="mdi-connection"
                :color="isConnected ? 'success' : 'warning'" />
            <VBtn
                icon="mdi-logout"
                @click="$store.dispatch('logOut')" />
        </template>
    </VAppBar>
</template>

<style scoped lang="scss">

</style>