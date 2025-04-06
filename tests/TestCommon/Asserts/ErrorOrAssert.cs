using ErrorOr;
using Xunit;

namespace TestCommon.Asserts;

public static partial class ErrorOrAssert
{
    public static void IsError<T>(ErrorOr<T> result, Error? expectedError = null)
    {
        Assert.True(result.IsError);

        if (expectedError is Error error)
            Assert.Equal(result.FirstError, error);
    }
}
