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
    }
}