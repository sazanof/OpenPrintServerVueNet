<script>
import Logo from '@/components/chunks/Logo.vue'

export default {
    name: 'Login',
    components: {
        Logo
    },
    data() {
        return {
            disabled: false,
            username: null,
            password: null
        }
    },
    computed: {
        loading() {
            return this.$store.getters['isLoading']
        }
    },
    methods: {
        async logIn() {
            await this.$store.dispatch('logIn', {
                username: this.username,
                password: this.password
            })
        }
    }
}
</script>

<template>
    <VSheet
        color="blue-grey-lighten-5"
        class="login">
        <VCard
            :loading="loading"
            width="360"
            align="center">
            <template #title>
                <Logo />
            </template>
            <template #text>
                <VTextField
                    v-model="username"
                    class="mb-4"
                    :label="$t('Username')" />
                <VTextField
                    v-model="password"
                    type="password"
                    class="mb-4"
                    :label="$t('Password')" />
                <VBtn
                    class="w-100"
                    :disabled="disabled"
                    :loading="loading"
                    prepend-icon="mdi-send"
                    :text="$t('Log in')"
                    @click="logIn" />
            </template>
        </VCard>
    </VSheet>
</template>

<style scoped lang="scss">
.login {
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