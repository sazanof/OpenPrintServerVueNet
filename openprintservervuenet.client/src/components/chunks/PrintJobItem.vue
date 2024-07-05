<script>
import formatBytes from '@/js/helpers/formatBytes.js'
import PaletteIcon from '@/components/chunks/PaletteIcon.vue'
import moment from 'moment'
import { printJobStatus } from '@/js/helpers/printJobStatus.js'

export default {
    name: 'PrintJobItem',
    components: { PaletteIcon },
    props: {
        job: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            more: false
        }
    },
    computed: {
        submitted() {
            return moment(this.job.Submitted).format('DD.MM.YYYY H:mm:ss')
        },
        bytesTotal() {
            return formatBytes(this.job.BytesTotal)
        },
        bytesPrinted() {
            return formatBytes(this.job.BytesPrinted)
        }
    },
    methods: { printJobStatus }
}
</script>

<template>
    <tr>
        <td>
            <VIcon
                :icon="more?'mdi-chevron-up':'mdi-chevron-down'"
                @click="more = !more" />
        </td>
        <td class="name">
            <VIcon icon="mdi-printer" />
            <span class="ml-2">{{ job.PrinterName }}</span>
        </td>
        <td>
            <VIcon icon="mdi-account" />
            <span class="ml-2">{{ job.Username }}</span>
        </td>
        <td>
            <div class="document">
                <PaletteIcon
                    :pallete="parseInt(job.Printer_Palette)"
                    :zoom="0.5"
                    class="d-inline-block" />
                <VTooltip :text="job.Document">
                    <template #activator="{ props }">
                        <span v-bind="props">{{ job.Document }}</span>
                    </template>
                </VTooltip>
            </div>
        </td>
        <td>
            {{ bytesTotal.num }} {{ bytesTotal.size }}
        </td>
        <td>
            {{ bytesPrinted.num }} {{ bytesPrinted.size }}
        </td>
        <td>
            {{ job.PagesTotal }}
        </td>
        <td>
            {{ job.PagesPrinted }}
        </td>
        <td>
            {{ submitted }}
        </td>
    </tr>
    <tr v-if="more">
        <td colspan="9">
            <div class="job-info pa-4">
                <div class="job-info-item">
                    <strong>Id</strong>: <span>{{ job.Id }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Job ID') }}</strong>: <span>{{ job.JobId }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Printer name') }}</strong>: <span>{{ job.PrinterName }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Machine name') }}</strong>: <span>{{ job.MachineName }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Username') }}</strong>: <span>{{ job.Username }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Driver') }}</strong>: <span>{{ job.DriverName }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Orientation') }}</strong>: <span>{{
                        job.Printer_Orientation
                    }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Paper size') }}</strong>: <span>{{ job.Printer_Paper_Size }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Copies') }}</strong>: <span>{{ job.Printer_Copies }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Color') }}</strong>: <span>{{ job.Printer_Palette }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Status') }}</strong>: <span>{{ printJobStatus(job.Status) }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Document') }}</strong>: <span>{{ job.Document }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Priority') }}</strong>: <span>{{ job.Priority }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Position') }}</strong>: <span>{{ job.Position }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Submitted') }}</strong>: <span>{{ submitted }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Time') }}</strong>: <span>{{ job.Time }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Pages total') }}</strong>: <span>{{ job.PagesTotal }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Pages printed') }}</strong>: <span>{{ job.PagesPrinted }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Size total') }}</strong>:
                    <span>{{ bytesTotal.num }} {{ bytesPrinted.size }}</span>
                </div>
                <div class="job-info-item">
                    <strong>{{ $t('Size printed') }}</strong>:
                    <span>{{ bytesPrinted.num }} {{ bytesPrinted.size }}</span>
                </div>

                <div class="job-info-item">
                    <strong>{{ $t('Synced') }}</strong>: <span>
                        <VIcon
                            v-if="job.Synced"
                            icon="mdi-check"
                            color="success" />
                        <VIcon
                            v-else
                            icon="mdi-close"
                            color="error" />
                    </span>
                </div>
            </div>
        </td>
    </tr>
</template>
<style scoped lang="scss">
.document {
    width: 250px;
    display: flex;
    align-items: center;
    justify-content: flex-start;

    span {
        width: 200px;
        overflow: hidden;
        text-overflow: ellipsis;
    }

}

.name {
    width: 200px;
}
</style>