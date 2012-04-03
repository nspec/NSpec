using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;

namespace Contoso.ShapeProviders
{
    public class ContentShapeProvider : IShapeTableProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ContentShapeProvider(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Content")
                .OnDisplaying(displaying =>
                {
                    if (displaying.ShapeMetadata.DisplayType == "Detail")
                    {
                        ContentItem contentItem = displaying.Shape.ContentItem;
                        if (_workContextAccessor.GetContext().CurrentSite.HomePage.EndsWith(
                            ';' + contentItem.Id.ToString()))
                        {
                            displaying.ShapeMetadata.Alternates.Add("Content__HomePage");
                        }
                    }
                });
        }
    }
}