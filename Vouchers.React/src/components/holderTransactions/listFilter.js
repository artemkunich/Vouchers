import React, {useState} from 'react'
import {ListElement} from '../common/list/list.tsx'

export const HolderTransactionsListFilter = ({handleSearch}) => {

    const [filter, setFilter] = useState({
        ticker: "",
        counterpartyName: "",
        minAmount: "",
        maxAmount: ""
    })

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="tickerFilter" className="form-label">Ticker</label>
            <input type="text" id="tickerFilter" className="form-control" value={filter.ticker} onChange={(event) => setFilter({...filter, ticker: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="counterpartyNameFilter" className="form-label">Counterparty</label>
            <input type="text" id="counterpartyNameFilter" className="form-control" value={filter.counterpartyName} onChange={(event) => setFilter({...filter, counterpartyName: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="amountFilter" className="form-label">Amount</label>
            <input type="number" id="amountFilter" className="form-control" value={filter.minAmount} onChange={(event) => setFilter({...filter, minAmount: event.target.value, maxAmount: event.target.value})}></input> 
        </div>  
        <div className="col-1 d-grid mb-2" >
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => handleSearch(filter)}>Search</button>
        </div> 
    </ListElement>
}