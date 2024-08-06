using Microsoft.AspNetCore.Http;
using Sgd.Application.Common.Interfaces;
using Sgd.Application.Users.Commands.AddUser;

namespace Sgd.Infrastructure.CIAM.Auth0;

public class UserExistsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ICurrentUserProvider currentUserProvider,
        Auth0AuthenticationService authenticationService,
        ISender sender
    )
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        if (currentUser is null)
        {
            var auth0Id = currentUserProvider.GetTokenSub();
            if (string.IsNullOrEmpty(auth0Id))
            {
                await next(context);
                return;
            }

            var userResponse = await authenticationService.GetUser(auth0Id);
            if (userResponse is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var (system, value) = Auth0TokenUtility.GetSystemAndId(auth0Id);
            var addUserCommand = new AddUserCommand(
                userResponse.FirstName,
                userResponse.LastName,
                userResponse.Email,
                system,
                value
            );

            var result = await sender.Send(addUserCommand);
            if (result.IsError)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            currentUser = currentUserProvider.GetCurrentUser();
            if (currentUser is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await next(context);
    }
}
