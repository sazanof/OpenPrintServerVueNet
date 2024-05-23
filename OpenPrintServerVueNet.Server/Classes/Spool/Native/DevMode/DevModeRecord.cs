using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using System.Diagnostics;

namespace OpenPrintServerVueNet.Classes.Spool.Native.DevMode {

    [DebuggerDisplay(Debugger2.DebuggerDisplay)]
    public abstract class DevModeRecord : IRecord, IRecordName<DevModeField> {
        public DevModeField Name { get; private set; }

        public object Value => GetValue();

        object IRecordName<object>.Name => Name;
        object IRecordValue<object>.Value => GetValue();

        protected abstract object GetValue();

        public DevModeRecord(DevModeField Name) {
            this.Name = Name;
        }

        public static DevModeRecord<TValue> Create<TValue>(DevModeField Name, TValue Value) {
            return new DevModeRecord<TValue>(Name, Value);
        }

        protected virtual string GetDebuggerDisplay() => IRecordExtensions.GetDebuggerDisplay(Name, Value);

    }

    public class DevModeRecord<TValue> : DevModeRecord, IRecordValue<TValue> {
        new public TValue Value { get; private set; }

        protected override object GetValue() {
            return Value;
        }

        public DevModeRecord(Native.DevMode.DevModeField Name, TValue Value) : base(Name) {
            this.Value = Value;
        }
    }

}
