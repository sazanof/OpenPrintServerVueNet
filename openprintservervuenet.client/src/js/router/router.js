import {createRouter, createWebHashHistory} from 'vue-router'

const routes = [];

const router = createRouter({
    history: createWebHashHistory(),
    routes // short for `routes: routes`
})

export default router