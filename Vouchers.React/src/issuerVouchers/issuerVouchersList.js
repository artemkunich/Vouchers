import React, {useState, useEffect, useRef, useCallback, useMemo} from 'react'
import {useSelector, useDispatch } from 'react-redux'
import {ListHeader, ListElement, CollapsedListElement} from '../components/list/list.tsx'
import {useLoadOnScroll} from '../api/helpers.ts'
import {api} from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const IssuerVouchersList = ({valuesItem, handleCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [vouchersFilter, setVouchersFilter] = useState({
        valueId: valuesItem.id,
        includePlanned: false,
        includeExpired: false,
    })

    const [vouchers, setVouchers] = useState([])

    const getVouchers = useCallback(async query => await api.getIssuerVouchers(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(vouchersFilter, getVouchers, setVouchers)

    const postVoucher = useCallback(async voucher => {
        const voucherId = await api.postIssuerVoucher(token, {...voucher, voucherValueId: valuesItem.id})
        if(voucherId)
            resetPageIndex()
    }, [api, token, getVouchers, vouchersFilter])


    const putVoucher = useCallback(async voucher => await api.putIssuerVoucher(token, {...voucher, voucherValueId: valuesItem.id}), [api, token, valuesItem])
    const postIssuerTransaction = useCallback(async transaction => await api.postIssuerTransaction(token, {...transaction, issuerAccountId: currentAccount.id}), [api, token])

    const handleSearch = (filter) => { 
        setVouchersFilter({
            ...vouchersFilter,
            ...filter
        })
        resetPageIndex()
    }

    return <>
        <IssuerVouchersListFilter initFilter={vouchersFilter} handleSearch={handleSearch} handleCancel={handleCancel}/>
        <NewIssuerVouchersListElement postVoucher={postVoucher}/>        
        {              
            vouchers.map((voucher, i) => <IssuerVouchersListElement key={voucher.id} vouchersItem={voucher} postIssuerTransaction={postIssuerTransaction} putVoucher={putVoucher}/>)
        }
    </>
}

const IssuerVouchersListFilter = ({initFilter, handleSearch, handleCancel}) => {

    const [filter, setFilter] = useState({
        includePlanned: initFilter.includePlanned,
        includeExpired: initFilter.includeExpired,
    })

    return (
        <ListElement>
            <div className="col-3 align-middle form-check">
                <label htmlFor={"includePlannedFilter"} className="form-check-label">Include planned</label>
                <input type="checkbox" id={"includePlannedFilter"} className="form-check-input" checked={filter.includePlanned} onChange={() => setFilter({...filter, includePlanned: !filter.includePlanned})}></input> 
            </div> 
            <div className="col-3 align-middle form-check">
                <label htmlFor={"includeExpiredFilter"} className="form-check-label">Include expired</label>
                <input type="checkbox" id={"includeExpiredFilter"} className="form-check-input" checked={filter.includeExpired} onChange={() => setFilter({...filter, includeExpired: !filter.includeExpired})}></input> 
            </div>  
            <div className="col-1 d-grid mt-2 mb-2" >
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => handleSearch(filter)}>Search</button>
            </div>  
            <div className="col-1 d-grid mt-2 mb-2" >
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={handleCancel}>Cancel</button>
            </div>  
        </ListElement> 
    )
}

const NewIssuerVouchersListElement = ({postVoucher}) => {

    const [newVoucher, setNewVoucher] = useState({
        validFrom: formatDate(new Date()),
        validTo: undefined,
        canBeExchanged: true
    })  

    return <ListHeader>
        <div className="col-4 align-middle"><p className="fs-5">New voucher</p></div>
        <div className="container">
            <div className="row mb-3">
                <div className="col-3" >
                    <label htmlFor="voucherValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="voucherValidFrom" className="form-control" value={newVoucher.validFrom} onChange={async (event) => setNewVoucher({...newVoucher, validFrom: event.target.value})}></input>                
                </div> 
                <div className="col-3" >
                    <label htmlFor="voucherValidTo" className="form-label">Valid to</label>
                    <input type="date" id="voucherValidTo" className="form-control" value={newVoucher.validTo} onChange={async (event) => setNewVoucher({...newVoucher, validTo: event.target.value})}></input>                
                </div> 
                <div className="col-3 align-middle form-check" >
                    <label htmlFor="voucherCanBeExchanged" className="form-check-label">Can be exchanged</label>
                    <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={newVoucher.canBeExchanged} onChange={async () => setNewVoucher({...newVoucher, canBeExchanged: !newVoucher.canBeExchanged})}></input>                
                </div>
                <div className="col-3">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await postVoucher(newVoucher)}><i className="bi bi-save"></i></button>
                </div>
            </div>                   
        </div>
    </ListHeader>
}

const IssuerVouchersListElement = ({vouchersItem, postIssuerTransaction, putVoucher}) => {

    const [voucher, setVoucher] = useState(vouchersItem)
    const [issuerQuantity, setIssuerQuantity] = useState(1)
    const [redeemQuantity, setRedeemQuantity] = useState(voucher.balance)
    const setRedeemQuantityWithValidation = (quantity) => {

        if(quantity > voucher.balance)
            quantity = voucher.balance

        setRedeemQuantity(quantity)          
    }

    useEffect(() => {
        setRedeemQuantity(voucher.balance)
    }, [voucher])

    const processIssuerTransactionAsync = useCallback(async (transaction) => {
        const transactionId = await postIssuerTransaction(transaction)
        transactionId && setVoucher({
            ...voucher,
            balance: voucher.balance + transaction.quantity,
            supply: voucher.supply + transaction.quantity
        })
    },[voucher])

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{voucher.validFrom.substring(0,10).replaceAll('-', '.')}-{voucher.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
            <div className="col-2 align-middle"><p className="fs-5">{voucher.balance}/{voucher.supply}</p></div>
            <div className="col-6 align-middle"><p className="fs-5">{voucher.canBeExchanged ? 'Exchangeable' : ''}</p></div>
        </>
        <div className="container">           
            <div className="row mb-3">
                <div className="col-4">
                    <label htmlFor="toIssue" className="form-label">Issue</label>
                    <input type="number" id="toIssue" className="form-control" value={issuerQuantity} onChange={async (event) => setIssuerQuantity(parseInt(event.target.value))}></input>           
                </div>
                <div className="col-1">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Issue" onClick={async () => await processIssuerTransactionAsync({voucherId: voucher.id, quantity: issuerQuantity})}><i className="bi bi-arrow-up"></i></button>
                </div>
                <div className="col-4">
                    <label htmlFor="toRedeem" className="form-label">Redeem</label>
                    <input type="number" id="toRedeem" className="form-control" value={redeemQuantity} onChange={async (event) => setRedeemQuantityWithValidation(parseInt(event.target.value))}></input>             
                </div>
                <div className="col-3">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Redeem" onClick={async () => await processIssuerTransactionAsync({voucherId: voucher.id, quantity: -redeemQuantity})}><i className="bi bi-arrow-down"></i></button>
                </div>
            </div>

            <div className="row mb-3">
                <div className="col-3" >
                    <label htmlFor="voucherValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="voucherValidFrom" className="form-control" value={voucher.validFrom.substring(0,10)} onChange={async (event) => setVoucher({...voucher, validFrom: event.target.value})}></input>                
                </div> 
                <div className="col-3" >
                    <label htmlFor="voucherValidTo" className="form-label">Valid to</label>
                    <input type="date" id="voucherValidTo" className="form-control" value={voucher.validTo.substring(0,10)} onChange={async (event) => setVoucher({...voucher, validTo: event.target.value})}></input>                
                </div> 
                <div className="col-3 align-middle form-check" >
                    <label htmlFor="voucherCanBeExchanged" className="form-check-label">Can be exchanged</label>
                    <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={voucher.canBeExchanged} onChange={async () => setVoucher({...voucher, canBeExchanged: !voucher.canBeExchanged})}></input>                
                </div>
                <div className="col-3">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await putVoucher(voucher)}><i className="bi bi-save"></i></button>
                </div>
            </div>                   
        </div>
    </CollapsedListElement>
}

const formatDate = (date) => {

    var month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day ].join('-');
}