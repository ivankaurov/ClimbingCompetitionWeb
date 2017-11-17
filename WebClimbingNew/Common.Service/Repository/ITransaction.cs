using System;
using System.Threading;
using System.Threading.Tasks;

namespace Climbing.Web.Common.Service.Repository
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}