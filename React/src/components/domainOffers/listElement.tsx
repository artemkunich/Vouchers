import * as React from 'react'
import { CollapsedListElement } from '../common/list/list'
import { DomainOfferDto } from '../../types/dtos';
import { UpdateDomainOfferCommand } from '../../types/commands';

import "bootstrap-icons/font/bootstrap-icons.css";

interface DomainOffersListElementProps {
    offer: DomainOfferDto,
    putDomainOffer: (command: UpdateDomainOfferCommand) => Promise<boolean | undefined>
}

export const DomainOffersListElement = ({offer, putDomainOffer}: DomainOffersListElementProps) => {
    const [offerData, setOfferData] = React.useState(offer)

    return <CollapsedListElement>
        <>
            <div className="col-4 align-middle"><p className="fs-5">{offerData.name}</p></div>
            <div className="col-2 align-middle">
                <p className="fs-5">
                    {offerData.maxMembersCount}/{offerData.maxContractsPerIdentity ? offerData.maxContractsPerIdentity : "-"}
                </p>
            </div>
            <div className="col-3 align-middle"><p className="fs-5">{offerData.amount} {offerData.currency}/{offerData.invoicePeriod}</p></div>
            <div className="col-3 align-middle"><p className="fs-5">{offerData.validFrom.substring(0,10).replace(/-/g, '.')} - {offerData.validTo.substring(0,10).replace(/-/g, '.')}</p></div>
        </>
        <div className="container">
            <div className="row mb-2">
                <div className="col-4">
                    <label htmlFor="domainOfferMaxContractsPerIdentity" className="form-label">Max contracts per identity</label>
                    <input type="text" id="domainOfferValidFrom" className="form-control" value={offerData.maxContractsPerIdentity} onChange={async (event) => setOfferData({...offerData, maxContractsPerIdentity: parseInt(event.target.value)})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidFrom" className="form-label">Valid from</label>
                    <input type="date" id="domainOfferValidFrom" className="form-control" value={offerData.validFrom.substring(0,10)} onChange={async (event) => setOfferData({...offerData, validFrom: event.target.value})}></input>                
                </div>
                <div className="col-4">
                    <label htmlFor="domainOfferValidTo" className="form-label">Valid to</label>
                    <input type="date" id="domainOfferValidTo" className="form-control" value={offerData.validTo.substring(0,10)} onChange={async (event) => setOfferData({...offerData, validTo: event.target.value})}></input>                
                </div>
            </div>
            <div className="row mb-3">
                <label  htmlFor="domainOfferDescription" className="form-label">Description</label>
                <textarea id="domainOfferDescription" className="form-control" value={offerData.description} onChange={async (event) => setOfferData({...offerData, description: event.target.value})}></textarea>               
            </div>
            <div className="row mb-2">
                <div className="col-12">
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} title="Save" onClick={async () => await putDomainOffer(offerData)}><i className="bi bi-save"></i></button>
                    <button className="btn me-2" style={{backgroundColor: '#59a8fc', color: 'white'}} data-bs-toggle="tooltip" data-bs-placement="top" title="Terminate" onClick={async () => await putDomainOffer({...offerData, terminate: true})} disabled={new Date(offerData.validTo) < new Date()}><i className="bi bi-x-lg"></i></button>
                </div>      
            </div>
        </div>
    </CollapsedListElement>
}