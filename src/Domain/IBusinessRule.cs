using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    // Taken from https://github.com/kgrzybek/modular-monolith-with-ddd/blob/master/src/BuildingBlocks/Domain/IBusinessRule.cs
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}
