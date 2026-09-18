using SmartScroll;

namespace FeaturedClicker.Dogs
{
    public class DogBreedScrollView : SmartScrollViewDirectional
    {
        protected override void Start()
        {
        }

        public void SetBreeds(DogBreedModel[] breeds)
        {
            Clear();
            CreateElements(breeds.Length);
        }
    }
}
