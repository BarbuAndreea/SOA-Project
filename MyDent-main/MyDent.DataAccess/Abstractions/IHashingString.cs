using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.DataAccess.Abstactions
{
    public interface IHashingString
    {
        public string HashString(string unhashed);
    }
}
