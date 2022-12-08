import React, { useState } from 'react'
import { ListHeader } from '../components/list/list.tsx'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainOffersListNewElement = ({postDomainOffer}) => {

    const [newOffer, setNewOffer] = useState({
        name: "",
        maxMembersCount: 0,
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
                    <label htmlFor="domainOfferMaxMembersCount" className="form-label">Max count of subscribers</label>
                    <input type="text" id="domainOfferMaxMembersCount" className="form-control" value={newOffer.maxMembersCount} onChange={async (event) => setNewOffer({...newOffer, maxMembersCount: Number(event.target.value)})}></input>                
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