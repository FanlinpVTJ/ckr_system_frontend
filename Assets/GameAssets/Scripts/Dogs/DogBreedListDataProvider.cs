using System;
using System.Collections.Generic;

namespace CkrSystem.Dogs
{
    public class DogBreedListDataProvider
    {
        public event Action<DogBreedModel> OnBreedSelected;

        private readonly List<DogBreedModel> _breeds = new List<DogBreedModel>();

        public void SetBreeds(DogBreedModel[] breeds)
        {
            _breeds.Clear();

            for (int i = 0; i < breeds.Length; i++)
            {
                _breeds.Add(breeds[i]);
            }
        }

        public DogBreedModel GetBreed(int index)
        {
            return _breeds[index];
        }

        public void SelectBreed(int index)
        {
            OnBreedSelected?.Invoke(_breeds[index]);
        }
    }
}
