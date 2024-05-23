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
    }
}