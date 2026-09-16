using System;

namespace Cryptogram
{
    public class SavableData
    {
        protected Action _saveAction;

        public SavableData(Action saveAction)
        {
            _saveAction = saveAction;
        }

        public void Save()
        {
            _saveAction?.Invoke();
        }
    }
}
