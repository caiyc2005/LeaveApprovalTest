using LeaveApproval.IStore;
using LeaveApproval.IService;
using LeaveApproval.StoreContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Service
{
    public class BaseService<T>:IBaseService<T> where T : class
    {
        protected IBaseStore<T> baseStore;
        public BaseService(IBaseStore<T> baseStore)
        {
            this.baseStore = baseStore;
        }

        public bool Create(T entity)
        {
            return this.baseStore.Create(entity)>0;
        }

        public bool Delete(T entity)
        {
            return this.baseStore.Delete(entity) > 0;
        }
        public bool Update(T entity)
        {
            return this.baseStore.Update(entity)>0;
        }
        public T? Find(string id)
        {
            return this.baseStore.Find(id);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> lambda)
        {
            return this.baseStore.Query(lambda);
        }

    }
}
