using RestSharp;
using Todo.WebApp.Managers.Abstract;
using Todo.WebApp.Models;

namespace Todo.WebApp.Managers
{

    public interface ITodoService : IService<Models.Todo, TodoCreate, Models.Todo>
    {
        //RestResponse<Models.Todo> Create(TodoCreate model);
        //RestResponse Delete(int id);
        //RestResponse<Models.Todo> GetById(int id);
        //List<Models.Todo> List();
        //RestResponse<Models.Todo> Update(int id, Models.Todo model);

        RestResponse<string> Authenticate(SignInModel model);
    }

    public class TodoService : ServiceBase<Models.Todo, TodoCreate, Models.Todo>, ITodoService
    {
        public TodoService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        public override void SetEndPoints()
        {
            _endpoint = _configuration.GetValue<string>("Services:TodoService:EndPoint");
            _listEndpoint = "/Todo/List";
            _createEndpoint = "/Todo/Create";
            _editEndpoint = "/Todo/Edit";
            _getByIdEndpoint = "/Todo/GetById";
            _deleteEndpoint = "/Todo/Remove";
        }

        public RestResponse<string> Authenticate(SignInModel model)
        {
            RestRequest request = new RestRequest("/Account/SignIn", Method.Post);
            request.AddJsonBody(model);

            return _client.ExecutePost<string>(request);
        }

        //private RestClient _client;
        //private IConfiguration _configuration;

        //public TodoService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    _client = new RestClient(_configuration.GetValue<string>("Services:TodoService:EndPoint"));
        //}

        //public List<Models.Todo> List()
        //{
        //    RestRequest request = new RestRequest("/Todo/List", Method.Get);
        //    return _client.Get<List<Models.Todo>>(request);
        //}

        //public RestResponse<Models.Todo> Create(TodoCreate model)
        //{
        //    RestRequest request = new RestRequest("/Todo/Create", Method.Post);
        //    request.AddJsonBody(model);

        //    RestResponse<Models.Todo> response = _client.ExecutePost<Models.Todo>(request);
        //    //Models.Todo todo = client.Post<Models.Todo>(request);

        //    Models.Todo todo = response.Data;

        //    return response;
        //}

        //public RestResponse<Models.Todo> GetById(int id)
        //{
        //    RestRequest request = new RestRequest($"/Todo/GetById/{id}", Method.Get);
        //    return _client.ExecuteGet<Models.Todo>(request);
        //}

        //public RestResponse<Models.Todo> Update(int id, Models.Todo model)
        //{
        //    RestRequest request = new RestRequest($"/Todo/Edit/{id}", Method.Put);
        //    request.AddJsonBody(model);

        //    return _client.ExecutePut<Models.Todo>(request);
        //}

        //public RestResponse Delete(int id)
        //{
        //    RestRequest request = new RestRequest($"/Todo/Remove/{id}", Method.Delete);
        //    return _client.Execute(request);
        //}
    }
}