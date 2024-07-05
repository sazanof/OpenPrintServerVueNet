import { createRouter, createWebHashHistory } from 'vue-router'

import Dashboard from '@/components/pages/router/Dashboard.vue'
import Printers from '@/components/pages/router/Printers.vue'
import PrintJobs from '@/components/pages/router/PrintJobs.vue'
import Settings from '@/components/pages/router/Settings.vue'
import Users from '@/components/pages/router/Users.vue'
import Printer from '@/components/pages/router/Printer.vue'

const routes = [
    { path: '/', component: Dashboard, name: 'dashboard' },
    {
        path: '/printers',
        children: [
            {
                path: '',
                component: Printers,
                name: 'printers'
            },

            {
                path: ':id(\\d+)',
                component: Printer,
                name: 'printer'
            }
        ]
    },
    { path: '/jobs', component: PrintJobs, name: 'jobs' },
    { path: '/settings', component: Settings, name: 'settings' },
    { path: '/users', component: Users, name: 'users' }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes // short for `routes: routes`
})

export default router