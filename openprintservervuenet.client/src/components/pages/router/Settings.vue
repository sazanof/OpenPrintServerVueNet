<script>
import PageTemplate from '@/components/pages/PageTemplate.vue'
import { createSuccessNotification } from '@/js/helpers/notificationHelper.js'

export default {
    name: 'Settings',
    components: {
        PageTemplate
    },
    data() {
        return {
            loading: false,
            config: {
                'app.title': null,
                'app.api_url': null,
                'app.api_enabled': null
            }
        }
    },
    computed: {
        serverConfig() {
            return this.$store.getters['getConfig']
        }
    },
    async created() {
        await this.$store.dispatch('getConfig')
        this.merge()
    },
    methods: {
        merge() {
            Object.keys(this.config).map((key, index) => {
                const setting = this.serverConfig.find(c => c.Key === key)
                if (setting) {
                    this.config[key] = setting.Value
                }
            })
        },
        validateUrl(url) {
            const expr = '(https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,})'
            const re = new RegExp(expr)
            return re.test(url)
        },
        async saveConfig() {
            this.loading = true
            const data = Object.keys(this.config).map((key, index) => {
                return {
                    Key: key,
                    Value: this.config[key] === false ? 'false' : this.config[key]
                }
            })
            await this.$store.dispatch('saveConfig', data).then(() => {
                this.$store.commit('addNotification', createSuccessNotification(this.$t('Saved')))
            })
            this.loading = false
        }
    }
}
</script>

<template>
    <PageTemplate>
        <VSheet
            width="400"
            class="mx-auto">
            <div class="text-h5 mb-4 text-center">
                {{ $t('Settings') }}
            </div>
            <VTextField
                v-model="config['app.title']"
                class="mb-4"
                :label="$t('Application name')" />

            <VTextField
                v-model="config['app.api_url']"
                class="mb-4"
                :label="$t('API url')" />

            <VCheckbox
                v-model="config['app.api_enabled']"
                :disabled="!validateUrl(config['app.api_url'])"
                :label="$t('API enabled')"
                value="true" />

            <VBtn
                :disabled="loading"
                :loading="loading"
                class="w-100"
                prepend-icon="mdi-content-save"
                :text="$t('Save')"
                @click="saveConfig" />
        </VSheet>
    </PageTemplate>
</template>

<style scoped lang="scss">

</style>