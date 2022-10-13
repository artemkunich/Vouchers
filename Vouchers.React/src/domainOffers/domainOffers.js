import React, {useState, useRef, useEffect, useCallback } from 'react';
import {useSelector, useDispatch } from 'react-redux'
import {createListHeader, createListElement, createCollapsedListElement, ListHeader, ListElement, CollapsedListElement} from '../list/listElement.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainOffers = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const identityId = useSelector(state => state.user.identityId)

    const [domainOffers, setDomainOffers] = useState([]);
    const [domainOffersFilter, setDomainOffersFilter] = useState({
        name: "",
        includeExpired: false,
        includePlanned: false,
        pageSize: 100
    });

    // useDomainOffers(token, identityId, getDomainOffers, setDomainOffers, domainOffers)

    const getDomainOffersAsync = useCallback(async (query) => {
        let result = await api.getDomainOffers(token, query)  
        if(!result)
            result = []
    
        setDomainOffers(result)
    }, [api, token])

    const postDomainOfferAsync = useCallback(async (domainOffer) => {
        const domainOfferId = await api.postDomainOffer(token, domainOffer)   
        if(domainOfferId){
            await getDomainOffersAsync(domainOffersFilter)  
        }
    }, [api, token, domainOffersFilter, getDomainOffersAsync])

    
    
    return (
        <div className="container">
            <h1 className="header mt-2 mb-3">Domain offers</h1>
            <ListElement>
                <div className="col-3 align-middle">
                    <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
                    <input type="text" id="domainOfferNameFilter" className="form-control" value={domainOffersFilter.name} onChange={(event) => setDomainOffersFilter({...domainOffersFilter, name: event.target.value})}></input> 
                </div>  
                <div className="col-3 align-middle form-check">
                    <div>
                        <label htmlFor="domainIncludePlannedFilter" className="form-check-label">Include planned</label>
                        <input type="checkbox" id="domainIncludePlannedFilter" className="form-check-input" defaultChecked={domainOffersFilter.includePlanned} onClick={() => setDomainOffersFilter({...domainOffersFilter, includePlanned: !domainOffersFilter.includePlanned})}></input> 
                    </div>
                    <div>
                        <label htmlFor="domainIncludeExpiredFilter" className="form-check-label">Include expired</label>
                        <input type="checkbox" id="domainIncludeExpiredFilter" className="form-check-input" defaultChecked={domainOffersFilter.includeExpired} onClick={() => setDomainOffersFilter({...domainOffersFilter, includeExpired: !domainOffersFilter.includeExpired})}></input> 
                    </div>              
                </div>
                <div className="col-5"></div>
                <div className="col-1 d-grid mt-2 mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => getDomainOffersAsync(domainOffersFilter)}>Search</button>
                </div>           
            </ListElement>
            <NewDomainOffer postDomainOffer={postDomainOfferAsync}/>         
            {              
                domainOffers.map((offer, i) => <DomainOffer key={offer.id} offersItem={offer} putDomainOffer={async (offerValues) => await api.putDomainOffer(token, offerValues)}/>)
            }
        </div>
    );
}

const NewDomainOffer = ({postDomainOffer}) => {

    const [newOffer, setNewOffer] = useState({
        name: "",
        maxSubscribersCount: 0,
        amount: 0,
        currency: "CZK",
        invoicePeriod: "MONTH",
        maxContractsPerIdentity: undefined,
        validFrom: formatDate(new Date()),
        validTo: undefined,
        description: ""
    })  

    return <ListHeader>
        <div className="col-4 align-middle"><p className="fs-5">New offer</p></div>
        <div className="container">
            <div className="mb-2" >
                <label htmlFor="domainOfferName" className="form-label">Name</label>
                <input type="text" id="domainOfferName" className="form-control" value={newOffer.name} onChange={async (event) => setNewOffer({...newOffer, name: event.target.value})}></input>                
            </div>
            <div className="row mb-2">
                <div className="col-3">
                    <label htmlFor="domainOfferMaxSubscribersCount" className="form-label">Max count of subscribers</label>
                    <input type="text" id="domainOfferMaxSubscribersCount" className="form-control" value={newOffer.maxSubscribersCount} onChange={async (event) => setNewOffer({...newOffer, maxSubscribersCount: Number(event.target.value)})}></input>                
                </div>
                <div className="col-3">
                    <label htmlFor="domainOfferInvoicePeriod" className="form-label">Invoice period</label>
                    <select id="domainOfferInvoicePeriod" className="form-select" value={newOffer.invoicePeriod} onChange={async (event) => setNewOffer({...newOffer, invoicePeriod: event.target.value})}>
                        <option value="MONTH">Month</option>
                        <option value="QUARTER">Quarter</option>                       
                        <option value="YEAR">Year</option>
                    </select>
                </div>
                <div className="col-3">
                    <label htmlFor="domainOfferAmount" className="form-label">Amount</label>
                    <input type="text" id="domainOfferAmount" className="form-control" value={newOffer.amount} onChange={async (event) => setNewOffer({...newOffer, amount: parseFloat(event.target.value)})}></input>                
                </div>
                <div className="col-3">
                    <label htmlFor="domainOfferCurrency" className="form-label">Currency</label>
                    <select id="domainOfferCurrency" className="form-select" value={newOffer.currency} onChange={async (event) => setNewOffer({...newOffer, currency: event.target.value})}>
                        <option value="USD">USD</option>
                        <option value="EUR">EUR</option>
                        <option value="CZK">CZK</option>
                        <option value="RUB">RUB</option>
                    </select>
                </div>  
            </div>

            <div className="row mb-2">
                <div className="col-4">
                    <label htmlFor="domainOfferMaxContractsPerIdentity" className="form-label">Max contracts per identity</label>
                    <input type="text" id="domainOfferValidFrom" className="form-control" value={newOffer.maxContractsPerIdentity} onChange={async (event) => setNewOffer({...newOffer, maxContractsPerIdentity: parseInt(event.target.value)})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="domainOfferValidFrom" className="form-control" value={newOffer.validFrom} onChange={async (event) => setNewOffer({...newOffer, validFrom: event.target.value})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidTo" className="form-label">Valid to</label>
                    <input type="date" id="domainOfferValidTo" className="form-control" value={newOffer.validTo} onChange={async (event) => setNewOffer({...newOffer, validTo: event.target.value})}></input>                
                </div>
            </div>                        

            <div className="row mb-3">
                <label  htmlFor="domainOfferDescription" className="form-label">Description</label>
                <textarea id="domainOfferDescription" className="form-control" rows="5" value={newOffer.description} onChange={async (event) => setNewOffer({...newOffer, description: event.target.value})}></textarea>               
            </div>
            <div className="row mb-2">
                <div className="d-grid col-3">
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={async () => await postDomainOffer(newOffer)}>Save</button>
                </div>
            </div>
        </div>
    </ListHeader>
}

const DomainOffer = ({offersItem, putDomainOffer}) => {
    const [offer, setOffer] = useState(offersItem)

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{offer.name}</p></div>
            <div className="col-2 align-middle">
                <p className="fs-5">
                    {offer.maxSubscribersCount}/{offer.maxContractsPerIdentity ? offer.maxContractsPerIdentity : "-"}
                </p>
            </div>
            <div className="col-3 align-middle"><p className="fs-5">{offer.amount} {offer.currency}/{offer.invoicePeriod}</p></div>
            <div className="col-3 align-middle"><p className="fs-5">{offer.validFrom.substring(0,10).replaceAll('-', '.')} - {offer.validTo.substring(0,10).replaceAll('-', '.')}</p></div>
        </>
        <div className="container">
            <div className="row mb-2">
                <div className="col-4">
                    <label htmlFor="domainOfferMaxContractsPerIdentity" className="form-label">Max contracts per identity</label>
                    <input type="text" id="domainOfferValidFrom" className="form-control" value={offer.maxContractsPerIdentity} onChange={async (event) => setOffer({...offer, maxContractsPerIdentity: parseInt(event.target.value)})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="domainOfferValidFrom" className="form-control" value={offer.validFrom.substring(0,10)} onChange={async (event) => setOffer({...offer, validFrom: event.target.value})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidTo" className="form-label">Valid to</label>
                    <input type="date" id="domainOfferValidTo" className="form-control" value={offer.validTo.substring(0,10)} onChange={async (event) => setOffer({...offer, validTo: event.target.value})}></input>                
                </div>
            </div>
            <div className="row mb-3">
                <label  htmlFor="domainOfferDescription" className="form-label">Description</label>
                <textarea id="domainOfferDescription" className="form-control" rows="auto" value={offer.description} onChange={async (event) => setOffer({...offer, description: event.target.value})}></textarea>               
            </div>
            <div className="row mb-2">
                <div className="col-12">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} title="Save" onClick={async () => await putDomainOffer(offer)}><i className="bi bi-save"></i></button>
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} data-bs-toggle="tooltip" data-bs-placement="top" title="Terminate" onClick={async () => await putDomainOffer({...offer, terminate: true})} disabled={offer.validTo < new Date()}><i className="bi bi-x-lg"></i></button>
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