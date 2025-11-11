using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LearnHub.Back.Tests.Configuration;

public class ControllerCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // Setup common controller dependencies
        fixture.Customize<ControllerContext>(composer =>
            composer.Without(context => context.HttpContext));

        fixture.Customize<BindingInfo>(composer =>
            composer.Without(p => p.BinderType));
                
        fixture.Customize<ControllerContext>(composer =>
            composer.Without(p => p.ActionDescriptor));

        // Setup default HttpContext
        var httpContext = new DefaultHttpContext();
        var serviceProvider = new ServiceCollection()
            .AddScoped(_ => Mock.Of<IMediator>())
            .BuildServiceProvider();
        httpContext.RequestServices = serviceProvider;

        fixture.Inject(httpContext);
    }
}