<script>
import formatBytes from '@/js/helpers/formatBytes.js'
import PaletteIcon from '@/components/chunks/PaletteIcon.vue'
import { useDate } from 'vuetify'

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
            const date = useDate()
            return date.format(this.job.Submitted, 'fullDateTime24h')
        },
        bytesTotal() {
            return formatBytes(this.job.BytesTotal)
        },
        bytesPrinted() {
            return formatBytes(this.job.BytesPrinted)
        }
    }
}
</script>

<template>
    <tr>
        <td>
            <VIcon
                :icon="more?'mdi-chevron-up':'mdi-chevron-down'"
                @click="more = !more" />
        </td>
        <td>
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
            <code>
                <pre>{{ job }}</pre>
            </code>
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
</style>