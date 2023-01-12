
using RestSharp;

namespace Todo.WebApp.Managers.Abstract
{
    public interface IService<T, TCreate, TEdit>
    {
        RestResponse<T> Create(TCreate model);
        RestResponse Delete(int id);
        RestResponse<T> GetById(int id);
        RestResponse<List<T>> List();
        RestResponse<T> Update(int id, TEdit model);
    }
}
