export default function formatBytes(bytes, decimals = 2) {
    if (!+bytes) return '0 Bytes'
    const k = 1024
    const dm = decimals < 0 ? 0 : decimals
    const sizes = [ 'Bytes', 'KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB' ]
    const i = Math.floor(Math.log(bytes) / Math.log(k))
    return {
        num: parseFloat((bytes / Math.pow(k, i)).toFixed(dm)),
        size: sizes[i]
    }
}