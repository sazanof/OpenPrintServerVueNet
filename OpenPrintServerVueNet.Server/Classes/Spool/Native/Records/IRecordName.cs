using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native {

    public interface IRecordName<TFieldName> {
        TFieldName Name { get; }
    }
}
