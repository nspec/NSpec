namespace NSpec.Domain
{
    public class ContextSelector
    {
        public ContextCollection Contexts { get; private set; }

        public Tags TagsFilter { get; private set; }

        public void Select(string binaryPath, string tagsText)
        {
            var reflector = new Reflector(binaryPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            TagsFilter = new Tags().Parse(tagsText);

            var builder = new ContextBuilder(finder, TagsFilter, conventions);

            Contexts = builder.Contexts().Build();
        }
    }
}
