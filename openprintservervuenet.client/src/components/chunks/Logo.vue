<script>
export default {
    name: 'Logo',
    props: {
        width: {
            type: Number,
            default: 64
        }
    }
}
</script>

<template>
    <div class="logo">
        <img
            src="../../assets/printer.png"
            :width="width">
    </div>
</template>

<style lang="scss" scoped>
.logo {
    display: inline-block;
    text-align: center;

    img {
        max-width: 200px;
        height: auto;
    }
}
</style>