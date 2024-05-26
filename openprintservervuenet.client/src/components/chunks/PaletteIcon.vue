<script>
export default {
    name: 'PaletteIcon',
    props: {
        pallete: {
            type: Number,
            default: 0
        },
        zoom: {
            type: Number,
            default: 1
        }
    },
    computed: {
        cyanColor() {
            switch (this.pallete) {
                case 1:
                    return 'grey-darken-2'
                case 2:
                    return 'cyan'
                default:
                    return 'grey-lighten-3'
            }
        },
        pinkColor() {
            switch (this.pallete) {
                case 1:
                    return 'grey-lighten-2'
                case 2:
                    return 'pink'
                default:
                    return 'grey-lighten-3'
            }
        },
        yellowColor() {
            switch (this.pallete) {
                case 1:
                    return 'grey-lighten-3'
                case 2:
                    return 'yellow'
                default:
                    return 'grey-lighten-3'
            }
        }
    }
}
</script>

<template>
    <div
        class="squares"
        :style="`transform: scale(${zoom})`">
        <div class="square-line-one">
            <VIcon
                class="square-cyan"
                icon="mdi-square"
                :color="cyanColor" />
            <VIcon
                class="square-pink"
                icon="mdi-square"
                :color="pinkColor" />
        </div>
        <div class="square-line-two">
            <VIcon
                class="square-yellow"
                icon="mdi-square"
                :color="yellowColor" />
            <VIcon
                class="square-black"
                icon="mdi-square"
                color="black" />
        </div>
    </div>
</template>

<style scoped lang="scss">
.squares {
    width: 40px;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    position: relative;

    .square-cyan, .square-pink, .square-yellow, .square-black {
        position: absolute;
        top: 0;
        left: 0;
    }

    .square-pink {
        left: 20px
    }

    .square-yellow {
        top: 20px
    }

    .square-black {
        top: 20px;
        left: 20px
    }

}
</style>