import Dashboard from '@/components/pages/router/Dashboard.vue'
import Printers from '@/components/pages/router/Printers.vue'
import PrintJobs from '@/components/pages/router/PrintJobs.vue'

import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    { path: '/', component: Dashboard, name: 'dashboard' },
    { path: '/printers', component: Printers, name: 'printers' },
    { path: '/jobs', component: PrintJobs, name: 'jobs' }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes // short for `routes: routes`
})

export default router