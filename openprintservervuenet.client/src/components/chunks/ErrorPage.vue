<script>
export default {
    name: 'ErrorPage',
    props: {
        text: {
            type: String,
            default: null
        }
    },
    computed: {
        lastError() {
            return this.$store.getters['getLastError']
        }
    }
}
</script>

<template>
    <div class="error">
        <VSheet
            max-width="500"
            class="pa-6 details"
            rounded="md">
            <div class="text-h6">
                {{ text }}
            </div>
            <pre>{{ lastError }}</pre>
        </VSheet>
    </div>
</template>

<style scoped lang="scss">
.error {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
}
</style>