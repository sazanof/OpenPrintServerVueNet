<script>
export default  {
  name: 'SyncStatus',
  data(){
    return {
      open: true
    }
  },
  computed: {
    syncStatus() {
      return this.$store.getters['getSyncStatus']
    }
  },
  watch: {
    syncStatus(){
      this.open = this.syncStatus === 1
    }
  }
}
</script>

<template>
    <VSnackbar
        v-if="syncStatus === 1"
        v-model="open"
        :timeout="-1">
        {{ $t('Printers sync job in process') }}
    </VSnackbar>
</template>

<style scoped lang="scss">

</style>