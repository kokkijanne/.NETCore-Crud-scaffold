using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Api.Middleware.Services
{
    public interface IPinService
    {
        bool IsCorrect(string pin);
    }
}
