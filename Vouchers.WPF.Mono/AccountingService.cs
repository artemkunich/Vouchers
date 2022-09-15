using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;
using Vouchers.WPF.Mono.Extensions;

namespace Vouchers.WPF.Mono
{
    class AccountingService : IAccountingService
    {
        Application.IAccountingService service;
        Application.Commands.DTOFactory dtoFactory;

        public AccountingService(Application.IAccountingService service, Application.Commands.DTOFactory dtoFactory) {
            this.service = service;
            this.dtoFactory = dtoFactory;
        }

        public void CreateIssuerTransaction(decimal amount, int voucherId, UserAccount authUser)
        {
            var quantity = service.CreateVoucherQuantity(amount, voucherId);
            service.CreateIssuerTransaction(quantity, authUser.GetUserAccount());
        }

        public void CreateHolderTransaction(string creditorId, string debtorId, decimal amount, int unitId, IEnumerable<VoucherQuantity> items, UserAccount authUser)
        {
            var valueQuantity = service.CreateVoucherValueQuantity(amount, unitId);
            var transactionDTO = dtoFactory.CreateTransactionDTO(
                creditorId,
                debtorId,
                valueQuantity
                );

            foreach(var item in items)
            {
                var quantity = service.CreateVoucherQuantity(item.Amount, item.Unit.Id);
                transactionDTO.AddItem(quantity);
            }
            

            var user = authUser.GetUserAccount();
            service.CreateHolderTransaction(transactionDTO, user);
        }        

        public Action OnDispose { get; set; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
