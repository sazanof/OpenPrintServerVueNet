<script>
export default {
    name: 'PageTemplate',
    props: {
        title: {
            type: String,
            default: null
        }
    },
    computed: {
        slotActions() {
            return !!this.$slots?.actions
        }
    }
}
</script>

<template>
    <VSheet>
        <div
            v-if="title"
            class="text-h5 title">
            {{ title }}
            <div
                v-if="slotActions"
                class="my-2 actions">
                <slot name="actions" />
            </div>
        </div>
        <div class="mt-2">
            <slot />
        </div>
    </VSheet>
</template>

<style scoped lang="scss">
.title {
    position: relative;

    .actions {
        position: absolute;
        right: 0;
        top: -18px;
        z-index: 10;
    }
}
</style>