using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Services
{
    public interface IIdentityProvider<T>
    {
        T CreateNewIdentity();
    }
}
