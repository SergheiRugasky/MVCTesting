using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MVCTesting.Tests
{
    [TestFixture]
    class CreateInterfaceExtensionTests
    {
        [Test]
        public void Should_say_hello()
        {
            Teachear t = new Teachear();
            Student s = new Student();

            t.SayHello().Should().Be("Hello Teacher");
            s.SayHello().Should().Be("Hello Student");

            t = null;
            t.Should().BeNull();
        }
    }

    public interface ITest
    {
        string IHaveType();
    }

    class Student : ITest
    {
        public string IHaveType()
        {
            return "Student";
        }
    }

    class Teachear : ITest
    {
        public string IHaveType()
        {
            return "Teacher";
        }
    }

    public static class ITestExtension
    {
        public static string SayHello(this ITest testObject)
        {
            return "Hello " + testObject.IHaveType();
        }
    }
}
