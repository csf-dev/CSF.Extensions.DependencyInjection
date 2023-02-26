using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace CSF.Extensions.DependencyInjection
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization())) {}
    }
}