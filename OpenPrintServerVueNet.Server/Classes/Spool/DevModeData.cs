using OpenPrintServerVueNet.Classes.Spool.Native;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using System.Collections.Generic;

namespace OpenPrintServerVueNet.Classes.Spool {
    public class DevModeData {
        public IDictionary<DevModeField, DevModeRecord> DevMode_Records { get; private set; } = new Dictionary<DevModeField, DevModeRecord>();

        public virtual IEnumerable<IRecord> AllRecords() {
            foreach (var item in DevMode_Records.Values) {
                yield return item;
            }
        }

        public DevModeRecord this[DevModeField Index] {
            get {
                DevMode_Records.TryGetValue(Index, out var ret);

                return ret;
            }
        }
    }
    

}
