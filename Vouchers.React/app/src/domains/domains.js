import { useSelector, useDispatch } from 'react-redux'
import React, {useState, useEffect } from 'react';
import { store, identityDefined } from '../store/userReducer.js';
import {createListHeader, createListElement, ListElement, createCollapsedListElement, CollapsedListElement} from '../list/listElement.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const Domains = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const [domains, setDomains] = useState([]);
    const [domainsFilter, setDomainsFilter] = useState({
        name: "",
        ownerName: ""
    });

    const getDomains = async (query) => {
        setDomainsFilter(query)
        let result = await api.getDomains(token, query)  
        if(!result)
            result = []
        
        setDomains(result)
    }
    
    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Domains</h1>   
            <ListElement>
                <div className="col-3 align-middle">
                    <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
                    <input type="text" id="domainOfferNameFilter" className="form-control" value={domainsFilter.name} onChange={(event) => setDomainsFilter({...domainsFilter, name: event.target.value})}></input> 
                </div>
                <div className="col-3 align-middle">
                    <label htmlFor="domainOfferNameFilter" className="form-label">Owner name</label>
                    <input type="text" id="domainOfferNameFilter" className="form-control" value={domainsFilter.ownerName} onChange={(event) => setDomainsFilter({...domainsFilter, ownerName: event.target.value})}></input> 
                </div>  

                <div className="col-1 d-grid mt-2 mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await getDomains(domainsFilter)}>Search</button>
                </div>           
            </ListElement>   
            {              
                domains.map((domain, i) => <Domain key={domain.id} domainsItem={domain} postSubscriptionRequest={async (domainId) => await api.postDomainAccount(token, {domainId: domainId})}/>)
            }
        </div>
    );
}

const Domain = ({domainsItem, postSubscriptionRequest}) => {

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{domainsItem.name}</p></div>
            <div className="col-4 align-middle"><p className="fs-5">{domainsItem.membersCount}</p></div>
        </>
        <div className="container">
            <div className="row mb-2">
                <div className="col-12">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} title="Send request" onClick={async () => await postSubscriptionRequest(domainsItem.id)}><i className="bi bi-save"></i></button>
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

// const useDomainOffers = (token, identityId, getDomainOffers, setDomainOffers) => {
//     useEffect(() => {
//         const effect = async () => {
//             if(identityId) {
//                 const result = await getDomainOffers(token)    
//                 setDomainOffers(result)
//             }
//         }

//         effect()
//     },[]);
// }