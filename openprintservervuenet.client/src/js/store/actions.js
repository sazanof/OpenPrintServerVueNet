import axios from 'axios'
import * as signalR from '@microsoft/signalr'
import { hu } from 'vuetify/locale'

export default {
    /**
     * Install APP
     * @param commit
     * @param data
     * @returns {Promise<void>}
     */
    async install({ commit }, data) {
        return await axios.post('/api/install', data).then(res => {
            if (res.data.hasOwnProperty('IsInstalled')) {
                commit('setInstalled', res.data.IsInstalled)
            }
            if (res.hasOwnProperty('User')) {
                commit('setUser', res.data.User)
            }
            if (res.hasOwnProperty('Authenticated')) {
                commit('setAuthenticated', res.data.Authenticated)
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
            commit('setAuthenticated', res.data.Authenticated)
            commit('setUser', res.data.User)
            return res.data
        }).catch(e => {
            if (e.response.status === 403 && e.response.data.hasOwnProperty('isInstalled')) {
                commit('setInstalled', e.response.data.isInstalled)
            } else {
                commit('setInstalled', null) //TODO throw error if 500 status
                commit('setLastError', e.response)
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
     * @returns {Promise<void>}
     */
    async logIn({ commit }, data) {
        commit('setLoading', true)
        return await axios.post('/auth/login', data).then(res => {
            commit('setAuthenticated', res.data.Authenticated)
            commit('setUser', res.data.User)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    /**
     * Log out user
     * @param commit
     * @returns {Promise<void>}
     */
    async logOut({ commit }) {
        commit('setLoading', true)
        return await axios.post('/auth/logout').then(res => {
            commit('setAuthenticated', false)
            commit('setUser', null)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async getPrinters({ commit }) {
        commit('setLoading', true)

        await axios.get('/api/printers').then(res => {
            commit('setPrinters', res.data)
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async getPrinter({ commit }, id) {
        commit('setLoading', true)
        return await axios.get(`/api/printers/${id}`).then(res => {
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async deletePrinter({ commit }, id) {
        commit('setLoading', true)
        return await axios.delete(`/api/printers/${id}`).then(res => {
            commit('deletePrinter', id)
            commit('removePrinterJobs', id)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async syncPrinters({ commit }, id = null) {
        commit('setLoading', true)
        return await axios.get(id === null ? '/api/printers/sync' : `/api/printers/sync/${id}`).then(res => {
            commit('setPrinters', res.data)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    createSignalR({ commit }) {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('/notifications')
            .build()
        hubConnection.on('on.job', function (job) {
            const _job = JSON.parse(job)
            commit('addJob', _job)
        })
        hubConnection.on('on.printer.changed', function (printer) {
            const _printer = JSON.parse(printer)
            commit('updatePrinter', _printer)
            console.log('Update printer', _printer)
        })
        hubConnection.on('on.printer.add', function (printer) {
            const _printer = JSON.parse(printer)
            commit('addPrinter', _printer)
            console.log('on.printer.add', _printer)
        })
        hubConnection.on('on.printer.delete', function (printer) {
            const _printer = JSON.parse(printer)
            commit('deletePrinter', _printer.Id)
            console.log('on.printer.delete', _printer)
        })
        hubConnection.start()
            .then(() => {
                commit('setConnected', true)
            })
            .catch(() => {
                commit('setConnected', false)
            })
        hubConnection.onclose(function () {
            commit('setConnected', false)
        })
        commit('setSignalR', hubConnection)
    },
    async getJobs({ commit }, filter = 1) {
        commit('setLoading', true)
        await axios.post('/api/jobs', filter).then(res => {
            commit('setJobs', res.data)
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async getConfig({ commit }) {
        commit('setLoading', true)
        await axios.get('/api/config').then(res => {
            commit('setConfig', res.data)
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async saveConfig({ commit }, data) {
        commit('setLoading', true)
        await axios.post('/api/config', data).then(res => {
            commit('setConfig', res.data)
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async getUsers({ commit }) {
        commit('setLoading', true)
        return await axios.get('/api/users').then(res => {
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    },
    async addUser({ commit }, data) {
        commit('setLoading', true)
        return await axios.post('/api/users', data).then(res => {
            return res.data
        }).finally(() => {
            commit('setLoading', false)
        })
    }
}