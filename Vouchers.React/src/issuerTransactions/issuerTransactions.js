import React, {useState, useEffect } from 'react';
import {useSelector, useDispatch } from 'react-redux'
import {createListHeader, createListElement, createCollapsedListElement} from '../list/listElement.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const createIssuerTransactions = (getIssuerTransactionsApi) => () => {
    
    const token = useSelector(state => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const [issuerTransactionsFilter, setIssuerTransactionsFilter] = useState({
        
    });
    const [issuerTransactions, setIssuerTransactions] = useState([]);

    const getIssuerTransactions = async (query) => {
        setIssuerTransactionsFilter(query)
        let result = await getIssuerTransactionsApi(token, query)  
        if(!result)
            result = []
        
        setIssuerTransactions(result)
    }
    
    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Domain offers</h1>
            <IssuerTransactionsFilter getIssuerTransactions={getIssuerTransactions}/>       
            {              
                issuerTransactions.map((transaction, i) => {
                    const IssuerTransaction = createIssuerTransaction(transaction)
                    return <IssuerTransaction key={i}/>;
                })
            }
        </div>
    );
}

const IssuerTransactionsFilter = ({getIssuerTransactions}) => {

        const [issuerTransactionsFilter, setIssuerTransactionsFilter] = useState({})

        const Content = <>
            <div className="col-3 align-middle">
                <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
                <input type="text" id="domainOfferNameFilter" className="form-control" value={domainOffersFilter.name} onChange={(event) => setDomainOffersFilter({...domainOffersFilter, name: event.target.value})}></input> 
            </div>  
            <div className="col-3 align-middle form-check">
                <div>
                    <label htmlFor="domainIncludePlannedFilter" className="form-check-label">Include planned</label>
                    <input type="checkbox" id="domainIncludePlannedFilter" className="form-check-input" defaultChecked={domainOffersFilter.includePlanned} onClick={() => setIssuerTransactionsFilter({...domainOffersFilter, includePlanned: !domainOffersFilter.includePlanned})}></input> 
                </div>
                <div>
                    <label htmlFor="domainIncludeExpiredFilter" className="form-check-label">Include expired</label>
                    <input type="checkbox" id="domainIncludeExpiredFilter" className="form-check-input" defaultChecked={domainOffersFilter.includeExpired} onClick={() => setIssuerTransactionsFilter({...domainOffersFilter, includeExpired: !domainOffersFilter.includeExpired})}></input> 
                </div>              
            </div>
            <div className="col-5"></div>
            <div className="col-1 d-grid mt-2 mb-2" >
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => getIssuerTransactions(issuerTransactionsFilter)}>Search</button>
            </div>           
        </>

    return ListElement(Content)
}