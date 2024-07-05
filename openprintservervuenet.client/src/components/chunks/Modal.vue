<script>
export default {
    name: 'Modal',
    props: {
        title: {
            type: String,
            default: null
        },
        subtitle: {
            type: String,
            default: null
        },
        width: {
            type: Number,
            default: 500
        }
    },
    data() {
        return {
            model: false
        }
    },
    methods: {
        show() {
            this.model = true
        },
        hide() {
            this.model = false
        }
    }
}
</script>

<template>
    <VDialog
        v-model="model"
        :width="width">
        <VCard>
            <VCardTitle v-if="title">
                {{ title }}
            </VCardTitle>
            <VCardSubtitle v-if="subtitle">
                {{ subtitle }}
            </VCardSubtitle>
            <VCardText>
                <slot />
            </VCardText>

            <VCardActions v-if="$slots.actions">
                <slot name="actions" />
            </VCardActions>
        </VCard>
    </VDialog>
</template>

<style scoped lang="scss">

</style>