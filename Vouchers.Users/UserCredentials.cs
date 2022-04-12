using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Vouchers.Accounting;
using Vouchers.Application;

namespace Vouchers.Auth
{
    public class UserCredentials
    {
        public int Id { get; }

        public User User { get; }

        public string PassHash { get; private set; }
        public DateTime LastPassUpdate { get; private set; }

        public UserCredentials(User user, string passHash, DateTime lastPassUpdate)
        {
            User = user;
            PassHash = passHash;
            LastPassUpdate = lastPassUpdate;
        }

        public UserCredentials(int id, User user, string passHash, DateTime lastPassUpdate) : this(user, passHash, lastPassUpdate)
        {
            Id = id;
        }

        private UserCredentials() { }

        public void SetPassHash(string passHash) {
            PassHash = passHash;
            LastPassUpdate = DateTime.Now;
        }

        public bool Equals(UserCredentials userAccount) {
            return Id == userAccount.Id;
        }

        public bool NotEquals(UserCredentials userAccount)
        {
            return Id != userAccount.Id;
        }
    }
}
