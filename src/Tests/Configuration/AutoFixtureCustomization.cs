using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Back.Tests.Configuration;

public class AutoFixtureCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new AutoMoqCustomization());
        
        // Customize ActionResult types for controller tests
        fixture.Customize<ActionResult>(composer => composer.OmitAutoProperties());
        fixture.Customize<ObjectResult>(composer => composer.OmitAutoProperties());
        
        // Add more customizations as needed for specific types
    }
}