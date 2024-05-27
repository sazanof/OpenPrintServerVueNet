import Dashboard from '@/components/pages/router/Dashboard.vue'
import Printers from '@/components/pages/router/Printers.vue'
import PrintJobs from '@/components/pages/router/PrintJobs.vue'
import Settings from '@/components/pages/router/Settings.vue'
import Users from '@/components/pages/router/Users.vue'

import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    { path: '/', component: Dashboard, name: 'dashboard' },
    { path: '/printers', component: Printers, name: 'printers' },
    { path: '/jobs', component: PrintJobs, name: 'jobs' },
    { path: '/settings', component: Settings, name: 'settings' },
    { path: '/users', component: Users, name: 'users' }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes // short for `routes: routes`
})

export default router