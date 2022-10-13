import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {HolderTransactionDetail} from '../holderTransactionDetail/holderTransactionDetail.js'
import {DomainAccounts} from '../domainAccounts/domainAccounts.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css";

const defaultValuesFilter = {
    ticker: "",
    issuerName: "", 
}

export const HolderValues = ({api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [valuesFilter, setValuesFilter] = useState(defaultValuesFilter);
    const [values, setValues] = useState([]);

    const [selectedValue, setSelectedValue] = useState();
    const [selectedDebtorAccount, setSelectedDebtorAccount] = useState();

    const loadHolderValuesAsync = useCallback(async (query) => {
        let result = await api.getHolderValues(token, {...query, holderId: currentDomainAccount.id})  
        if(!result)
            result = []
        
        setValues(result)
    }, [api, token, currentDomainAccount])

    const onCancel = useCallback(() => {
        setSelectedValue()
        setSelectedDebtorAccount()
    }, [])

    useEffect(() => { 
        setValuesFilter(defaultValuesFilter)
        loadHolderValuesAsync(defaultValuesFilter)

    }, [loadHolderValuesAsync])
    
    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Holder's values</h1>
            {
                 !selectedValue ?
                    <>
                        <ListElement>
                            <div className="col-3 align-middle">
                                <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                                <input type="text" id="valueTickerFilter" className="form-control" value={valuesFilter.ticker} onChange={(event) => setValuesFilter({...valuesFilter, ticker: event.target.value})}></input> 
                            </div>
                            <div className="col-3 align-middle">
                                <label htmlFor="transactionRequestIssuerNameFilter" className="form-label">Issuer name</label>
                                <input type="text" id="transactionRequestIssuerNameFilter" className="form-control" value={valuesFilter.issuerName} onChange={(event) => setValuesFilter({...valuesFilter, issuerName: event.target.value})}></input> 
                            </div> 
                            <div className="col-1 d-grid mb-2" > 
                                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await loadHolderValuesAsync(valuesFilter)}>Search</button>
                            </div>                             
                        </ListElement>
                        {
                            values.map((value, i) => <Value key={`${value.id} ${value.balance}`} valueItem={value} setSelectedValue={setSelectedValue} api={api}/>)
                        }
                    </>
                 :
                 !selectedDebtorAccount ?
                    <DomainAccounts header={"Please select recipient"} setSelectedDomainAccount={setSelectedDebtorAccount} onCancel={onCancel} api={api}/>
                :
                    <HolderTransactionDetail transaction={{
                        creditorAccountId: currentDomainAccount.id,
                        debtorAccountId: selectedDebtorAccount?.id,
                        debtorName: selectedDebtorAccount.name,
                        debtorEmail: selectedDebtorAccount.email,
                        amount: 0,
                        unitTypeId: selectedValue.id,
                        unitIssuerAccountId: selectedValue.issuerAccountId,
                        unitTicker: selectedValue.ticker,
                        items: [],
                        message: ""
                    }} api={api} onCancel={onCancel}/>
                
            }           
        </div>  
    );
}

const Value = ({valueItem, setSelectedValue, api}) => {

    const token = useSelector(state => state.user.token)

    const [value, setValue] = useState(valueItem)
    const [vouchers, setVouchers] = useState([])

    const loadHolderVouchersAsync = async (query) => {

        let result = await api.getHolderVouchers(token, query)  
        if(!result)
            result = []

        setVouchers(result)
    }

    useEffect(() => {
        const effectAsync = async () => await loadHolderVouchersAsync({valueId: value.id})

        effectAsync()
    },[])

    return <ListElement>

        <div className="col-2 align-middle">
            { value.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(value.imageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">{value.ticker}</p>
            <p className='mb-0'>{value.description}</p>
            <p className='mb-0'>Issuer: {value.issuerName} ({value.issuerEmail})</p>
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">Balance: {value.balance}</p>
        </div> 
        <div className=" col-2">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"New transfer"} onClick={() => setSelectedValue(value)}>
                New transfer
            </button>
        </div>

    </ListElement>
}