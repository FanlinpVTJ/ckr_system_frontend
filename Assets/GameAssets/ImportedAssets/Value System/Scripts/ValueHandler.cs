using System;

namespace ValueSystem
{
    public class ValueHandler
    {
        public event Action OnValueChanged;
        public event Action<float> OnAdd;
        public event Action<float> OnSubtract;

        public readonly ValueData Data;

        public float Value
        {
            get => _value;
            private set
            {
                _value = value;
                OnValueChanged?.Invoke();
            }
        }

        public string Id => Data.Id;
        public bool IsInfinite => Data.IsInfinite;
        public float MaxCount => Data.MaxCount;
        public float MinCount => Data.MinCount;
        public bool IsFullFilled => IsInfinite ? false : Value >= MaxCount;

        private float _value;

        public ValueHandler(ValueData data, float value)
        {
            Data = data;
            Value = value;
        }

        public bool CanSubtract(float amount)
        {
            return Value - amount >= MinCount;
        }

        public bool TrySubtract(float amount, bool forced = false)
        {
            if (CanSubtract(amount) || forced)
            {
                Value -= amount;

                if (Value < MinCount)
                    Value = MinCount;

                OnSubtract?.Invoke(amount);
                return true;
            }

            return false;
        }

        public bool Add(float amount)
        {
            if (!IsInfinite)
            {
                if (IsFullFilled) return false;

                if (Value + amount > MaxCount)
                {
                    amount = MaxCount - Value;
                }
            }

            Value += amount;
            OnAdd?.Invoke(amount);

            return true;
        }

        public bool Change(float amount, bool forced = false)
        {
            if (amount >= 0)
                return Add(amount);
            else
                return TrySubtract(-amount);
        }

        public void Set(float value)
        {
            Value = value;
        }

    }
}

