using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native {
    public interface IRecord : IRecordName<object>, IRecordValue<object> {
    }

    public static class IRecordExtensions {
        public static string GetDebuggerDisplay(object Name, object Value) {
            var NameText = Name.ToString();
            string ValueText;
            switch(Value) {
                case string V1:
                    ValueText = V1;
                    break;
                case IEnumerable IE:
                    ValueText = string.Join(", ", IE.Cast<object>().Select(x => x.ToString()));
                    break;
                default:
                    ValueText = Value.ToString();
                    break;
            }; 

            return $@"{NameText} = {ValueText}";
        }
    }

}
