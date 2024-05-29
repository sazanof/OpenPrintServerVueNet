<script>
export default {
    name: 'ConsumableItem',

    props: {
        consumable: {
            type: Object,
            required: true
        }
    },
    computed: {
        value() {
            return this.consumable.Remains
        },
        max() {
            return this.consumable.Capacity
        },
        color() {
            switch (this.consumable.Color) {
                case 'black':
                    return 'black'
                case 'cyan':
                    return 'blue'
                case 'magenta':
                    return 'pink'
                case 'yellow':
                    return 'yellow'
                default:
                    return 'teal-darker-4'
            }
        }
    }
}
</script>
<template>
    <div>
        {{ consumable.Name }} ({{ value }}%)
    </div>
    <VProgressLinear
        class="mb-3"
        rounded="sm"
        height="6"
        v-bind="props"
        :color="color"
        :model-value="value"
        :max="max" />
</template>

<style scoped lang="scss">

</style>