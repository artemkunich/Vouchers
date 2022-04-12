using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public interface ISessions
    {
        Session GetSession(Guid tokenId);
        void Save(Session session);
    }
}
