import React, { useState } from 'react'
import { ListElement } from '../common/list/list.tsx'

export const HolderValuesListFilter = ({handleSearch}) => {

    const [filter, setFilter] = useState({
        ticker: "",
        issuerName: ""
    });

    return (
        <ListElement>
            <div className="col-3 align-middle">
                <label htmlFor="tickerFilter" className="form-label">Ticker</label>
                <input type="text" id="tickerFilter" className="form-control" value={filter.ticker} onChange={(event) => setFilter({...filter, ticker: event.target.value})}></input> 
            </div>
            <div className="col-3 align-middle">
                <label htmlFor="issuerNameFilter" className="form-label">Issuer name</label>
                <input type="text" id="issuerNameFilter" className="form-control" value={filter.issuerName} onChange={(event) => setFilter({...filter, issuerName: event.target.value})}></input> 
            </div> 
            <div className="col-1 d-grid mb-2" > 
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={()=>handleSearch(filter)}>Search</button>
            </div>                             
        </ListElement>
    )
}