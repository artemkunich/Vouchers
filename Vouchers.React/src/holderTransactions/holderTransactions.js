import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {HolderTransactionDetail} from '../holderTransactionDetail/holderTransactionDetail.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css"

export const HolderTransactions = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionsFilter, setTransactionsFilter] = useState({
        accountId: currentDomainAccount.id,
        ticker: "",
        counterpartyName: "",
    });
    const [transactions, setTransactions] = useState([]);
    const [selectedTransaction4Detail, setSelectedTransaction4Detail] = useState();

    const loadTransactionsAsync = useCallback(async (transactionsQuery) => {
        const transactions = await api.getHolderTransactions(token, transactionsQuery)
        if(transactions)
            setTransactions(transactions);
    }, [token, api])

    const onCancel = useCallback(async () => {
        setSelectedTransaction4Detail()
    }, [])

    useEffect(() => {
        const effectAsync = async () => {
            await loadTransactionsAsync(transactionsFilter);
        } 

        effectAsync();
    }, [token, currentDomainAccount])

    return <div className="container">
        <h1 className="header mt-2 mb-3">Holder ransactions</h1>
        {
            selectedTransaction4Detail ? 
            <HolderTransactionDetail transaction={selectedTransaction4Detail} api={api} onCancel={onCancel}/>
            : 
            <>
                <ListElement>
                    <div className="col-3 align-middle">
                        <label htmlFor="tickerFilter" className="form-label">Ticker</label>
                        <input type="text" id="tickerFilter" className="form-control" value={transactionsFilter.ticker} onChange={(event) => setTransactionsFilter({...transactionsFilter, ticker: event.target.value})}></input> 
                    </div>
                    <div className="col-3 align-middle">
                        <label htmlFor="counterpartyNameFilter" className="form-label">Counterparty</label>
                        <input type="text" id="counterpartyNameFilter" className="form-control" value={transactionsFilter.counterpartyName} onChange={(event) => setTransactionsFilter({...transactionsFilter, counterpartyName: event.target.value})}></input> 
                    </div>
                    <div className="col-3 align-middle">
                        <label htmlFor="amountFilter" className="form-label">Amount</label>
                        <input type="number" id="amountFilter" className="form-control" value={transactionsFilter.minAmount} onChange={(event) => setTransactionsFilter({...transactionsFilter, minAmount: event.target.value, maxAmount: event.target.value})}></input> 
                    </div>  
                    <div className="col-1 d-grid mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await loadTransactionsAsync(transactionsFilter)}>Search</button>
                    </div> 
                </ListElement>   
                {              
                    transactions.map((transaction, i) => <HolderTransaction key={transaction.id} transaction={transaction} setSelectedTransaction4Detail={setSelectedTransaction4Detail} />)
                }
            </>
        }

    </div>
}

const HolderTransaction = ({transaction, setSelectedTransaction4Detail}) => {

    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    return <ListElement>
        <div className="col-1 align-middle">
            { transaction.unitImageId && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(transaction.unitImageId)}></img> }
        </div>     
        <div className="col-4 align-middle">
            <p className="mb-0">Ticker: {transaction.unitTicker}</p>
            <p className='mb-0'>Issuer: {transaction.unitIssuerName} ({transaction.unitIssuerEmail})</p>
        </div> 
        <div className="col-1 align-middle">
            {(currentDomainAccount.id == transaction.creditorAccountId ? transaction.debtorImageId : transaction.creditorImageId) && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(currentDomainAccount.id == transaction.creditorAccountId ? transaction.debtorImageId : transaction.creditorImageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="mb-0">{transaction.timestamp.substring(0,19).replaceAll('T', ' ')}</p>
            <p className="mb-0">Counterparty: { currentDomainAccount.id == transaction.creditorAccountId ? `${transaction.debtorName} (${transaction.debtorEmail})` : `${transaction.creditorName} (${transaction.creditorEmail})`}</p>
            <p className='mb-0'>{`Amount: `} 
                <b style={{color: currentDomainAccount.id == transaction.creditorAccountId ? "red" : "#4CBB17"}}>
                    {transaction.amount}
                </b>
            </p>
        </div>  
        <div className=" col-1">         
        </div>
        <div className=" col-1">         
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15}} title={"Detail"} onClick={() => setSelectedTransaction4Detail(transaction)}>
                Detail
            </button>
        </div>

    </ListElement>
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