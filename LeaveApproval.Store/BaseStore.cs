using LeaveApproval.Config;
using LeaveApproval.IStore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Store
{
    /// <summary>
    /// 公共数据处理具体实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseStore<T>:IBaseStore<T> where T : class
    {
        //获取数据库上下文对象
        public AppDbContext db = WebAppServices.dbContext!;

        public int Create(T entity)
        {
            db.Add(entity);
            return db.SaveChanges();
        }

        public int Update(T entity)
        {
            db.Update(entity);
            return db.SaveChanges();
        }

        public int Delete(T entity)
        {
            db.Remove(entity);
            return db.SaveChanges();
        }

        public T? Find(string id)
        {
            return db.Find<T>(new Guid(id).ToString());
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> lambda)
        {
            return db.Set<T>().AsNoTracking().Where(lambda);
        }
    }
}
