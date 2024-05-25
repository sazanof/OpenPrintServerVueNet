<template>
    <VApp
        v-if="visible"
        class="bg-blue-grey-lighten-5"
        full-height>
        <Installation v-if="installed !== null && !installed" />
        <Login v-else-if="installed && !authenticated" />
        <Page v-else-if="installed && authenticated" />
    </VApp>
</template>

<script>
import Installation from '@/components/chunks/Installation.vue'
import Login from '@/components/pages/Login.vue'
import Page from '@/components/pages/Page.vue'

export default {
    name: 'App',
    components: {
        Login,
        Installation,
        Page
    },
    data() {
        return {
            visible: false
        }
    },
    computed: {
        authenticated() {
            return this.$store.getters['getAuthenticated']
        },
        user() {
            return this.$store.getters['getUser']
        },
        installed() {
            return this.$store.getters['isInstalled']
        }
    },
    async created() {
        await this.$store.dispatch('checkAuth').finally(() => {
            this.visible = true
        })
    }
}
</script>

<style lang="scss" scoped>
header {
    line-height: 1.5;
}

.logo {
    display: block;
    margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
    header {
        display: flex;
        place-items: center;
        padding-right: calc(var(--section-gap) / 2);
    }

    .logo {
        margin: 0 2rem 0 0;
    }

    header .wrapper {
        display: flex;
        place-items: flex-start;
        flex-wrap: wrap;
    }
}
</style>
