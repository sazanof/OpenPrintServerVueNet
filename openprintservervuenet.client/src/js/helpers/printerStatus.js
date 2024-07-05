/**
 * https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-printer
 * Extended Printer Status
 * 1 (0x1): Other
 * 2 (0x2): Unknown
 * 3 (0x3): Idle
 * 4 (0x4): Printing
 * 5 (0x5): Warming Up
 * 6 (0x6): Stopped Printing
 * 7: Offline
 * 8 (0x8) : Paused
 * 9 (0x9): Error
 * 10 (0xA): Busy
 * 11 (0xB): Not Available
 * 12 (0xC): Waiting
 * 13 (0xD): Processing
 * 14 (0xE): Initialization
 * 15: Power Save
 * 16 (0x10): Pending Deletion
 * 17 (0x11): I/O Active
 * 18 (0x12): Manual Feed
 */

/**
 * https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-printer
 * Printer Status
 * Other (1)
 * Unknown (2)
 * Idle (3)
 * Printing (4)
 * Warmup (5)
 * Stopped Printing (6)
 * Offline (7)
 */

export function getPrinterStatus(status) {
    switch (status) {
        case 1:
            return 'Other'
        case 2:
            return 'Unknown'
        case 3:
            return 'Idle'
        case 4:
            return 'Printing'
        case 5:
            return 'Warming up'
        case 6:
            return 'Stopped printing'
        case 7:
            return 'Offline'
        default:
            return 'Other'
    }
}