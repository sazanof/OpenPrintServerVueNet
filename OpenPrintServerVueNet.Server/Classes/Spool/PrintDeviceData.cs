using OpenPrintServerVueNet.Classes.Spool.Native;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using System.Collections.Generic;

namespace OpenPrintServerVueNet.Classes.Spool {
    public class PrintDeviceData : DevModeData {
        public IDictionary<PrintDeviceField, PrintDeviceRecord> PrintDevice_Records { get; private set; } = new Dictionary<PrintDeviceField, PrintDeviceRecord>();

        public override IEnumerable<IRecord> AllRecords() {
            foreach (var item in base.AllRecords()) {
                yield return item;
            }

            foreach (var item in PrintDevice_Records.Values) {
                yield return item;
            }

        }

        public PrintDeviceRecord this[PrintDeviceField Index] {
            get {
                PrintDevice_Records.TryGetValue(Index, out var ret);

                return ret;
            }
        }

    }
    

}
