import React, {useState, useEffect, useRef } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {createListHeader, createListElement, createCollapsedListElement, CollapsedListElement} from '../list/listElement.js'
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
        let result = await api.getDomainOffers(token, query)  
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

    return <CollapsedListElement>
        <>
            <div className="col-5 align-middle"><p className="fs-5">{offersItem.name}</p></div>          
            <div className="col-4 align-middle"><p className="fs-5">{offersItem.amount} {offersItem.currency}/{offersItem.invoicePeriod}</p></div>
            <div className="col-3 align-middle">
                <p className="fs-5">
                    {offersItem.maxSubscribersCount} subscribers
                </p>
            </div>
        </>
        <div className="container">
            <div className="row mb-2">
                <div className="col-6">
                    <label htmlFor="offerValidTo" className="form-label">Valid to</label>
                    <input type="date" id="offerValidTo" className="form-control" value={offersItem.validTo.substring(0,10)} disabled></input>                
                </div>
                <div className="col-6">
                    <label htmlFor="contractsPerIdentity" className="form-label">Contracts per identity</label>
                    <input type="text" id="contractsPerIdentity" className="form-control" value={`${offersItem.contractsPerIdentity ? offersItem.contractsPerIdentity: offersItem.maxContractsPerIdentity ? 0 : "-"}/${offersItem.maxContractsPerIdentity ? offersItem.maxContractsPerIdentity : "-"}`} disabled></input>                
                </div>
            </div> 
            <div className="row mb-3">
                <label  htmlFor="domainOfferDescription" className="form-label">Description</label>
                <textarea id="domainOfferDescription" className="form-control" rows="auto" value={offersItem.description} disabled></textarea>               
            </div>
            <div className="row mb-2">
                <div className="col-6">
                    <label htmlFor="domainName" className="form-label">Domain name</label>
                    <input type="text" id="domainName" className="form-control" value={domainName} onChange={async (event) => setDomainName(event.target.value)}></input>                
                </div>
                <div className="col-6">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15}} title="Create" onClick={async () => await postDomainConract(domainName)}><i className="bi bi-briefcase"></i></button>
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