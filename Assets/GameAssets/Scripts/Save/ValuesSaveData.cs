using System;
using System.Collections.Generic;

namespace CkrSystem.Save
{
    [Serializable]
    public class ValuesSaveData
    {
        public List<ValueSaveEntry> Entries = new List<ValueSaveEntry>();
    }
}
