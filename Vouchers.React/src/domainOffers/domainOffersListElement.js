import React, { useState } from 'react'
import { CollapsedListElement } from '../components/list/list.tsx'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainOffersListElement = ({offersItem, putDomainOffer}) => {
    const [offer, setOffer] = useState(offersItem)

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{offer.name}</p></div>
            <div className="col-2 align-middle">
                <p className="fs-5">
                    {offer.maxMembersCount}/{offer.maxContractsPerIdentity ? offer.maxContractsPerIdentity : "-"}
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