using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tyne.HttpMediator;

public static class AssertProblemDetails
{
    public static void NotEmpty(ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(problemDetails);

        Assert.NotNull(problemDetails.Type);
        Assert.NotNull(problemDetails.Title);
        Assert.NotNull(problemDetails.Detail);
        Assert.NotNull(problemDetails.Instance);
    }

    public static void HasStatus(HttpStatusCode statusCode, ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(problemDetails);

        Assert.Equal((int?)statusCode, problemDetails.Status);
    }
}
