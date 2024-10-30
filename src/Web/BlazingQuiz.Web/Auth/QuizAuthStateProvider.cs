using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingQuiz.Web.Auth;

public class QuizAuthStateProvider : AuthenticationStateProvider
{
    private const string AuthType = "quiz-auth";
    private const string UserDataKey = "udata";

    private Task<AuthenticationState> _authStateTask;
    private readonly IJSRuntime _jsRuntime;    

    public LoggedInUser? User { get; private set; }
    public bool IsAuthenticated => User is not null;
    public bool IsInitializing { get; private set; } = true;

    public QuizAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;        

        SetAuthStateAsync();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return _authStateTask;        
    }

    public async Task SetLoginAsync(LoggedInUser user)
    {
        User = user;

        SetAuthStateAsync();
        NotifyAuthenticationStateChanged(_authStateTask);        

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKey, user.ToJson());
    }

    public async Task InitializeAsync()
    {
        try
        {
            var udata = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserDataKey);

            if (string.IsNullOrWhiteSpace(udata))
            {
                return;
            }

            var user = LoggedInUser.LoadFromJson(udata);

            if (user is null)
            {
                return;
            }

            await SetLoginAsync(user);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error => {ex.Message}");
        }
        finally
        {
            IsInitializing = false;
        }
    }

    public async Task SetLogout()
    {
        User = null;

        SetAuthStateAsync();
        NotifyAuthenticationStateChanged(_authStateTask);

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserDataKey);
    }

    private void SetAuthStateAsync()
    {
        if(IsAuthenticated)
        {
            var identity = new ClaimsIdentity(User!.GetClaims(), AuthType);
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);

            _authStateTask = Task.FromResult(authState);
        }
        else
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);

            _authStateTask = Task.FromResult(authState);
        }
    }
}
