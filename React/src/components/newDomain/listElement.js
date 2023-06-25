import React, {useState, useEffect, useRef, useCallback } from 'react'
import {ListElement} from '../common/list/list.tsx'

export const NewDomainOffersListElement = ({offersItem, postDomainConract}) => {

    const [domainName, setDomainName] = useState()

    return <ListElement>
        <div className="col-12 align-middle"><p className="fs-5">{offersItem.name}</p></div>
        <div className="container">
            <div className="row mb-2">
                <div className="col-3 align-middle">
                    <p className="my-0">Valid to: {offersItem.validTo.substring(0,10)}</p>
                    <p className="my-0">Price: {offersItem.amount} {offersItem.currency}/{offersItem.invoicePeriod}</p>
                    <p className="my-0">Max members: {offersItem.maxMembersCount}</p>
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