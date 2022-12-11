import React, {useState} from 'react'
import {ListElement} from '../common/list/list.tsx'

export const TransactionRequestsListFilter = ({handleSearch}) => {

    const [filter, setFilter] = useState({
        ticker: "",
        counterpartyName: "",
        includePerformed: false,
        includeNotPerformed: true
    });

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="transactionRequestTickerFilter" className="form-label">Ticker</label>
            <input type="text" id="transactionRequestTickerFilter" className="form-control" value={filter.ticker} onChange={(event) => setFilter({...filter, ticker: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="transactionRequestCounterpartyNameFilter" className="form-label">Counterparty name</label>
            <input type="text" id="transactionRequestCounterpartyNameFilter" className="form-control" value={filter.counterpartyName} onChange={(event) => setFilter({...filter, counterpartyName: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="transactionRequestAmountFilter" className="form-label">Amount</label>
            <input type="text" id="transactionRequestAmountFilter" className="form-control" value={filter.minAmount} onChange={(event) => setFilter({...filter, minAmount: event.target.value, maxAmount: event.target.value})}></input> 
        </div> 
        <div className="col-2 align-middle">
            <div className='container'>
                <div className='row'>
                    <div className="col-12 align-middle form-check">
                        <label htmlFor="includePerformed" className="form-check-label">Performed</label>
                        <input type="checkbox" id="includePerformed" className="form-check-input" checked={filter.includePerformed} 
                            onChange={() => setFilter({...filter, includePerformed: !filter.includePerformed})} >
                        </input> 
                    </div>
                    <div className="col-12 align-middle form-check">
                        <label htmlFor="includeNotPerformed" className="form-check-label">Not performed</label>
                        <input type="checkbox" id="includeNotPerformed" className="form-check-input" checked={filter.includeNotPerformed} 
                            onChange={() => setFilter({...filter, includeNotPerformed: !filter.includeNotPerformed})} >
                        </input>
                    </div>
                </div>
                
            </div>
        </div> 

        <div className="col-1 d-grid mb-2" >
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => handleSearch(filter)}>Search</button>
        </div> 
    </ListElement>
}