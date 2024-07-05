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
            src="../../assets/logo-mini.svg"
            :width="width">
    </div>
</template>

<style lang="scss" scoped>
.logo {
    display: inline-flex;
    align-items: center;
    justify-content: center;

    img {
        max-width: 100%;
        height: auto;
        display: block;
        margin: 0 auto;
    }
}
</style>