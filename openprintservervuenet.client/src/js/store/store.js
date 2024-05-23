import { createStore } from 'vuex'
import actions from './actions.js'
import mutations from './mutations.js'
import getters from './getters.js'
import state from './state.js'

export default createStore(
    {
        actions,
        mutations,
        getters,
        state
    }
)