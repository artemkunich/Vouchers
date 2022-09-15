using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;
using Vouchers.WPF.Mono.Extensions;

namespace Vouchers.WPF.Mono
{
    public class ValuesService : IValuesService
    {
        Application.IValuesService valuesService;
        Application.Commands.DTOFactory dtoFactory;
       
        public ValuesService(Application.IValuesService service, Application.Commands.DTOFactory dtoFactory)
        {
            this.valuesService = service;
            this.dtoFactory = dtoFactory;
        }

        public VoucherValue CreateVoucherValue(string category, string name, string description, UserAccount authUser)
        {
            var valueDTO = dtoFactory.CreateVoucherValueDTO(category, name, description, authUser.Id);
            var createdValue = valuesService.CreateVoucherValue(valueDTO, authUser.GetUserAccount());
            return createdValue.GetVoucherValue();

        }

        public VoucherValue UpdateVoucherValue(int voucherValueId, string category, string name, string description, UserAccount authUser)
        {
            var valueDTO = dtoFactory.CreateVoucherValueDTO(category, name, description, authUser.Id);
            var updatedValue = valuesService.UpdateVoucherValue(voucherValueId, valueDTO, authUser.GetUserAccount());

            return updatedValue.GetVoucherValue();
        }

        public void RemoveVoucherValue(int voucherValueId, UserAccount authUser)
        {
            valuesService.RemoveVoucherValue(voucherValueId, authUser.GetUserAccount());
        }

        




        public Voucher AddVoucher(int voucherValueId, DateTime validFrom, DateTime validTo, bool canBeExchanged, UserAccount authUser)
        {
            var voucherDTO = dtoFactory.CreateVoucherDTO(validFrom, validTo, canBeExchanged);
            var createdVoucher = valuesService.AddVoucher(voucherValueId, voucherDTO, authUser.GetUserAccount());
            return createdVoucher.GetVoucher();
        }

        public Voucher UpdateVoucher(int voucherValueId, int voucherId, DateTime validFrom, DateTime validTo, bool canBeExchanged, UserAccount authUser)
        {
            var voucherDTO = dtoFactory.CreateVoucherDTO(validFrom, validTo, canBeExchanged);
            var updatedVoucher = valuesService.UpdateVoucher(voucherValueId, voucherId, voucherDTO, authUser.GetUserAccount());

            return updatedVoucher.GetVoucher();
        }

        public void RemoveVoucher(int voucherValueId, int voucherId, UserAccount authUser)
        {
            valuesService.RemoveVoucher(voucherValueId, voucherId, authUser.GetUserAccount());
        }
        

        public Action OnDispose { get; set; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
