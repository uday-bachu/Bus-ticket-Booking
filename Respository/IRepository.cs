using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Busticket.Respository
{
    public interface IRepository<T> where T:class
    {
        // getall add update delete  find
        List<T> GetAll();

        bool add(T entity);

        bool update(object id , T entity);

        bool delete(object id);

        T find(Object id);
    }
}
