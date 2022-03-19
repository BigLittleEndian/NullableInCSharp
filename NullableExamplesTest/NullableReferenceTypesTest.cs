using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NullableExamples;

namespace NullableExamplesTest;

[TestClass]
public class NullableReferenceTypesTest
{
    [TestMethod]
    public void TestNullForgivingOperator()
    {
        var studentParker = new Student("Peter", "Parker", "Benjamin");

        Assert.ThrowsException<ArgumentNullException>(
            () => studentParker.SetFirstName(null!));
    }
}
