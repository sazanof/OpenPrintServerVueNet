<script>
export default {
    name: 'UserDropdown',
    computed: {
        user() {
            return this.$store.getters['getUser']
        },
        fullName() {
            return `${this.user.FirstName} ${this.user.LastName}`
        },
        initials() {
            return `${this.user.FirstName[0]}${this.user.LastName[0]}`
        }
    }
}
</script>

<template>
    <VMenu>
        <template #activator="{props}">
            <VBtn
                rounded="sircle"
                icon
                v-bind="props">
                <VAvatar
                    :text="initials"
                    color="primary" />
            </VBtn>
        </template>
        <VCard width="200">
            <VCardText class="pa-0">
                <div class="mx-auto text-center pa-4">
                    <VAvatar
                        :size="56"
                        color="primary"
                        class="mb-2">
                        <span class="text-h5">{{ initials }}</span>
                    </VAvatar>
                    <h3>{{ fullName }}</h3>
                    <p class="text-caption mt-1">
                        {{ user.Email }}
                    </p>
                </div>
                <VDivider />
                <div class="mt-1">
                    <VBtn
                        :rounded="0"
                        variant="text"
                        width="100%"
                        color="blue-grey-darken-3"
                        value="settings"
                        :to="{name:'settings'}"
                        prepend-icon="mdi-cog">
                        {{ $t('Settings') }}
                    </VBtn>
                    <VBtn
                        :rounded="0"
                        variant="text"
                        width="100%"
                        value="settings"
                        color="blue-grey-darken-3"
                        :to="{name:'settings'}"
                        prepend-icon="mdi-account-multiple">
                        {{ $t('Users') }}
                    </VBtn>
                </div>
            </VCardText>
        </VCard>
    </VMenu>
</template>

<style scoped lang="scss">

</style>