import React, {useState, useEffect, useRef } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {createListHeader, createListElement, createCollapsedListElement, CollapsedListElement, ListElement} from '../list/listElement.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const NewDomain = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const [domainOffers, setDomainOffers] = useState([]);
    const [domainOffersFilter, setDomainOffersFilter] = useState({
        name: "",
        pageSize: 100
    });

    const getDomainOffers = useRef(async (query) => {
        setDomainOffersFilter(query)
        let result = await api.getIdentityDomainOffers(token, query)  
        if(!result)
            result = []
        
        return result;
    })

    useDomainOffers(identityId, getDomainOffers.current, setDomainOffers)

    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">New domain</h1>        
            {           
                domainOffers && domainOffers.map((offer, i) => <DomainOffer key={i} offersItem={offer} postDomainConract={async (domainName) => await api.postDomain(token, {offerId: offer.id, domainName: domainName})}/>)
            }
        </div>
    );
}

const DomainOffer = ({offersItem, postDomainConract}) => {

    const [domainName, setDomainName] = useState()

    return <ListElement>
        <>
            <div className="col-12 align-middle"><p className="fs-5">{offersItem.name}</p></div>
        </>
        <div className="container">
            <div className="row mb-2">
                <div className="col-3 align-middle">
                    <p className="my-0">Valid to: {offersItem.validTo.substring(0,10)}</p>
                    <p className="my-0">Price: {offersItem.amount} {offersItem.currency}/{offersItem.invoicePeriod}</p>
                    <p className="my-0">Max members: {offersItem.maxSubscribersCount}</p>
                    <p className="my-0">Contracts per identity: {`${offersItem.contractsPerIdentity ? offersItem.contractsPerIdentity: offersItem.maxContractsPerIdentity ? 0 : "-"}/${offersItem.maxContractsPerIdentity ? offersItem.maxContractsPerIdentity : "-"}`}</p>
                </div>
                <div className="col-4 align-middle">
                    <p className="my-0">Description:</p>
                    <p className="my-0">{offersItem.description}</p>
                </div>
                <div className="col-3">
                    <label htmlFor="domainName" className="form-label">Domain name</label>
                    <input type="text" id="domainName" className="form-control" value={domainName} onChange={async (event) => setDomainName(event.target.value)}></input>                
                </div>
                <div className="col-2">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} title="Create" onClick={async () => await postDomainConract(domainName)}>Create domain</button>
                </div> 
            </div> 
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

const useDomainOffers = (identityId, getDomainOffers, setDomainOffers) => {
    useEffect(() => {
        const effect = async () => {
            if(identityId) {
                const result = await getDomainOffers({pageSize: 100})    
                setDomainOffers(result)
            }
        }

        effect()
    },[]);
}