import React, { useState } from 'react'
import { ListElement } from '../common/list/list.tsx'
import "bootstrap-icons/font/bootstrap-icons.css";

export const HolderTransactionDetailItem = ({transaction, voucher, setItem}) => {

    const [amount, setAmountTemp] = useState(voucher.initialAmount);

    const setAmount = (value) => {

        if(value > voucher.balance)
            value = voucher.balance

        setItem({
            voucherId: voucher.id,
            amount: value
        })

        setAmountTemp(value)           
    }

    return <ListElement>
        <div className="container">
            <div className="row">
                <div className="col-5 align-middle"><p className="fs-5">{voucher.validFrom.substring(0,10).replaceAll('-', '.')}-{voucher.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
                <div className="col-3 align-middle">
                    { voucher.canBeExchanged ? "Exchangeable" : "Nonexchangeable" }
                </div>

                <div className="col-2 align-middle">
                    <input type="number" className="form-control" value={amount ?? 0} onChange={(event) => setAmount(event.target.value == NaN ? 0 : event.target.value)} disabled={transaction.id}></input>           
                </div>
                {
                    !transaction.id &&
                    <div className="col-2 align-middle"><p className="fs-5">{voucher.balance}</p></div>
                }
            </div>
        </div>
    </ListElement>

}