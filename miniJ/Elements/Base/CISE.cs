using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Elements.Base
{
    class CISE
    {
        public enum CISEType
        {
            Class,
            Interface,
            Struct,
            Enum
        }

        public CISEType Type { get; set; }
    }
}
