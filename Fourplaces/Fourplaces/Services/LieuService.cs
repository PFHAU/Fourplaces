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
using Xamarin.Forms;
using System.IO;

namespace Fourplaces.Services
{

    public interface ILieuService
    {
        Task<List<Lieu>> GetAllLieux();
        Task<string> CreateLieu(Lieu lieu);
        Task<Image> getUriImage(int Imageid);
        Task<List<Comment>> GetComments(int idApi);
        Task<string> SendMsg(int Id, string msg);
    }

    public class LieuService : ILieuService
    {
        private const string LIEU_LIST = "LIEUX";


        private List<Lieu> _lieuList;
        

        public LieuService() { }

        public async Task<List<Comment>> GetComments(int idApi)
        {
            ApiClient api = new ApiClient();

            

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Get,
                   "https://td-api.julienmialon.com/places/" + idApi);




            if (httpResponse.IsSuccessStatusCode)
            {
                Response<PlaceItem> response = await api.ReadFromResponse<Response<PlaceItem>>(httpResponse);
                List<CommentItem> comments = response.Data.Comments;
                List<Comment> _commentList;
                _commentList = new List<Comment>();

                foreach (CommentItem comment in comments)
                {
                    

                    _commentList.Add(new Comment()
                    {

                        Date = comment.Date,
                        Author = new User()
                        {
                            Email = comment.Author.Email,
                            FirstName = comment.Author.FirstName,
                            LastName = comment.Author.LastName,
                        },
                        Text = comment.Text
                    }); 
                }

                return _commentList;
            }
            else
            {
                Response response = await api.ReadFromResponse<Response>(httpResponse);
                return null;
            }
        }

        public async Task<string> SendMsg(int Id, string msg)
        {
            ApiClient api = new ApiClient();


            


            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Post,
                "https://td-api.julienmialon.com/places/" + Id + "/comments" ,
                new CreateCommentRequest()
                {
                    Text = msg
                }, ApiClient.Token);



            Response response = await api.ReadFromResponse<Response>(httpResponse);
            if (httpResponse.IsSuccessStatusCode)
            {
                return await Task.FromResult("");
            }
            else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }
        }

        public async Task<string> CreateLieu(Lieu lieu)
        {
            ApiClient api = new ApiClient();

            await InitializeIfNeeded();

            

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Post,
                "https://td-api.julienmialon.com/places",
                new PlaceItem()
                {
                    Title = lieu.Nom,
                    Description = lieu.Description,
                    ImageId = lieu.ImageId,
                    Comments = null,
                    Latitude = lieu.Position.Latitude,
                    Longitude = lieu.Position.Longitude
                }, ApiClient.Token);

            Response response = await api.ReadFromResponse<Response>(httpResponse);


            if (response.IsSuccess)
            {
                SaveLieux();
                return await Task.FromResult("");
            }
            else
            {
                return await Task.FromResult("Erreur " + response.ErrorCode + " : " + response.ErrorMessage);
            }

        }

        public async Task<List<Lieu>> GetAllLieux()
        {

            await InitializeIfNeeded();

            return await Task.FromResult(_lieuList);


        }

        private async Task InitializeIfNeeded()
        {
            if (_lieuList is null)
            {

                

                var serializedLieuList = CrossSettings.Current.GetValueOrDefault(LIEU_LIST, string.Empty);
                

                if (string.IsNullOrEmpty(serializedLieuList))
                {
                    
                    _lieuList = new List<Lieu>();
                }
                else
                {

                    _lieuList = JsonConvert.DeserializeObject<List<Lieu>>(serializedLieuList);
                }

                ApiClient api = new ApiClient();

                HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Get,
                    "https://td-api.julienmialon.com/places");

                Response<List<PlaceItem>> response = await api.ReadFromResponse<Response<List<PlaceItem>>>(httpResponse);

                if (httpResponse.IsSuccessStatusCode)
                {
                    foreach (PlaceItem lieu in response.Data)
                    {

                        _lieuList.Add(new Lieu(lieu.ImageId,
                            lieu.Title,
                            lieu.Description,
                            null,
                            new Xamarin.Forms.Maps.Position(lieu.Latitude, lieu.Longitude))
                        {
                            IdApi = lieu.Id,

                            
                        });
                    }
                }
            }
        }

        private void SaveLieux()
        {
            CrossSettings.Current.AddOrUpdateValue(LIEU_LIST, JsonConvert.SerializeObject(_lieuList));
        }

        public async Task<Image> getUriImage(int ImageId)
        {

            ApiClient api = new ApiClient();

            

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Get,
                "https://td-api.julienmialon.com/images/" + ImageId, null, ApiClient.Token);

            if (httpResponse.IsSuccessStatusCode)
            {
                HttpClient client = new HttpClient();

                byte[] result = await httpResponse.Content.ReadAsByteArrayAsync();

                Image image = new Image();

                image.Source = ImageSource.FromStream(() => new MemoryStream(result));




                return image;


            }
            else
            {
                return null;
            }
        }
 
    }
}