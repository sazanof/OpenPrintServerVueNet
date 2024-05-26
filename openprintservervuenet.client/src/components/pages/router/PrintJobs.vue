<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import Pagination from '@/components/chunks/Pagination.vue'
import PrintJobItem from '@/components/chunks/PrintJobItem.vue'

export default {
    name: 'PrintJobs',
    components: {
        PrintJobItem,
        Pagination,
        PageTemplate
    },
    data() {
        return {
            page: 1
        }
    },
    computed: {
        jobs() {
            return this.$store.getters['getJobs']
        }
    },
    async created() {
        await this.$store.dispatch('getJobs', this.page)
    },
    methods: {
        getJobs(page) {
            this.page = page
            this.$store.dispatch('getJobs', this.page)
        }
    }
}
</script>

<template>
    <PageTemplate :title="$t('Print jobs')">
        <VTable fixed-header>
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
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>