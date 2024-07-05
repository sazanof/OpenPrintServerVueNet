<template>
    <v-dialog
        v-model="isVisible"
        width="400">
        <template #default="{ isActive }">
            <v-card
                :color="color"
                :title="title">
                <v-card-text>
                    {{ message }}
                    <slot />
                </v-card-text>

                <v-card-actions>
                    <v-spacer />

                    <v-btn
                        :disabled="disabled"
                        :prepend-icon="okIcon"
                        :color="okColor"
                        :text="okButton"
                        @click="_confirm(isActive)" />
                    <v-btn
                        :color="cancelColor"
                        :prepend-icon="cancelIcon"
                        :text="cancelButton"
                        @click="_cancel(isActive)" />
                </v-card-actions>
            </v-card>
        </template>
    </v-dialog>
</template>
<script>
export default {
    name: 'ConfirmationDialog',
    props: {
        color: {
            type: String,
            default: 'white'
        },
        disabled: {
            type: Boolean,
            default: false
        }
    },
    emits: [ 'on-dialog-confirm' ],

    data: () => ({
        // Parameters that change depending on the type of dialogue
        isVisible: false,
        title: undefined,
        message: undefined, // Main text content
        okButton: undefined, // Text for confirm button; leave it empty because we don't know what we're using it for
        cancelButton: undefined, // Text for confirm button; leave it empty because we don't know what we're using it for
        okIcon: 'mdi-check',
        okColor: 'error',
        cancelColor: 'primary',
        cancelIcon: 'mdi-close',
        // Private variables
        resolvePromise: undefined,
        rejectPromise: undefined
    }),

    methods: {
        open() {
            this.isVisible = true
        },

        close() {
            this.isVisible = false
        },
        show(opts = {}) {

            this.title = opts.title
            this.message = opts.message
            this.okButton = opts.okButton
            if (opts.okColor) {
                this.okColor = opts.okColor
            }
            if (opts.cancelColor) {
                this.cancelColor = opts.cancelColor
            }
            if (opts.okIcon) {
                this.okIcon = opts.okIcon
            }
            if (opts.cancelIcon) {
                this.cancelIcon = opts.cancelIcon
            }

            if (opts.cancelButton) {
                this.cancelButton = opts.cancelButton
            } else {
                this.cancelButton = this.$i18n.t('Cancel')
            }
            // Once we set our config, we tell the popup modal to open
            this.open()
            // Return promise so the caller can get results
            return new Promise((resolve, reject) => {
                this.resolvePromise = resolve
                this.rejectPromise = reject
            })
        },

        _confirm(isActive) {
            this.$emit('on-dialog-confirm')
            isActive = false
            this.close()
            this.resolvePromise(true)
        },

        _cancel(isActive) {
            isActive.value = false
            this.close()
            //this.resolvePromise(false)
            // Or you can throw an error
            this.rejectPromise(new Error('User cancelled the dialogue'))
        }
    }
}
</script>
<style lang="scss" scoped>
</style>