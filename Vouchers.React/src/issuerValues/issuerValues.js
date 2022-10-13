import React, {useState, useEffect, useRef, useCallback, useMemo } from 'react'
import {useSelector, useDispatch } from 'react-redux'
import {ListHeader, ListElement, CollapsedListElement } from '../list/listElement.js'
import {getImageSrc } from '../imageSources.js';
import {ValueDetail} from '../valueDetail/valueDetail.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const IssuerValues = ({api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [values, setValues] = useState([]);
    const [valuesFilter, setValuesFilter] = useState({
        ticker: "",
    });
    const [selectedValueDetail, setSelectedValueDetail] = useState()
    const [selectedVouchers, setSelectedVouchers] = useState()

    const getValuesAsync = useCallback(async query => {
        let result = await api.getValues(token, {... query, issuerAccountId: currentDomainAccount.id})  
        if(!result)
            result = []
        
        setValues(result)
    }, [currentDomainAccount, token])

    useEffect(() => {

        setValuesFilter({
            ticker: "",
        })
        getValuesAsync(valuesFilter)

    }, [currentDomainAccount, token])

    return (
        <div className="container">           
            {
                selectedValueDetail ?
                    <ValueDetail header={"Issuer's value"} valuesItem={selectedValueDetail} api={api} onCancel={() => setSelectedValueDetail()}/>
                :
                selectedVouchers ? <Vouchers valuesItem={selectedVouchers} api={api} onCancel={() => setSelectedVouchers()}/>
                :
                <>
                    <h1 className="header mt-2 mb-3">Issuer's values</h1>
                    <ListElement>
                        <div className="col-3 align-middle">
                            <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                            <input type="text" id="valueTickerFilter" className="form-control" value={valuesFilter.ticker} onChange={(event) => setValuesFilter({...valuesFilter, ticker: event.target.value})}></input> 
                        </div>  
                        <div className="col-1 mb-2" > 
                            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await getValuesAsync(valuesFilter)}>Search</button>
                        </div> 
                    </ListElement>      
                    {          
                        values.map((value, i) => <Value key={value.id} valuesItem={value} setSelectedValueDetail={setSelectedValueDetail} setSelectedVouchers={setSelectedVouchers}/>)
                    }
                </>
            }
        </div>
    )
}

const Value = ({valuesItem, setSelectedValueDetail, setSelectedVouchers}) => {

    return <ListElement>
        <div className="col-2 align-middle">
            { valuesItem.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(valuesItem.imageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">{valuesItem.ticker}</p>
            <p>{valuesItem.description}</p>
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">Supply: {valuesItem.supply}</p>
        </div> 
        <div className=" col-2">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => setSelectedValueDetail(valuesItem)}>
                Detail
            </button>
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Vouchers"} onClick={() => setSelectedVouchers(valuesItem)}>
                Vouchers
            </button>
        </div>
        
    </ListElement>
}

const Vouchers = ({valuesItem, api, onCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [value, setValue] = useState(valuesItem)

    const [vouchersFilter, setVouchersFilter] = useState({
        valueId: value.id,
        includePlanned: false,
        includeExpired: false,
    })

    const [vouchers, setVouchers] = useState([])

    const getIssuerVouchersAsync = useCallback(async query => {
        let result = await api.getIssuerVouchers(token, query)  
        if(!result)
            result = []

        setVouchers(result)
    }, [api, token])

    const postIssuerVoucherAsync = useCallback(async voucher => {
        const voucherId = await api.postIssuerVoucher(token, {voucherValueId: value.id, voucher: voucher})
        if(voucherId)
            await getIssuerVouchersAsync(vouchersFilter) 
    }, [api, token, getIssuerVouchersAsync, vouchersFilter])


    const putIssuerVoucherAsync = useCallback(async voucher => await api.putIssuerVoucher(token, {voucherValueId: value.id, voucher: voucher}), [api, token, value])
    const postIssuerTransactionAsync = useCallback(async transaction => await api.postIssuerTransaction(token, {...transaction, issuerAccountId: currentDomainAccount.id}), [api, token])

    const voucherApi = useMemo(() => { return {
        
    }}, [api, token])

    useEffect(() => {
        const effect = async () => await getIssuerVouchersAsync(vouchersFilter)
        effect()
    }, [])

    return <>
        <h1 className="header mt-2 mb-3">Issuer's values</h1>
        <div className="col-4 align-middle"><p className="fs-5">{value.ticker} vouchers</p></div>
        <div className="container">           
            <div className="row mt-2">
                <ListElement>
                    <div className="col-3 align-middle form-check">
                        <label htmlFor={`voucherIncludePlannedFilter_${value.id}`} className="form-check-label">Include planned</label>
                        <input type="checkbox" id={`voucherIncludePlannedFilter_${value.id}`} className="form-check-input" checked={vouchersFilter.includePlanned} onChange={() => setVouchersFilter({...vouchersFilter, includePlanned: !vouchersFilter.includePlanned})}></input> 
                    </div> 
                    <div className="col-3 align-middle form-check">
                        <label htmlFor={`voucherIncludeExpiredFilter_${value.id}`} className="form-check-label">Include expired</label>
                        <input type="checkbox" id={`voucherIncludeExpiredFilter_${value.id}`} className="form-check-input" checked={vouchersFilter.includeExpired} onChange={() => setVouchersFilter({...vouchersFilter, includeExpired: !vouchersFilter.includeExpired})}></input> 
                    </div>  
                    <div className="col-1 d-grid mt-2 mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await getIssuerVouchersAsync(vouchersFilter)}>Search</button>
                    </div>  
                    <div className="col-1 d-grid mt-2 mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={onCancel}>Cancel</button>
                    </div>  
                </ListElement>
            </div>
            <div className="row mb-2">
                <NewVoucher postIssuerVoucher={postIssuerVoucherAsync}/>
            </div>
            {              
                vouchers.map((voucher, i) => <div key={voucher.id} className="row mb-2"><Voucher vouchersItem={voucher} postIssuerTransactionAsync={postIssuerTransactionAsync} putIssuerVoucherAsync={putIssuerVoucherAsync}/></div>)
            }
        </div>
    </>
}

const NewVoucher = ({postIssuerVoucher}) => {

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
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await postIssuerVoucher(newVoucher)}><i className="bi bi-save"></i></button>
                </div>
            </div>                   
        </div>
    </ListHeader>
}

const Voucher = ({vouchersItem, postIssuerTransactionAsync, putIssuerVoucherAsync}) => {

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
        const transactionId = await postIssuerTransactionAsync(transaction)
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
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Save" onClick={async () => await putIssuerVoucherAsync(voucher)}><i className="bi bi-save"></i></button>
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