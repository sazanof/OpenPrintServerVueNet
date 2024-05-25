export default {
    setAuthenticated(state, a) {
        state.authenticated = a
    },
    setUser(state, user) {
        state.user = user
    },
    setInstalled(state, inst) {
        state.installed = inst
    },
    setLoading(state, load) {
        state.loading = load
    },
    setPrinters(state, printers) {
        state.printers = printers
    },
    setSignalR(state, signal) {
        state.signal = signal
    },
    setConnected(state, connected) {
        state.connected = connected
    }
}