export const STATUS = {
    Blocked: {
        code: 512,
        c: 'Blocked',
        t: 'An error condition, possibly on a print job that precedes this one in the queue, blocked the print job.'
    },
    Completed: {
        code: 4096,
        c: 'Completed',
        t: 'The print job is complete, including any post-printing processing.'
    },
    Deleted: {
        code: 256,
        c: 'Deleted',
        t: 'The print job was deleted from the queue, typically after printing.'
    },

    Deleting: {
        code: 4,
        c: 'Deleting',
        t: 'The print job is in the process of being deleted.'
    },
    Error: {
        code: 2,
        c: 'Error',
        t: 'The print job is in an error state.'
    },
    None: {
        code: 0,
        c: 'None',
        t: 'The print job has no specified state.'
    },

    Offline: {
        code: 32,
        c: 'Offline',
        t: 'The printer is offline.'
    },

    PaperOut: {
        code: 64,
        c: 'Paper out',
        t: 'The printer is out of the required paper size.'
    },

    Paused: {
        code: 1,
        c: 'Paused',
        t: 'The print job is paused.'
    },

    Printed: {
        code: 128,
        c: 'Printed',
        t: 'The print job printed.'
    },
    Printing: {
        code: 16,
        c: 'Printing',
        t: 'The print job is now printing.'
    },

    Restarted: {
        code: 2048,
        c: 'Restarted',
        t: 'The print job was blocked but has restarted.'
    },

    Retained: {
        code: 8192,
        c: 'Retained',
        t: 'The print job is retained in the print queue after printing.'
    },

    Spooling: {
        code: 8,
        c: 'Spooling',
        t: 'The print job is spooling.'
    },

    UserIntervention: {
        code: 1024,
        c: 'User intervention',
        t: 'The printer requires user action to fix an error condition.'
    }
}

export function printJobStatus(_status) {
    let statuses = []
    Object.values(STATUS).forEach((status) => {
        if (_status & status.code) {
            statuses.unshift(status)
        }
    })
    return statuses
}