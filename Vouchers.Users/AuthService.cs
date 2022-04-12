using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Vouchers.Application;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Auth
{
    public class AuthService : IAuthService
    {
        private readonly TokenFactory tokenFactory;

        private readonly IHashCalculator hashCalculator;
        private readonly UserCredentialsFactory userCredentialsFactory;
        private readonly IUserCredentialsRepository userCredentialsRepository;

        private readonly FactorFactory factorFactory;
        private readonly SessionFactory sessionFactory;
        private readonly ISessions sessions;
        private readonly IMFCodeGenerator codeGenerator;
        private readonly IUpdateEmailFactors updateEmailFactors;
        private readonly IUpdatePasswordFactors updatePasswordFactors;
        private readonly IAuthUserFactors authUserFactors;

        internal AuthService(
            TokenFactory tokenFactory,
            IHashCalculator hashCalculator,
            UserCredentialsFactory userCredentialsFactory,
            IUserCredentialsRepository userCredentialsRepository,
            SessionFactory sessionFactory,
            ISessions sessions,
            IMFCodeGenerator codeGenerator,
            IUpdateEmailFactors updateEmailFactors,
            IUpdatePasswordFactors updatePasswordFactors,
            IAuthUserFactors authUserFactors)
        {
            this.tokenFactory = tokenFactory;
            this.hashCalculator = hashCalculator;
            this.userCredentialsFactory = userCredentialsFactory;
            this.userCredentialsRepository = userCredentialsRepository;
            this.sessionFactory = sessionFactory;
            this.sessions = sessions;
            this.codeGenerator = codeGenerator;
            this.updateEmailFactors = updateEmailFactors;
            this.updatePasswordFactors = updatePasswordFactors;
            this.authUserFactors = authUserFactors;
        }


        public void SendFactorToGetToken(string domain, string loginName)
        {
            var authUserCredentials = userCredentialsRepository.GetByUserUId(domain, loginName);

            if (authUserCredentials == null)
                throw new ApplicationException("User or password is not correct");

            string code = codeGenerator.GenerateCode();
            codeGenerator.SendCodeToEmail(authUserCredentials.User.Email);

            var factor = factorFactory.CreateAuthUserFactor(authUserCredentials, code);
            authUserFactors.Save(factor);
        }

        public Token GetToken(string code, string domain, string loginName, string password)
        {
            var factor = authUserFactors.Get(domain, loginName);
            if (factor is null)
                throw new ApplicationException("Invalid credentials");

            if (factor.Code != code)
                throw new ApplicationException("Invalid credentials");

            var authUserCredentials = factor.UserCredentials;

            if (!hashCalculator.HasTheSameHash(authUserCredentials.PassHash, password))
                throw new ApplicationException("Invalid credentials");

            if (factor.CreationTime.AddSeconds(300) < DateTime.Now)
                throw new ApplicationException("Invalid credentials");

            var authUser = authUserCredentials.User;
            var session = sessionFactory.CreateSession(domain, loginName, authUser.Id);
            sessions.Save(session);
            return tokenFactory.CreateToken(session.TokenId, domain, loginName);
        }

        public void SendFactorToCreateUser(User newUser, string password)
        {
            //if (userCredentialsRepository.ContainsCredentialsForNickname(newUser.UserAccount.Domain, newUser.UserAccount.Nickname))
            //    throw new ApplicationException("Id is occupied");

            //if (userCredentialsRepository.ContainsCredentialsForEmail(newUser.UserAccount.Domain, newUser.Email))
            //    throw new ApplicationException("Email is occupied");

            var passHash = hashCalculator.CalculateHash(password);
            var newAccount = userCredentialsFactory.CreateUserCredentials(newUser, passHash);

            string code = codeGenerator.GenerateCode();
            codeGenerator.SendCodeToEmail(newUser.Email);
            var factor = factorFactory.CreateAuthUserFactor(newAccount, code);
            authUserFactors.Save(factor);
        }

        public User CreateUser(string code, string domain, string nickname)
        {
            var factor = authUserFactors.Get(domain, nickname);
            if (factor is null)
                throw new ApplicationException("Invalid credentials");

            if (factor.Code != code)
                throw new ApplicationException("Invalid credentials");

            var newUserCredentials = factor.UserCredentials;

            userCredentialsRepository.Add(newUserCredentials);
            userCredentialsRepository.Save();

            return newUserCredentials.User;
        }

        public void SendFactorToVerifyEmail(string email, User user)
        {
            try
            {
                MailAddress emailAddress = new MailAddress(email);
            }
            catch
            {
                throw new ApplicationException("Invalid email");
            }

            string code = codeGenerator.GenerateCode();
            codeGenerator.SendCodeToEmail(email);
            var factor = factorFactory.CreateUpdateEmailFactor(user.Id, email, code);
            updateEmailFactors.Save(factor);
        }

        public string GetVerifiedEmail(string code, User user)
        {
            var factor = updateEmailFactors.Get(user.Id);
            if (factor is null)
                throw new ApplicationException("Invalid code");

            if (factor.Code != code)
                throw new ApplicationException("Invalid code");

            if (factor.CreationTime.AddSeconds(300) < DateTime.Now)
                throw new ApplicationException("Invalid code");

            return factor.UserEmail;
        }

        public void SendFactorToResetPassword(string currentPassword, string newPassword, User user)
        {
            if (newPassword.Length < 6)
                throw new ApplicationException("Password must be at least 6 symbols");

            var userCredentials = userCredentialsRepository.GetByUserId(user.Id);
            if (userCredentials is null)
                throw new ApplicationException("User's credentials not found");

            if (!hashCalculator.HasTheSameHash(userCredentials.PassHash, currentPassword))
                throw new ApplicationException("User or password is not correct");

            var newPassHash = hashCalculator.CalculateHash(newPassword);

            string code = codeGenerator.GenerateCode();
            codeGenerator.SendCodeToEmail(user.Email);
            var factor = factorFactory.CreateUpdatePasswordFactor(user.Id, newPassHash, code);
            updatePasswordFactors.Save(factor);
        }

        public void ResetPassword(string code, User user)
        {
            var factor = updatePasswordFactors.Get(user.Id);
            if (factor is null)
                throw new ApplicationException("Invalid code");

            if (factor.Code != code)
                throw new ApplicationException("Invalid code");

            if (factor.CreationTime.AddSeconds(300) < DateTime.Now)
                throw new ApplicationException("Invalid code");

            var userCredentials = userCredentialsRepository.GetByUserId(user.Id);
            if (userCredentials is null)
                throw new ApplicationException("User's credentials not found");
            userCredentials.SetPassHash(factor.UserPasswordHash);

            userCredentialsRepository.Update(userCredentials);
            userCredentialsRepository.Save();
        }
    }
}
