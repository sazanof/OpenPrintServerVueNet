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
                    return 'blue-darker-4'
            }
        }
    }
}
</script>
<template>
    <div class="consumable mb-2">
        <div class="type">
            {{ $t(`Consumable_Type_${consumable.Type}`) }}
        </div>
        <div class="name">
            {{ consumable.Name }} ({{ value }}%)
        </div>
        <VProgressLinear
            class="mb-3"
            rounded="sm"
            height="6"
            :color="color"
            :model-value="value"
            :max="max" />
    </div>
</template>

<style scoped lang="scss">
.consumable {
    .name {
        font-weight: bold;
    }

    .type {
        font-size: var(--font-small);
    }
}
</style>