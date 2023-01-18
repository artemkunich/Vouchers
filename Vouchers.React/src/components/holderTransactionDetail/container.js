import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { HolderTransactionDetailItem } from './item.js'
import { api } from '../../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const HolderTransactionDetailContainer = ({transaction, transactionRequest, handleCancel}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [vouchers, setVouchers] = useState([])
    const [currentTransaction, setCurrentTransaction] = useState({
        ...transaction,
        items: [],
        holderTransactionRequestId: transactionRequest?.id,
    })

    const [items, setItems] = useState({})

    const setItem = useCallback(item => {

        const newItems = items
        newItems[item.voucherId] = item.amount
        setItems(newItems)

        const newTransactionItems = []
        let newAmount = 0

        for(const voucherId of Object.keys(items)){
            if(items[voucherId] > 0) {
                newTransactionItems.push({
                    item1: voucherId,
                    item2: items[voucherId]
                })

                newAmount = newAmount + parseInt(items[voucherId]);
            }          
        }

        setCurrentTransaction({
            ...currentTransaction,
            items: newTransactionItems,
            amount: newAmount
        })
    }, [items,currentTransaction])

    const loadHolderVouchers = async (query) => {
        let vouchers = transaction.items.map(item => {return { ...item.unit, initialAmount: item.amount}})
        
        if(vouchers.length == 0)
            vouchers = await api.getHolderVouchers(token, query)
        
        if(!vouchers)
            vouchers = []

        if(![currentTransaction.debtorAccountId, currentTransaction.creditorAccountId].includes(currentTransaction.unitIssuerAccountId))
            vouchers = vouchers.filter(v => v.canBeExchanged)

        if(transactionRequest){
            if(transactionRequest.mustBeExchangeable)
                vouchers = vouchers.filter(v => v.canBeExchanged)

            if(transactionRequest.maxDaysBeforeValidityStart) {
                const maxValidityStart = new Date();
                maxValidityStart.setDate(maxValidityStart.getDate() + transactionRequest.maxDaysBeforeValidityStart);
                vouchers = vouchers.filter(v => v.validFrom <= maxValidityStart)
            }
            

            if(transactionRequest.minDaysBeforeValidityEnd) {
                const minValidityEnd = new Date();
                minValidityEnd.setDate(minValidityEnd.getDate() + transactionRequest.minDaysBeforeValidityEnd);
                vouchers = vouchers.filter(v => v.validTo >= minValidityEnd)
            }

            const sum = vouchers.reduce((sum, voucher) => sum + voucher.balance, 0)
            if(sum < transactionRequest.amount){
                alert(`Request can not be performed, max amount you can transfer is ${sum}`)
                handleCancel()
                return;
            }

            vouchers = vouchers.sort((v1, v2) => v1.validTo < v2.validTo ? -1 : v1.validTo > v2.validTo ? 1 : 0)

            let totalAmount = transactionRequest.amount

            vouchers.map(voucher => {
                const amount = Math.min(totalAmount, voucher.balance)
                voucher.initialAmount = amount               
                setItem({
                    voucherId: voucher.id,
                    amount: amount
                })
                totalAmount -= voucher.initialAmount

                return voucher;
            })          
        }

        setVouchers(vouchers)
    }

    const postHolderTransaction = async (transaction) => {
        const result = await api.postHolderTransaction(token, transaction) 
        if(result?.holderTransactionId){
            setCurrentTransaction({
                ...currentTransaction,
                id: result.holderTransactionId
            })
        }
    }

    useEffect(() => {
        const effectAsync = async () => {
            await loadHolderVouchers({valueId: transaction.unitTypeId})
        }

        effectAsync()
    },[])

    return <>
            <div className="row">
                <div className='col-2 me-auto'><h3>Transfer id:</h3></div><div className='col-10'><h3>{currentTransaction.id}</h3></div>
            </div>
            <div className="row">
                <div className='col-2 me-auto'><h3>Counterpaty:</h3></div><div className='col-10'><h3>{currentDomainAccount.id == currentTransaction.debtorAccountId ? currentTransaction.creditorName : currentTransaction.debtorName} ({currentDomainAccount.id == currentTransaction.debtorAccountId ? currentTransaction.creditorEmail : currentTransaction.debtorEmail})</h3></div>
            </div>
            <div className="row">
                <div className='col-2 me-auto'><h3>Ticker:</h3></div><div className='col-10'><h3>{currentTransaction.unitTicker}</h3></div>
            </div>
            <div className="row mb-2">
                <div className='col-2 me-auto'><h3>Total amount:</h3></div><div className='col-10'><h3>{currentTransaction.amount}{transactionRequest && `/${transactionRequest.amount}`}</h3></div>
            </div>
            <div className="row mb-2">
                <div className='col-2 me-auto'><h3>Vouchers:</h3></div>
            </div>
            {
                vouchers.map((voucher, i) => <HolderTransactionDetailItem key={voucher.id} transaction={currentTransaction} voucher={voucher} setItem={setItem}/>)
            }
            <div className="row mb-2">
                <div className="col-12">
                    <label  htmlFor="message" className="form-label">Message</label>
                    <textarea id="message" className="form-control" rows="auto" maxLength="1024" value={currentTransaction.message} onChange={async (event) => setCurrentTransaction({ ...currentTransaction, message: event.target.value })} disabled={currentTransaction.id}></textarea>  
                </div>
            </div>
            <div className="row mb-2">
                {
                    !currentTransaction.id &&
                    <div className="col-2 d-grid mt-2 mb-2">
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await postHolderTransaction(currentTransaction)}>Send</button>
                    </div>
                }
                <div className="col-2 d-grid mt-2 mb-2">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={handleCancel}>Cancel</button>
                </div>
            </div>
        </>
}