using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Views.Specifications;
using Vouchers.WPF.Model;
using Vouchers.WPF.Mono.Extensions;

namespace Vouchers.WPF.Mono
{
    public class ReportsService : IReportsService
    {
        Application.IReportsService service;

        public ReportsService(Application.IReportsService service)
        {
            this.service = service;
        }

        public IEnumerable<VoucherValue> GetIssuerVoucherValues(UserAccount authUser)
        {
            var values = service.GetIssuerVoucherValues(authUser.GetUserAccount());
            return values.Select(value => value.GetVoucherValue());
        }

        public IEnumerable<VoucherValue> GetHolderVoucherValues(UserAccount authUser)
        {
            var values = service.GetHolderVoucherValues(authUser.GetUserAccount());
            return values.Select(value => value.GetVoucherValue());
        }

        public IEnumerable<HolderTransaction> GetHolderTransactions(IEnumerable<Model.Specifications.TransactionSpecification> specifications, UserAccount authUser) {

            Specification resultSpecification = null;

            foreach (var specification in specifications) {
                
                var sp = (Specification)TransactionSpecificationExtensions.GetVouchersSpecification(specification as dynamic);
                if (resultSpecification is null) {
                    resultSpecification = sp;
                    continue;
                }

                resultSpecification = resultSpecification.And(sp);
            }

            var transactions = service.GetHolderTransactions(resultSpecification, authUser.GetUserAccount());
            return transactions.Select(t => t.GetHolderTransaction());
        }

        public IEnumerable<IssuerTransaction> GetIssuerTransactions(IEnumerable<Model.Specifications.TransactionSpecification> specifications, UserAccount authUser)
        {
            Specification resultSpecification = null;

            foreach (var specification in specifications)
            {
                var sp = (Specification)TransactionSpecificationExtensions.GetVouchersSpecification(specification as dynamic);
                if (resultSpecification is null)
                {
                    resultSpecification = sp;
                    continue;
                }
                resultSpecification = resultSpecification.And(sp);
            }

            var transactions = service.GetIssuerTransactions(resultSpecification, authUser.GetUserAccount());
            return transactions.Select(t => t.GetIssuerTransaction());
        }

        public IEnumerable<UserAccount> GetUserAccounts(IEnumerable<Model.Specifications.UserAccountSpecification> specifications, UserAccount authUser) {
            Specification resultSpecification = null;

            foreach (var specification in specifications)
            {
                var sp = (Specification)UserAccountSpecificationExtensions.GetVouchersSpecification(specification as dynamic);
                if (resultSpecification is null)
                {
                    resultSpecification = sp;
                    continue;
                }
                resultSpecification = resultSpecification.And(sp);
            }

            var userAccounts = service.GetUserAccounts(resultSpecification, authUser.GetUserAccount());
            return userAccounts.Select(t => t.GetUserAccount());
        }


        public Action OnDispose { get; set; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
