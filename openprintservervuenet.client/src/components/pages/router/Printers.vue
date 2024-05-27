<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import { createSuccessNotification } from '@/js/helpers/notificationHelper.js'

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
    },
    methods: {
        async sync() {
            await this.$store.dispatch('syncPrinters').then(() => {
                this.$store.commit('addNotification', createSuccessNotification(this.$t('Synced')))
            })
        }
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
                        {{ printer.Name }}
                    </template>
                    <template #subtitle>
                        <div class="driver">
                            {{ $t('Driver') }}: {{ printer.DriverName }}
                        </div>
                        <div
                            v-if="printer.Ports"
                            class="ports">
                            <VChip
                                v-for="port in printer.Ports"
                                :key="port.Id"
                                class="mt-2 mr-2"
                                density="compact"
                                color="success"
                                :text="port.Name">
                                <template #prepend>
                                    <VIcon icon="mdi-ip" />
                                </template>
                            </VChip>
                        </div>
                    </template>
                    <template #append>
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
                @click="sync" />
        </template>
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>