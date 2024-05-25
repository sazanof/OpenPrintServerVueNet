<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'

export default {
    name: 'Printers',
    components: {
        PageTemplate
    },
    computed: {
        printers() {
            return this.$store.getters['getPrinters']
        },
        loading() {
            return this.$store.getters['isLoading']
        }
    },
    created() {
        this.$store.dispatch('getPrinters')
    }
}
</script>

<template>
    <PageTemplate :title="$t('Printers')">
        <template #default>
            <VList>
                <VListItem
                    v-for="printer in printers"
                    :key="printer.id"
                    :value="printer"
                    lines="three">
                    <template #title>
                        {{ printer.name }}
                    </template>
                    <template #subtitle>
                        <div class="driver">
                            {{ $t('Driver') }}: {{ printer.driverName }}
                        </div>
                        <div
                            v-if="printer.ports"
                            class="ports">
                            <VChip
                                v-for="port in printer.ports"
                                :key="port.id"
                                class="mt-2 mr-2"
                                density="compact"
                                color="success"
                                :text="port.name">
                                <template #prepend>
                                    <VIcon icon="mdi-ip" />
                                </template>
                            </VChip>
                        </div>
                    </template>
                    <template #append>
                        <VBtn icon="mdi-pencil" />
                        <VBtn
                            icon="mdi-trash-can"
                            color="error" />
                    </template>
                </VListItem>
            </VList>
        </template>
        <template #actions>
            <VBtn
                :disabled="loading"
                :loading="loading"
                :text="$t('Sync')"
                prepend-icon="mdi-sync"
                @click="$store.dispatch('syncPrinters')" />
        </template>
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>