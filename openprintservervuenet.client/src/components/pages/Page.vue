<script>
import Notifications from '@/components/chunks/Notifications.vue'
import AppHeader from '@/components/chunks/AppHeader.vue'
import AppNavigationDrawer from '@/components/chunks/AppNavigationDrawer.vue'
import SyncStatus from '@/components/chunks/SyncStatus.vue'

export default {
  name: 'Page',
  components: {
    SyncStatus,
    AppNavigationDrawer,
    AppHeader,
    Notifications
  },
  computed: {
    loading() {
      return this.$store.getters['isLoading']
    }
  },
  async created() {
    this.$store.dispatch('createSignalR')
    await this.$store.dispatch('getConfig')

    setInterval(()=>{
      this.$store.dispatch('getPrinterSyncStatus')
    }, 10000)
  } 

}
</script>

<template>
    <VMain>
        <AppNavigationDrawer />
        <AppHeader />
        <VLayout
            class="pa-4 page-layout"
            full-height>
            <VSheet
                rounded
                color="bg-blue-grey-lighten-5"
                class="pa-6 layout-sheet"
                height="100%"
                width="100%">
                <VProgressLinear 
                    :active="loading"
                    indeterminate
                    height="4"
                    color="blue-grey-darken-3"
                    class="progress-bar" />
                <router-view />
            </VSheet>
        </VLayout>
        <Notifications />
        <SyncStatus />
    </VMain>
</template>

<style scoped lang="scss">

.layout-sheet {
  position: relative;

  .progress-bar {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    z-index: 11;
  }
}

</style>