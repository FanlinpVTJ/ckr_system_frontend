namespace CkrSystem.Dogs
{
    public class DogBreedModel
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }

        public DogBreedModel(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
