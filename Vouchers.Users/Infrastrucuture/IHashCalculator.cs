using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Auth
{
    public interface IHashCalculator
    {
        string CalculateHash(string password);
        bool HasTheSameHash(string passHash, string password);
    }
}
