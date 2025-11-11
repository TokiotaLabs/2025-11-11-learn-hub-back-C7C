using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LearnHub.Back.Tests.Configuration
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() 
            : base(() => 
            {
                var fixture = new Fixture()
                    .Customize(new AutoMoqCustomization());

                fixture.Customize<ControllerContext>(c => c
                    .OmitAutoProperties());
                    
                fixture.Customize<BindingInfo>(c => c
                    .OmitAutoProperties());

                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                return fixture;
            })
        {
        }
    }
}