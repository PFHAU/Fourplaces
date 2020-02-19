using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Fourplaces.Models;
using TD.Api.Dtos;
using System.Net.Http;
using MonkeyCache.SQLite;
using System;
using Common.Api.Dtos;

namespace Fourplaces.Services
{

    public interface IUserService
    {
        Task<string> Login(string email, string password);
        Task<string> Register(string email, string first_name, string last_name, string password);
        Task<User> Me();
        Task<string> ChangeMe(string first_name, string last_name);
        Task<string> ChangePassword(string old_password, string new_password);


    }
    public class UserService : IUserService
    {
        public User User { get; private set; }

        public UserService(){ }

        public async Task<string> ChangeMe(string first_name, string last_name)
        {
            ApiClient api = new ApiClient();


           

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Patch,
                "https://td-api.julienmialon.com/me", 
                new UpdateProfileRequest(){
                    FirstName = first_name,
                    LastName = last_name,
                    ImageId = 0
                },
            ApiClient.Token);

            Response<UserItem> response = await api.ReadFromResponse<Response<UserItem>>(httpResponse);

            if (response.IsSuccess)
            {

                User.FirstName = response.Data.FirstName;
                User.LastName = response.Data.LastName;

                return await Task.FromResult("Nom et Prenom Changé !");
            }
            else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }
        }

        public async Task<string> ChangePassword(string old_password, string new_password)
        {
            ApiClient api = new ApiClient();


            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Patch,
                "https://td-api.julienmialon.com/me/password",
                new UpdatePasswordRequest()
                {
                    OldPassword = old_password,
                    NewPassword = new_password
                },
            ApiClient.Token);

            Response response = await api.ReadFromResponse<Response>(httpResponse);

            if (response.IsSuccess)
            {
                //Barrel.Current.Add("User", User, TimeSpan.FromDays(1));

                

                return await Task.FromResult("");
            }
            else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }
        }

        public async Task<string> Login(string email, string password)
        {
            ApiClient api = new ApiClient();

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Post,
                "https://td-api.julienmialon.com/auth/login",
                new LoginRequest() { 
                    Email = email,
                    Password = password 
                });

            Response<LoginResult> response = await api.ReadFromResponse< Response<LoginResult> >(httpResponse);
     
            
            if (response.IsSuccess)
            {
                User = new User();
                User.Email = email;

                ApiClient.setRefreshToken(response.Data.RefreshToken, response.Data.AccessToken);


                //Barrel.Current.Add("Refresh_Token", _refresh_token,TimeSpan.FromDays(1));
                //Barrel.Current.Add("Token", _token, TimeSpan.FromSeconds(response.Data.ExpiresIn));

                return await Task.FromResult("");
            } else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }
        }

        public async Task<User> Me()
        {
            ApiClient api = new ApiClient();

            
            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Get, "https://td-api.julienmialon.com/me", null, ApiClient.Token);

            Response<UserItem> response = await api.ReadFromResponse<Response<UserItem>>(httpResponse);

            if (response.IsSuccess)
            {
                User = new User() {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    FirstName = response.Data.FirstName,
                    LastName = response.Data.LastName
                };

                //Barrel.Current.Add("User", User, TimeSpan.FromDays(1));

            }
            

            return await Task.FromResult(User);
        }

        

        public async Task<string> Register(string email, string first_name, string last_name, string password)
        {
            ApiClient api = new ApiClient();

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Post, 
                "https://td-api.julienmialon.com/auth/register", 
                new RegisterRequest() { 
                    Email = email,
                    Password = password,
                    FirstName = first_name,
                    LastName = last_name
            });

            Response<LoginResult> response = await api.ReadFromResponse<Response<LoginResult>>(httpResponse);


            if (response.IsSuccess)
            {
                User = new User();
                User.Email = email;
                User.FirstName = first_name;
                User.LastName = last_name;

                ApiClient.setRefreshToken(response.Data.RefreshToken, response.Data.AccessToken);

                //Barrel.Current.Add("Refresh_Token", _refresh_token, TimeSpan.FromDays(1));
                //Barrel.Current.Add("Token", _token, TimeSpan.FromSeconds(response.Data.ExpiresIn));

                return await Task.FromResult("");
            }
            else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }
        }


    }
}