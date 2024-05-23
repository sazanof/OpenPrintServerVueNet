import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

import store from '@/js/store/store.js'
import router from '@/js/router/router.js'

import { aliases, mdi } from 'vuetify/iconsets/mdi'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { createVuetify } from 'vuetify'

const vuetify = createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi
        }
    },
    defaults: {
        global: {},
        VTextField: {
            density: 'compact',
            variant: 'outlined',
            hideDetails: true
        },
        VProgressLinear: {
            rounded: true,
            height: 10,
            color: 'primary'
        },
        VTextarea: {
            density: 'compact',
            variant: 'outlined',
            hideDetails: true
        },
        VFileInput: {
            density: 'compact',
            variant: 'outlined',
            hideDetails: true
        },
        VDialog: {
            VCard: {
                rounded: 'lg'
            }
        },
        VSelect: {
            density: 'compact',
            variant: 'outlined',
            hideDetails: true
        },
        VBtn: {
            variant: 'flat',
            color: 'blue-grey-darken-1'
        },
        VTable: {
            hover: true,
            density: 'compact'
        },
        VText: {
            density: 'compact',
            variant: 'outlined',
            hideDetails: true
        }
    }
})

import { setupI18n, loadLocaleMessages, plural } from './js/i18n/i18n.js'
import { createApp } from 'vue'
import App from './App.vue'

const i18n = setupI18n({
    locale: 'ru',
    pluralizationRules: {
        ru: plural
    }
})
loadLocaleMessages(i18n, i18n.global.locale).then(() => {
    const app = createApp(App)
    app.use(store)
    app.use(router)
    app.use(i18n)
    app.use(vuetify)
    app.mount('#app')
})


