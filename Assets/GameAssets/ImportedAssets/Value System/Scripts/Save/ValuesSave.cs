using FeaturedClicker.Save;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ValueSystem.Save
{
	[System.Serializable]
	public class ValuesSave : SavableData
    {
		public SerializedDictionary<string, float> ValueAmounts = new();

		public ValuesSave(Action saveAction) : base(saveAction) { }

		public float GetValue(string id, float defaultAmmount)
		{
			if (!ValueAmounts.ContainsKey(id))
			{
				ValueAmounts.Add(id, defaultAmmount);
			}

			return ValueAmounts[id];
		}

		public float GetValue(ValueData data)
		{
			return GetValue(data.Id, data.DefaulCount);
		}

		public void UpdateValue(string id, float amount)
		{
			if (!ValueAmounts.ContainsKey(id))
				ValueAmounts.Add(id, amount);
			else
				ValueAmounts[id] = amount;

			Save();
		}

		public void UpdateValue(ValueData data, float amount)
		{
			UpdateValue(data.Id, amount);
		}
	}
}

