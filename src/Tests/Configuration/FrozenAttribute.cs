using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace LearnHub.Back.Tests.Configuration
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FrozenAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            // Ensure the parameter is not null
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            // Return a new instance of FreezingCustomization for the parameter's type
            return new FreezingCustomization(parameter.ParameterType);
        }
    }
}