﻿using CSharpFunctionalExtensions;

namespace Infrastructure.Http
{
    public interface IHttpManager
    {
        Task<Result<T>> Get<T>(string url, Dictionary<string, string> headerParams);
        Task<Result<T>> Post<T>(string url, Dictionary<string, string> postData);
        Task<Result> Put<T>(string data, string url, string token);
    }
}
