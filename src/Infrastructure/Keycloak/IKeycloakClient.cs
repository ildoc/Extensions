﻿using CSharpFunctionalExtensions;

namespace Infrastructure.Keycloak
{
    public interface IKeycloakClient
    {
        Task<bool> CheckToken(string token);
        Task<KeycloackGetUserResponse> GetUserInfo(string mail);
        Task<KeycloackGetUserResponse> GetUserInfoByUsername(string username);
        Task<string> GetTokenUsingClientCredentials();
        Task<Result> ChangeUserPassword(string userId);
        Task<Result> ChangeMail(string mail, string userId);
        Task<Result> CreateUser(string userName, string email, string firstName, string lastName, Dictionary<string, string> attributes);

        Task<Result> ImpersonateUser(string userId, string token);
    }
}
