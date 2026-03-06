using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using webapi.Model.CorrespondenceModel;
using webapi.Services.IServices.ICorrespondenceService;

namespace webapi.Services.CorrespondenceService
{
    public class LetterCorrespondenceService : BaseService, ILetterCorrespondenceService
    {
        private readonly IHttpClientFactory _clientFactory;

        public LetterCorrespondenceService(IHttpClientFactory clientFactory):base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> addLetter<T>(setLetterDTO letterDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data = letterDTO,
                Url=SD.gatewayApiBase + "/api/Letter/addLetter"
            });
        }

        public async Task<T> getLetter<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.GET,
                Url=SD.gatewayApiBase +"/api/Letter/getLetter/"+id
            });
        }

        public async Task<T> getLetters<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url=SD.gatewayApiBase +"/api/Letter/getLetters"
            });
        }

        public async Task<T> updateLetter<T>(LetterDTO letterDTO)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType=SD.ApiType.POST,
                Data=letterDTO,
                Url=SD.gatewayApiBase +"/api/Letter/updateLetter"
            });
        }

        public async Task<T> Ping<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.gatewayApiBase +"/api/Letter/Ping"
            });
        }

        
    }
}
