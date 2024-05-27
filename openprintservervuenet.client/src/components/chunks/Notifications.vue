<template>
    <VSnackbar
        v-for="(notification, index) in activeNotifications"
        :key="notification"
        v-model="notification.active"
        location="bottom right"
        :style="`bottom:${(60 * index)}px`"
        :color="notification.style"
        :timeout="notification.timeout"
        @update:model-value="onClose($event,notification)">
        <template #actions>
            <VBtn
                icon="mdi-close"
                density="compact"
                variant="text"
                color="white"
                @click="onClose(false, notification)" />
        </template>
        {{ notification.text }}
    </VSnackbar>
</template>

<script>
export default {
    name: 'Notifications',
    computed: {
        notifications() {
            return this.$store.getters['getNotifications']
        },

        activeNotifications() {
            return this.notifications.filter(n => n.active === true)
        }
    },
    methods: {
        onClose(state, notification) {
            notification.active = false
            if (!state) {
                this.$store.commit('removeNotification', notification)
            }
        }
    }
}
</script>

<style scoped>

</style>