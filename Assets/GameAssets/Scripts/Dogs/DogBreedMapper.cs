using FeaturedClicker.Network.Features.Dogs;

namespace FeaturedClicker.Dogs
{
    public class DogBreedMapper
    {
        private const int MAX_BREEDS_COUNT = 10;

        public bool TryMapBreeds(DogBreedsResponse response, out DogBreedModel[] breeds)
        {
            breeds = new DogBreedModel[0];

            if (response == null || response.Data == null || response.Data.Length == 0)
            {
                return false;
            }

            int breedsCount = response.Data.Length < MAX_BREEDS_COUNT ? response.Data.Length : MAX_BREEDS_COUNT;
            breeds = new DogBreedModel[breedsCount];

            for (int i = 0; i < breedsCount; i++)
            {
                DogBreedData breedData = response.Data[i];

                if (breedData == null || breedData.Attributes == null)
                {
                    breeds = new DogBreedModel[0];

                    return false;
                }

                breeds[i] = new DogBreedModel(breedData.Id, breedData.Attributes.Name, breedData.Attributes.Description);
            }

            return true;
        }

        public bool TryMapBreed(DogBreedResponse response, out DogBreedModel breed)
        {
            breed = null;

            if (response == null || response.Data == null || response.Data.Attributes == null)
            {
                return false;
            }

            breed = new DogBreedModel(
                response.Data.Id,
                response.Data.Attributes.Name,
                response.Data.Attributes.Description);

            return true;
        }
    }
}
