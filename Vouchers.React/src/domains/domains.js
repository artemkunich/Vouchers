import { useSelector, useDispatch } from 'react-redux'
import React, {useState, useEffect } from 'react';
import { store, identityDefined } from '../store/userReducer.js';
import {ListElement} from '../list/listElement.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const Domains = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const [domains, setDomains] = useState([]);
    const [domainsFilter, setDomainsFilter] = useState({
        name: "",
        ownerName: ""
    });

    const getDomainsAsync = async (query) => {
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

                <div className="col-1 mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await getDomainsAsync(domainsFilter)}>Search</button>
                </div>           
            </ListElement>   
            {              
                domains.map((domain, i) => <Domain key={domain.id} domainsItem={domain} postSubscriptionRequest={async (domainId) => await api.postDomainAccount(token, {domainId: domainId})}/>)
            }
        </div>
    );
}

const Domain = ({domainsItem, postSubscriptionRequest}) => {

    return <ListElement>

        <div className="col-2 align-middle">
            { domainsItem.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainsItem.imageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{domainsItem.name}</p>
            <p>{domainsItem.description}</p>
        </div> 
        <div className=" col-2 d-grid mt-2 mb-2">
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} title={"Subscribe"} onClick={async () => await postSubscriptionRequest(domainsItem.id)}>
                {"Subscribe"}
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