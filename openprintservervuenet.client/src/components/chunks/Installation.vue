<script>
import Logo from '@/components/chunks/Logo.vue'

export default {
    name: 'Installation',
    components: { Logo },
    data() {
        return {
            loading: false,
            passwordConfirm: null,
            install: {
                firstname: null,
                lastname: null,
                email: null,
                username: null,
                password: null
            }
        }
    },
    computed: {
        passwordsEquals() {
            return this.install.password?.length >= 6 && (this.install.password === this.passwordConfirm)
        },
        disabled() {
            return Object.values(this.install).indexOf(null) !== -1 || !this.passwordsEquals
        }
    },
    methods: {
        async installApp() {
            this.loading = true
            const res = await this.$store.dispatch('install', this.install)
                .catch(e => {
                })
                .finally(() => {
                    this.loading = false
                })
            this.loading = false
        }
    }
}
</script>

<template>
    <VSheet
        color="blue-grey-lighten-5"
        class="install">
        <VCard
            width="360"
            align="center">
            <template #title>
                <Logo />
            </template>
            <template #text>
                <VTextField
                    v-model="install.firstname"
                    class="mb-4"
                    :label="$t('Firstname')" />
                <VTextField
                    v-model="install.lastname"
                    class="mb-4"
                    :label="$t('Lastname')" />
                <VTextField
                    v-model="install.username"
                    class="mb-4"
                    :label="$t('Username')" />
                <VTextField
                    v-model="install.email"
                    class="mb-4"
                    type="email"
                    :label="$t('Email')" />
                <VTextField
                    v-model="install.password"
                    class="mb-4"
                    type="password"
                    :label="$t('Password')" />
                <VTextField
                    v-model="passwordConfirm"
                    type="password"
                    class="mb-4"
                    :label="$t('Password confirmation')" />
                <VBtn
                    class="w-100"
                    :disabled="disabled"
                    :loading="loading"
                    prepend-icon="mdi-send"
                    :text="$t('Create account')"
                    @click="installApp" />
            </template>
        </VCard>
    </VSheet>
</template>

<style scoped lang="scss">
.install {
    position: absolute;
    top: 0;
    right: 0;
    left: 0;
    bottom: 0;
    z-index: 2;
    display: flex;
    align-items: center;
    justify-content: center;
}
</style>