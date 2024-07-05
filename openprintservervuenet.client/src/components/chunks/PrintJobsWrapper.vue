<script>
import PrintJobItem from '@/components/chunks/PrintJobItem.vue'
import Pagination from '@/components/chunks/Pagination.vue'

export default {
    name: 'PrintJobsWrapper',
    components: { Pagination, PrintJobItem },
    props: {
        printerId: {
            type: Number,
            default: null
        }
    },
    data() {
        return {
            page: 1,
            limit: 25
        }
    },
    computed: {
        jobs() {
            return this.$store.getters['getJobs']
        },
        filter() {
            return {
                id: this.printerId,
                page: this.page,
                limit: this.limit
            }
        }
    },
    async created() {
        await this.$store.dispatch('getJobs', this.filter)
    },
    methods: {
        getJobs(page) {
            this.page = page
            this.$store.dispatch('getJobs', this.filter)
        }
    }
}
</script>

<template>
    <div class="jobs">
        <VTable
            v-if="jobs"
            fixed-header
            density="compact">
            <thead>
                <tr>
                    <th />
                    <th class="text-left">
                        {{ $t('Printer') }}
                    </th>
                    <th class="text-left">
                        {{ $t('User') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Document') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Size total') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Size printed') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Pages total') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Pages printed') }}
                    </th>
                    <th class="text-left">
                        {{ $t('Submitted') }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <PrintJobItem
                    v-for="job in jobs.Results"
                    :key="job.Id"
                    :job="job" />
            </tbody>
        </VTable>
        <Pagination
            v-if="jobs.PageInfo"
            :pagination-results="jobs.PageInfo"
            @on-page-changed="getJobs" />
    </div>
</template>

<style scoped lang="scss">

</style>