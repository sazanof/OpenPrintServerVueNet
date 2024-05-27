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
    },
    setJobs(state, jobs) {
        state.jobs = jobs
    },
    addJob(state, job) {
        let existingIndex = state.jobs.Results.findIndex(j => j.Id === job.Id)
        if (existingIndex === -1) {
            state.jobs.Results.unshift(job)
        } else {
            state.jobs.Results[existingIndex] = job
            console.log('update job', job, existingIndex)
        }
    },
    deleteJob(state, job) {
        state.jobs.Results.splice(state.jobs.Results.indexOf(job), 1)
    },
    setLastError(state, lastError) {
        state.lastError = lastError
    },
    setConfig(state, config) {
        state.config = config
    },
    addNotification(state, notification) {
        state.notifications.push(notification)
    },
    removeNotification(state, notification) {
        state.notifications = state.notifications.filter(n => n.active === true)
    }
}