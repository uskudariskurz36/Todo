
using RestSharp;

namespace Todo.WebApp.Managers.Abstract
{
    public abstract class ServiceBase<T, TCreate, TEdit> : IService<T, TCreate, TEdit>
        where T : class, new()
        where TEdit : class, new()
        where TCreate : class, new()
    {
        private RestClient _client;
        private IConfiguration _configuration;

        protected string _endpoint;
        protected string _listEndpoint;
        protected string _createEndpoint;
        protected string _getByIdEndpoint;
        protected string _editEndpoint;
        protected string _deleteEndpoint;

        public abstract void SetEndPoints();

        public ServiceBase(IConfiguration configuration)
        {
            _configuration = configuration;
            SetEndPoints();
            _client = new RestClient(_endpoint);
        }

        public List<T> List()
        {
            RestRequest request = new RestRequest(_listEndpoint, Method.Get);
            return _client.Get<List<T>>(request);
        }

        public RestResponse<T> Create(TCreate model)
        {
            RestRequest request = new RestRequest(_createEndpoint, Method.Post);
            request.AddJsonBody(model);

            RestResponse<T> response = _client.ExecutePost<T>(request);
            //T todo = _client.Post<T>(request);

            T todo = response.Data;

            return response;
        }

        public RestResponse<T> GetById(int id)
        {
            RestRequest request = new RestRequest($"{_getByIdEndpoint}/{id}", Method.Get);
            return _client.ExecuteGet<T>(request);
        }

        public RestResponse<T> Update(int id, TEdit model)
        {
            RestRequest request = new RestRequest($"{_editEndpoint}/{id}", Method.Put);
            request.AddJsonBody(model);

            return _client.ExecutePut<T>(request);
        }

        public RestResponse Delete(int id)
        {
            RestRequest request = new RestRequest($"{_deleteEndpoint}/{id}", Method.Delete);
            return _client.Execute(request);
        }
    }
}