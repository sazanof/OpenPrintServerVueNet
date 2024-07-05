<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import Modal from '@/components/chunks/Modal.vue'

export default {
    name: 'Users',
    components: {
        Modal,
        PageTemplate
    },
    data() {
        return {
            currentUser: null,
            users: null,
            data: {
                firstname: null,
                lastname: null,
                username: null,
                email: null,
                password: null
            }
        }
    },
    computed: {
        disabled() {
            return false
        },
        loading() {
            return this.$store.getters['isLoading']
        }
    },
    created() {
        this.getUsers()
    },
    methods: {
        openModal() {
            this.currentUser = null
            this.$refs.usersModal.show()
        },
        async addUser() {
            await this.$store.dispatch('addUser', this.data)
            this.$refs.usersModal.hide()
            await this.getUsers()
        },
        async getUsers() {
            this.users = await this.$store.dispatch('getUsers')
        }
    }
}
</script>

<template>
    <PageTemplate :title="$t('Users')">
        <template #actions>
            <VBtn
                prepend-icon="mdi-plus"
                :text="$t('Add')"
                @click="openModal" />
        </template>
        <Modal
            ref="usersModal"
            :title="currentUser !== null ? $t('Edit user'):$t('Add user')">
            <VTextField
                v-model="data.firstname"
                class="mb-4"
                :label="$t('Firstname')" />
            <VTextField
                v-model="data.lastname"
                class="mb-4"
                :label="$t('Lastname')" />
            <VTextField
                v-model="data.username"
                class="mb-4"
                :label="$t('Username')" />
            <VTextField
                v-model="data.email"
                class="mb-4"
                type="email"
                :label="$t('Email')" />
            <VTextField
                v-model="data.password"
                class="mb-4"
                type="password"
                :label="$t('Password')" />
            <VBtn
                class="w-100"
                :disabled="disabled"
                :loading="loading"
                prepend-icon="mdi-send"
                :text="$t('Add')"
                @click="addUser" />
        </Modal>
        <pre>{{ users }}</pre>
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>