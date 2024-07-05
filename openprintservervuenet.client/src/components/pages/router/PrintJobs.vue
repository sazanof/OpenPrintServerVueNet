<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import PrintJobsWrapper from '@/components/chunks/PrintJobsWrapper.vue'

export default {
    name: 'PrintJobs',
    components: {
        PageTemplate,
        PrintJobsWrapper
    }

}
</script>

<template>
    <PageTemplate :title="$t('Print jobs')">
        <PrintJobsWrapper />
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>