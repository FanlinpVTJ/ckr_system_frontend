using System;
using System.Collections.Generic;

namespace FeaturedClicker.Save
{
    [Serializable]
    public class ValuesSaveData
    {
        public List<ValueSaveEntry> Entries = new List<ValueSaveEntry>();
    }
}
