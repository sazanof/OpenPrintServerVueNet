<script>
import { getPrinterStatus } from '@/js/helpers/printerStatus.js'

import ConsumableItem from '@/components/chunks/ConsumableItem.vue'
import PageTemplate from '@/components/pages/PageTemplate.vue'
import PrintJobsWrapper from '@/components/chunks/PrintJobsWrapper.vue'
import { createSuccessNotification } from '@/js/helpers/notificationHelper.js'

export default {
    name: 'Printer',
    components: { PrintJobsWrapper, PageTemplate, ConsumableItem },
    data() {
        return {
            printer: null,
            tab: null
        }
    },
    computed: {
        id() {
            return this.$route.params.id
        },
        status() {
            return this.$t(getPrinterStatus(this.printer.PrinterStatus))
        },
        loading() {
            return this.$store.getters['isLoading']
        }
    },
    async created() {
        this.printer = await this.$store.dispatch('getPrinter', this.id)
    },
    methods: {
        async getJobs(printer) {
            await this.$store.dispatch('getJobs', {
                page: 1,
                limit: 25,
                id: this.id
            })
        },
        async sync() {
            await this.$store.dispatch('syncPrinters', this.шв).then(() => {
                this.$store.commit('addNotification', createSuccessNotification(this.$t('Synced')))
            })
            this.printer = await this.$store.dispatch('getPrinter', this.id)
        }
    }
}
</script>

<template>
    <PageTemplate
        v-if="printer !== null"
        :title="$t('Printer {name}',{name:printer.Name})">
        <div class="printer mt-4">
            <div class="img pr-6">
                <img
                    src="../../../assets/printer-default.png"
                    class="printer-img">
                <div class="status">
                    <VChip
                        color="primary"
                        class="mt-3"
                        :text="status"
                        prepend-icon="mdi-printer" />
                </div>
            </div>
            <div class="info">
                <VTabs
                    v-model="tab"
                    show-arrows
                    center-active>
                    <VTab
                        value="info"
                        prepend-icon="mdi-printer">
                        {{ $t("Common information") }}
                    </VTab>
                    <VTab
                        value="cons"
                        prepend-icon="mdi-format-color-fill">
                        {{ $t("Consumables") }}
                    </VTab>
                    <VTab
                        value="jobs"
                        prepend-icon="mdi-tray-full"
                        @click="getJobs(printer)">
                        {{ $t("Print jobs") }}
                    </VTab>
                </VTabs>

                <div class="mt-4">
                    <VTabsWindow v-model="tab">
                        <VTabsWindowItem value="info">
                            <VSheet
                                color="blue-grey-lighten-5 pa-4"
                                rounded="sm">
                                <h4>{{ $t('Ports') }}</h4>
                                <div
                                    v-if="printer.Ports"
                                    class="ports">
                                    <div
                                        v-for="port in printer.Ports"
                                        :key="port.Id"
                                        class="port">
                                        <div class="name">
                                            {{ $t('Port name') }}: {{ port.Name }}
                                        </div>
                                        <div class="ip">
                                            {{ $t('Port IP') }}: {{ port.HostAddress }}
                                        </div>
                                        <div
                                            v-if="port.MacAddress"
                                            class="mac">
                                            {{ $t('Port MAC') }}: {{ port.MacAddress }}
                                        </div>
                                    </div>
                                </div>
                            </VSheet>
                            <VSheet
                                color="blue-grey-lighten-5 pa-4 mt-4"
                                rounded="sm">
                                <div
                                    v-if="printer.DriverName"
                                    class="driver">
                                    {{ $t('Driver') }}: {{ printer.DriverName }}
                                </div>
                                <div
                                    v-if="printer.Comment"
                                    class="comment">
                                    {{ $t('Comment') }}: {{ printer.Comment }}
                                </div>
                            </VSheet>
                            <VSheet
                                color="blue-grey-lighten-5 pa-4 mt-4"
                                rounded="sm">
                                <div
                                    v-if="printer.PrinterPaperNames"
                                    class="formats">
                                    <h4>
                                        {{ $t('Paper formats') }}
                                    </h4>
                                    <VChip
                                        v-for="f in printer.PrinterPaperNames"
                                        :key="f"
                                        :text="f"
                                        density="compact"
                                        class="mb-2 mr-2" />
                                </div>
                            </VSheet>

                            <VSheet
                                color="blue-grey-lighten-5 pa-4 mt-4"
                                rounded="sm">
                                <h4>{{ $t('SNMP information') }}</h4>
                                <div class="snmp-item">
                                    {{ $t('SnmpUptime') }}: {{ printer?.SnmpUptime }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpContact') }}: {{ printer?.SnmpContact }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpFQDN') }}: {{ printer.SnmpFQDN ?? printer.SnmpSystemName }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpLocation') }}: {{ printer?.SnmpLocation }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpSerialNumber') }}: {{
                                        printer?.SnmpSerialNumber
                                    }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpCountTotal') }}: {{ printer?.SnmpCountTotal }}
                                </div>
                                <div class="snmp-item">
                                    {{ $t('SnmpCountUptime') }}: {{ printer?.SnmpCountUptime }}
                                </div>
                                <div
                                    v-if="printer.OperatorMessage"
                                    class="snmp-item">
                                    {{ $t('OperatorMessage') }}:
                                    {{ printer?.OperatorMessage }}
                                </div>
                            </VSheet>
                        </VTabsWindowItem>

                        <VTabsWindowItem value="cons">
                            <div class="consumables">
                                <ConsumableItem
                                    v-for="cons in printer.Consumables"
                                    :key="cons.Id"
                                    class="w-md-33 w-lg-25 w-sm-100 pa-2"
                                    :consumable="cons" />
                            </div>
                        </VTabsWindowItem>
                        <VTabsWindowItem value="jobs">
                            <PrintJobsWrapper :printer-id="printer.Id" />
                        </VTabsWindowItem>
                    </VTabsWindow>
                </div>
            </div>
        </div>
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
.printer {
    display: flex;
    align-items: flex-start;
    flex-wrap: wrap;

    .img {
        width: 200px;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .printer-img {
        width: 200px;
        height: auto;
        display: block;
        opacity: 0.6
    }

    .info {

        width: calc(100% - 200px);

        .consumables {
            display: flex;
            align-items: flex-start;
            justify-content: flex-start;
            flex-wrap: wrap;
        }
    }
}


</style>