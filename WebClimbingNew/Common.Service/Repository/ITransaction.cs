namespace Climbing.Web.Common.Service.Repository
{
    using System;

    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}