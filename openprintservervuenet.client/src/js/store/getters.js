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
    }
}