<script>

import ConfirmationDialog from '@/components/chunks/ConfirmationDialog.vue'

export default {
    name: 'PrinterItem',
    components: { ConfirmationDialog },
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
    methods: {
        async deletePrinter() {
            const ok = await this.$refs.delPrinter.show({
                okButton: this.$t('Delete'),
                okColor: 'white',
                cancelColor: 'white',
                okIcon: 'mdi-trash-can',
                title: this.$t('Delete printer'),
                message: this.$t('Are you sure you want to delete printer? All printer jobs will be deleted too!')
            })

            if (ok) {
                await this.$store.dispatch('deletePrinter', this.printer.Id)
            }
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
                </div>
            </div>
            <div class="actions">
                <VBtn
                    variant="plain"
                    icon="mdi-eye"
                    color="black"
                    @click="$router.push({ name: 'printer', params:{id:printer.Id} })" />
                <VBtn
                    variant="plain"
                    icon="mdi-trash-can"
                    color="error"
                    @click="deletePrinter" />
            </div>
            <ConfirmationDialog
                ref="delPrinter"
                color="error" />
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