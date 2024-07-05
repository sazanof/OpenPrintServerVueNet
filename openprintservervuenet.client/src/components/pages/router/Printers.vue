<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import { createSuccessNotification } from '@/js/helpers/notificationHelper.js'
import PrinterItem from '@/components/chunks/PrinterItem.vue'

export default {
    name: 'Printers',
    components: {
        PrinterItem,
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
            <PrinterItem
                v-for="printer in printers"
                :key="printer.id"
                :printer="printer" />
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