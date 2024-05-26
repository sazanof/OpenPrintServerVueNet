<script>
export default {
    name: 'Pagination',
    props: {
        paginationResults: {
            type: Object,
            required: true
        },
        pagesVisible: {
            type: Number,
            default: 15
        }
    },
    emits: [ 'on-page-changed' ],
    data() {
        return {
            pageNumber: 0,
            pageSize: 0,
            totalItems: 0,
            totalPages: 0
        }
    },
    computed: {
        total() {
            return this.paginationResults.totalPages
        }
    },
    created() {
        this.pageNumber = this.paginationResults?.pageNumber
        this.pageSize = this.paginationResults?.pageSize
        this.totalItems = this.paginationResults?.totalItems
    },
    methods: {
        switchPage(page) {
            this.pageNumber = page
            this.$emit('on-page-changed', page)
        }
    }
}
</script>

<template>
    <div
        v-if="paginationResults && total > 0"
        class="text-center">
        <VPagination
            v-model="pageNumber"
            :length="total"
            :total-visible="pagesVisible"
            @update:model-value="switchPage" />
    </div>
</template>

<style scoped lang="scss">

</style>