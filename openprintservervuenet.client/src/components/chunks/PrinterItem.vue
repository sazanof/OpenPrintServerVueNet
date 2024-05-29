<script>
import ConsumableItem from '@/components/chunks/ConsumableItem.vue'
import { de } from 'vuetify/locale'

export default {
    name: 'PrinterItem',
    components: { ConsumableItem },
    props: {
        printer: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            details: false
        }
    },
    computed: {
        de() {
            return de
        }
    }
}
</script>

<template>
    <VCard
        :height="190"
        class="printer-card mb-6 mr-6 pa-4">
        <div class="icon mr-2">
            <VIcon
                :size="70"
                icon="mdi-printer" />
        </div>

        <div class="info">
            <div class="text-capitalize font-weight-bold">
                {{ printer.Name }}
            </div>

            <div class="driver text-caption text-blue-grey-darken-1">
                {{ $t('Driver') }}: {{ printer.DriverName }}
            </div>
            <div class="host text-caption text-blue-grey-darken-1">
                {{ $t('Host') }}: {{ printer.SnmpFQDN }}
            </div>
            <div
                v-if="printer.Ports"
                class="ports">
                <div
                    v-for="port in printer.Ports"
                    :key="port.Id"
                    class="port">
                    <VChip
                        class="mt-2 mr-2"
                        density="compact"
                        color="success"
                        :text="port.HostAddress">
                        <template #prepend>
                            <VIcon icon="mdi-ip" />
                        </template>
                    </VChip>
                    <VChip
                        v-if="port.MacAddress && port.MacAddress.length > 0"
                        class="mt-2 mr-2"
                        density="compact"
                        color="success"
                        :text="port.MacAddress">
                        <template #prepend>
                            <VIcon icon="mdi-network" />
                        </template>
                    </VChip>
                    <Teleport to="body">
                        <VDialog
                            v-model="details"
                            width="400">
                            <VCard>
                                <template #title>
                                    {{ $t('Consumables statistics') }}
                                </template>
                                <template #text>
                                    <div class="mb-3">
                                        {{ $t('Total counter') }}: {{ printer.SnmpCountTotal }}
                                    </div>
                                    <div class="mb-3">
                                        {{ $t('Uptime counter') }}: {{ printer.SnmpCountTotal }}
                                    </div>
                                    <div class="mb-3">
                                        {{ $t('Uptime') }}: {{ printer.SnmpUptime }}
                                    </div>
                                    <ConsumableItem
                                        v-for="cons in printer.Consumables"
                                        :key="cons.Id"
                                        :consumable="cons" />
                                </template>
                            </VCard>
                        </VDialog>
                    </Teleport>
                </div>
            </div>
            <div class="actions">
                <VBtn
                    variant="plain"
                    icon="mdi-format-color-fill"
                    color="black"
                    @click="details = !details" />
                <VBtn
                    variant="plain"
                    icon="mdi-trash-can"
                    color="error" />
            </div>
        </div>
    </VCard>
</template>

<style scoped lang="scss">
.printer-card {
    width: 15%;
    max-width: 380px;
    min-width: 300px;
    align-items: flex-start;
    display: inline-flex;
    position: relative;

    .actions {
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        z-index: 10;
        display: flex;
        justify-content: space-between;
    }
}
</style>