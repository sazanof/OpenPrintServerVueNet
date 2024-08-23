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
    addPrinter(state, printer) {
        const found = state.printers.findIndex(_printer => _printer.Id === printer.Id)
        if (found === -1) {
            state.printers.push(printer)
        }
    },
    updatePrinter(state, printer) {
        const found = state.printers.findIndex(_printer => _printer.Id === printer.Id)
        if (found >= 0) {
            state.printers[found] = printer
        }
    },
    deletePrinter(state, id) {
        state.printers = state.printers.filter(p => p.Id !== id)
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
    removePrinterJobs(state, id) {
        state.jobs = state.jobs.filter(job => job.PrinterId !== id)
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
    setSync(state, sync){
        state.syncRunning = sync
    },
    setSyncStatus(state, status){
        state.syncStatus = status
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