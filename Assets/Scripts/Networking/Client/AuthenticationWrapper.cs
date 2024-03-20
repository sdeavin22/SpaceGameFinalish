using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;


/* This script creates a wrapper for handling authentication, allowing the game to authenticate users 
anonymously and handle various auth states, with error handling for erros that might occur during the authentication process */
public static class AuthenticationWrapper
{
    /* Property to store the current authentication state. The property has a getter that is accessible from anywhere 
    (since it's public), but the setter is private, so it can only be set within the class itself */
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    /* Function that performs the authentication. Task represents an async class, allowing for portions of code to finsish. 
    This allows other operations in the rest of the program to continue while waiting for the asynchronous operation to finish.*/
    public static async Task<AuthState> DoAuth(int maxRetries = 5)
    {
        // If user is already authenticated, just return the current state
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        // If already authenticating, wait for it to finsish doing so, and return the state
        if (AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already authenticating.");
            await Authenticating();
            return AuthState;
        }
        // If no authentication, then user can anonymously sign in
        await SignInAnonymouslyAsync(maxRetries);
        return AuthState;
    }


    // Function used for authentication waiting, 
    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }
        return AuthState;
    }



    // Function that performs anonymous sign in 
    private static async Task SignInAnonymouslyAsync(int maxRetries)
    {

        // Sets state to authenticating, and keeps track of a given number of authtication retries for the user
        AuthState = AuthState.Authenticating;
        int reTries = 0;

        // Loops until successful authentication, or the number of retries is depleted with error handling
        while (AuthState == AuthState.Authenticating && reTries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authException)
            {
                Debug.LogError(authException);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException requestException)
            {
                Debug.LogError(requestException);
                AuthState = AuthState.Error;
            }

            reTries++;
            await Task.Delay(1000);
        }

        if (AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"Player not signed in successfully after {reTries} reTries");
            AuthState = AuthState.TimeOut;
        }

    }
}

// Enum to declare/represent the state of authentication
public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}
