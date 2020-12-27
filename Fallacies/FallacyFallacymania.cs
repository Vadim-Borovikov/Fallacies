namespace Fallacies
{
    public class FallacyFallacyMania
    {
        public readonly int Id;
        public readonly string NameRu;
        public readonly string NameEn;
        public readonly string IconBackPath;
        public readonly string IconPath;
        public readonly string Description;
        public readonly string Examples;

        public FallacyFallacyMania(int id, string nameRu, string nameEn, string iconBackPath, string iconPath,
                                   string description, string examples)
        {
            Id = id;
            NameRu = nameRu;
            NameEn = nameEn;
            IconBackPath = iconBackPath;
            IconPath = iconPath;
            Description = description;
            Examples = examples;
        }
    }
}
