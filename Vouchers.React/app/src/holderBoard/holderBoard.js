import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {CollapsedListElement, ListElement} from '../list/listElement.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const HolderBoard = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [valuesFilter, setValuesFilter] = useState({
        ticker: "",
    });
    const [values, setValues] = useState([]);
    const [selectedValue, setSelectedValue] = useState();

    const [domainAccountsFilter, setDomainAccountsFilter] = useState({
        email: "",
        name: ""
    });
    const [domainAccounts, setDomainAccounts] = useState([]);
    const [selectedDomainAccount, setSelectedDomainAccount] = useState();

    const transactionApi = useMemo(() => {
        return {
            getValues: async (query) => {
                setValuesFilter(query)
                let result = await api.getValues(token, {...query, holderId: currentDomainAccount.id})  
                if(!result)
                    result = []
                
                setValues(result)
            },
            getDomainAccounts: async (query) => {
                let result = await api.getDomainAccounts(token, {...query, domainId: currentDomainAccount.domainId})  
                if(!result)
                    result = []
                
                setDomainAccounts(result)
            },
            getVouchers: async (query) => {
                let result = await api.getVouchers(token, query)  
                if(!result)
                    result = []
                
                return result
            },
            postTransaction: async (transaction) => {
                return await api.postTransaction(token, transaction)  
            }
        }
    },[api, currentDomainAccount, token])

    useEffect(() => { 
        setValuesFilter({
            ticker: "",
        })
        setSelectedValue()

        setDomainAccountsFilter({
            email: "",
            name: ""
        })
        setDomainAccounts([])
        setSelectedDomainAccount()


        transactionApi.getValues(valuesFilter)

    },[currentDomainAccount, token])

    const cancelTransaction = useCallback(() => {
        setSelectedValue()
        setSelectedDomainAccount()
    },[selectedValue, selectedDomainAccount])
    
    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Holder's values</h1>

            {
                selectedValue ?
                    selectedDomainAccount ?
                    <>
                        <NewTransfer creditorAccount={currentDomainAccount} debtorAccount={selectedDomainAccount} value={selectedValue} api={transactionApi} cancelTransaction={cancelTransaction}/>
                    </>                  
                    :
                    <>
                        <h2 className="header mt-2 mb-3">Please select recipient</h2>

                        <ListElement>
                            <div className="col-3 align-middle">
                                <label htmlFor="domainOfferNameFilter" className="form-label">Email</label>
                                <input type="text" id="domainOfferNameFilter" className="form-control" value={domainAccountsFilter.email} onChange={(event) => setDomainAccountsFilter({...domainAccountsFilter, email: event.target.value})}></input> 
                            </div>
                            <div className="col-3 align-middle">
                                <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
                                <input type="text" id="domainOfferNameFilter" className="form-control" value={domainAccountsFilter.name} onChange={(event) => setDomainAccountsFilter({...domainAccountsFilter, name: event.target.value})}></input> 
                            </div>  

                            <div className="col-1 mb-2" >
                                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => transactionApi.getDomainAccounts(domainAccountsFilter)}>Search</button>
                            </div>           
                        </ListElement>     
                        {              
                            domainAccounts.map((domainAccount, i) => <DomainAccount key={domainAccount.id} domainAccount={domainAccount} setSelectedDomainAccount={setSelectedDomainAccount}/>)
                        }
                    </>                  
                : 
                <>
                    <ListElement>
                        <div className="col-3 align-middle">
                            <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                            <input type="text" id="valueTickerFilter" className="form-control" value={valuesFilter.ticker} onChange={(event) => setValuesFilter({...valuesFilter, ticker: event.target.value})}></input> 
                        </div>  
                        <div className="col-1 mb-2" > 
                            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => transactionApi.getValues(valuesFilter)}>Search</button>
                        </div>           
                    </ListElement>
                    {
                        values.map((value, i) => <Value key={value.id} valueItem={value} setSelectedValue={setSelectedValue} api={transactionApi}/>)
                    }
                </>
            }           
        </div>
    );
}

const Value = ({valueItem, setSelectedValue, api}) => {

    const [value, setValue] = useState(valueItem)
    const [vouchers, setVouchers] = useState([])

    const getVouchers = async (query) => {

        let result = await api.getVouchers(query)  
        if(!result)
            result = []

        setVouchers(result)
    }

    useEffect(() => {
        const effect = async () => await getVouchers({valueId: value.id})

        effect()
    },[])

    return <CollapsedListElement>
        <div className="col-4 align-middle"><p className="fs-5">{value.ticker}</p></div>
        <div className="container">           
            <div className="row mb-4">
                <div className="col-12">
                    <label  htmlFor="valueDescription" className="form-label">Description</label>
                    <textarea id="valueDescription" className="form-control" rows="auto" disabled={true} value={value.description} onChange={async (event) => setValue({...value, description: event.target.value })}></textarea>               
                </div>    
            </div>

            <hr/>

            {              
                vouchers.map((voucher, i) => <Voucher key={voucher.id} voucher={voucher}/>)
            }

            <div className="row mb-2">
                <div className="col-3 d-grid">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={() => setSelectedValue(value)}>New transfer</button>
                </div>
            </div>
        </div>
    </CollapsedListElement>
}

const Voucher = ({voucher}) => {

    return <ListElement>
        <div className="col-6 align-middle"><p className="fs-5">{voucher.validFrom.substring(0,10).replaceAll('-', '.')}-{voucher.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
        <div className="col-3 align-middle">
            <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={voucher.canBeExchanged} disabled={true}></input>
        </div>
        <div className="col-3 align-middle"><p className="fs-5">{voucher.balance}</p></div>
    </ListElement>
}


const DomainAccount = ({domainAccount, setSelectedDomainAccount}) => {

    return <ListElement>
        <div className="container">
            <a title="New transfer" onClick={() => setSelectedDomainAccount(domainAccount)}>
                <div className="row mb-2">
                    <div className="col-2 align-middle">
                        { domainAccount.imageBase64 && <img style={{maxHeight: 100, maxWidth: 100}} src={`data:image/png;base64,${domainAccount.imageBase64}`}></img> }               
                    </div>            
                    <div className="col-4 align-middle">
                        <p className="fs-5">{domainAccount.email}</p>
                        <p className="fs-5">{domainAccount.name}</p>
                    </div>        
                </div>
            </a> 
        </div>
    </ListElement>
}

const NewTransfer = ({creditorAccount, debtorAccount, value, api, cancelTransaction}) => {
    
    const [vouchers, setVouchers] = useState([])
    const [transaction, setTransaction] = useState({
        creditorDomainAccountId: creditorAccount.id,
        debtorDomainAccountId: debtorAccount.id,
        quantity: 0,
        voucherValueId: value.id,
        items: []
    })

    const [items, setItems] = useState({})
    const setItem = useCallback(item => {

        const newItems = items
        newItems[item.voucherId] = item.quantity
        setItems(newItems)

        const newTransactionItems = []
        let newQuantity = 0

        for(const voucherId of Object.keys(items)){
            if(items[voucherId] > 0){
                newTransactionItems.push({
                    item1: voucherId,
                    item2: items[voucherId]
                })

                newQuantity = newQuantity + parseInt(items[voucherId]);
            }          
        }

        setTransaction({
            ...transaction,
            items: newTransactionItems,
            quantity: newQuantity
        })
    }, [items,transaction])

    const getVouchers = async(query) => {
        let vouchers = await api.getVouchers(query)
        if(!vouchers)
            vouchers = []

        if(![transaction.debtorDomainAccountId, transaction.creditorDomainAccountId].includes(value.issuerId))
            vouchers = vouchers.filter(v => v.canBeExchanged)

        setVouchers(vouchers)
    }

    const postTransaction = async (transaction) => {
        const transactionId = await api.postTransaction(transaction)
        if(transactionId)
            await getVouchers({valueId: value.id})
    }

    useEffect(() => {
        const effect = async () => {
            await getVouchers({valueId: value.id})
        }

        effect()
    },[])

    return <>
        <h3>Recipient: {debtorAccount.name}</h3>
        <h3>Ticker: {value.ticker}</h3>
        <h3 className='mb-3'>Total quantity: {transaction.quantity}</h3>
        <h3>Vouchers:</h3>
        <div className="container">

            <div className="row mb-2">
            {
                vouchers.map((voucher, i) => <ListElement key={voucher.id}><Item voucher={voucher} setItem={setItem}/></ListElement>)
            }
            </div>
            <div className="row mb-2">
                <div className="col-2 d-grid mt-2 mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await postTransaction(transaction)}>Send</button>
                </div>
                <div className="col-2 d-grid mt-2 mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={cancelTransaction}>Cancel</button>
                </div> 
            </div>
        </div>
    </> 
}

const Item = ({voucher, setItem}) => {

        const [quantity, setQuantityTemp] = useState();

        const setQuantity = (value) => {

            if(value > voucher.balance)
                value = voucher.balance

            setItem({
                voucherId: voucher.id,
                quantity: value
            })

            setQuantityTemp(value)           
        }

        return <div className="container">
            <div className="row">
                <div className="col-6 align-middle"><p className="fs-5">{voucher.validFrom.substring(0,10).replaceAll('-', '.')}-{voucher.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
                <div className="col-2 align-middle">
                    <input type="checkbox" id="voucherCanBeExchanged" className="form-check-input" checked={voucher.canBeExchanged} disabled={true}></input>
                </div>
                <div className="col-2 align-middle">
                    <input type="number" className="form-control" value={quantity ?? 0}  onChange={(event) => setQuantity(event.target.value == NaN ? 0 : event.target.value)}></input>           
                </div>
                <div className="col-2 align-middle"><p className="fs-5">{voucher.balance}</p></div>
            </div>
        </div>
}