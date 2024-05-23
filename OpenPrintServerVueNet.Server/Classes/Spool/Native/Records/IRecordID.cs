using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native {
    public interface IRecordID<T> {
        T ID { get; }
    }

}
