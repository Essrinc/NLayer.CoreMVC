using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //dbcontexin bir savechange bir de savechangeasync isimli metodu var.
        //genelde transactionda commit ismi daha çok kullanıldığından ben burda commit ismini kullancam. 
        Task CommitAsync();
        void Commit();
    }
}
