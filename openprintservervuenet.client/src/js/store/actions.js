import axios from 'axios'

export default {
    /**
     * Install APP
     * @param commit
     * @param data
     * @returns {Promise<void>}
     */
    async install({ commit }, data) {
        return await axios.post('/api/install', data).then(res => {
            if (res.data.hasOwnProperty('isInstalled')) {
                commit('setInstalled', res.data.isInstalled)
            }
            if (res.hasOwnProperty('user')) {
                commit('setUser', res.data.user)
            }
            if (res.hasOwnProperty('authenticated')) {
                commit('setAuthenticated', res.data.authenticated)
            }
        })
    },
    /**
     * Check user is authenticated
     * @param commit
     * @returns {Promise<void>}
     */
    async checkAuth({ commit }) {
        commit('setLoading', true)
        const res = await axios.get('/auth/check').then(res => {
            commit('setAuthenticated', res.data.authenticated)
            commit('setUser', res.data.user)
            return res.data
        }).catch(e => {
            if (e.response.status === 403 && e.response.data.hasOwnProperty('isInstalled')) {
                commit('setInstalled', e.response.data.isInstalled)
            } else {
                commit('setInstalled', null) //TODO throw error if 500 status
            }
        }).finally(() => {
            commit('setLoading', false)
        })
        if (res) {
            commit('setInstalled', true)
        }
        return res
    },
    /**
     * Log in user
     * @param commit
     * @param data
     * @returns {Promise<axios.AxiosResponse<any>>}
     */
    async logIn({ commit }, data) {
        commit('setLoading', true)
        return await axios.post('/auth/login', data).then(res => {
            commit('setAuthenticated', res.data.authenticated)
            commit('setUser', res.data.user)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    }
}