namespace Climbing.Web.Database
{
    using System;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore.Storage;

    internal sealed class ContextTransaction : ITransaction
    {
        private readonly IDbContextTransaction dbctxTransaction;

        private bool operationsComplete;

        private bool objectDisposed;

        public ContextTransaction(IDbContextTransaction dbctxTransaction)
        {
            Guard.NotNull(dbctxTransaction, nameof(dbctxTransaction));
            this.dbctxTransaction = dbctxTransaction;
        }

        public void Commit()
        {
            if (this.operationsComplete)
            {
                throw new InvalidOperationException("Transaction already completed");
            }

            this.dbctxTransaction.Commit();
            this.operationsComplete = true;
        }

        public void Dispose()
        {
            if (!this.operationsComplete)
            {
                this.Rollback();
            }

            if (!this.objectDisposed)
            {
                this.dbctxTransaction.Dispose();
                this.objectDisposed = true;
            }
        }

        public void Rollback()
        {
            if (this.operationsComplete)
            {
                throw new InvalidOperationException("Transaction already completed");
            }

            this.dbctxTransaction.Rollback();
            this.operationsComplete = true;
        }
    }
}