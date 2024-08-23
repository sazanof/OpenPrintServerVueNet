import state from '@/js/store/state.js'

export default {
    getAuthenticated(state) {
        return state.authenticated
    },
    getUser(state) {
        return state.user
    },
    isInstalled(state) {
        return state.installed
    },
    isLoading(state) {
        return state.loading
    },
    isSyncRunning(state){
      return state.syncRunning
    },
    getSyncStatus(state){
      return state.syncStatus
    },
    getPrinters(state) {
        return state.printers
    },
    getSignalR(state) {
        return state.signal
    },
    isConnected(state) {
        return state.connected
    },
    getJobs(state) {
        return state.jobs
    },
    getLastError(state) {
        return state.lastError
    },
    getConfig(state) {
        return state.config
    },
    getNotifications(state) {
        return state.notifications
    }
}