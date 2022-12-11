import React, { useState } from 'react'
import { ListElement } from '../common/list/list.tsx'

export const DomainValuesListFilter = ({handleSearch}) => {

    const [filter, setFilter] = useState({
        ticker: "",
        issuerName: ""
    });

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
            <input type="text" id="valueTickerFilter" className="form-control" value={filter.ticker} onChange={(event) => setFilter({...filter, ticker: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="valueIssuerFilter" className="form-label">Issuer name</label>
            <input type="text" id="valueTickevalueIssuerFilterrFilter" className="form-control" value={filter.issuerName} onChange={(event) => setFilter({...filter, issuerName: event.target.value})}></input> 
        </div> 
        <div className="col-2 align-middle" > 
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => handleSearch(filter)}>Search</button>
        </div>
    </ListElement>
}