using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public interface IIdentityObject<T>
    {
        T Id { get; } 
    }
}
