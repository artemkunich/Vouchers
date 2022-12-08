import React, { useState } from 'react'
import { getImageSrc } from '../imageSources.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const TransactionRequestDetailForm = ({transactionRequest, currentAccount, handleSelectCreditor, handlePerform, saveTransactionRequest, handleCancel}) => {

    const [formData, setFormData] = useState({
        mustBeExchangeable: transactionRequest.mustBeExchangeable,
        amount: transactionRequest.amount,
        dueDate: transactionRequest.dueDate,
        maxDaysBeforeValidityStart: transactionRequest.maxDaysBeforeValidityStart,
        minDaysBeforeValidityEnd: transactionRequest.minDaysBeforeValidityEnd,
        message: transactionRequest.message,
    })

    return <>
        <div className="row mb-2">
            {transactionRequest.id && <h3>Request id: ${transactionRequest.id}</h3>}
        </div>
        <div className="row mb-2">
            <div className="col-2 align-middle">
                {transactionRequest.unitImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequest.unitImageId)}></img> }
                <p className='mb-0'>Ticker: {transactionRequest.unitTicker}</p>
                <p>Issuer: {transactionRequest.unitIssuerName} {transactionRequest.unitIssuerEmail && `(${transactionRequest.unitIssuerEmail})`}</p>
                <div className="align-middle form-check">
                    <label htmlFor="mustBeExchangeable" className="form-check-label">Must be exchangeable</label>
                    <input type="checkbox" id="mustBeExchangeable" className="form-check-input" checked={formData.mustBeExchangeable} 
                        onChange={() => setFormData({...formData, mustBeExchangeable: !formData.mustBeExchangeable})} 
                        disabled={transactionRequest.id || transactionRequest.unitIssuerAccountId == currentAccount.id}>
                    </input>
                </div>
            </div>
            <div className="col-2 align-middle">
                { transactionRequest.counterpartyImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequest.counterpartyImageId)}></img> }
                <p>Counterparty: {transactionRequest.counterpartyName} {transactionRequest.counterpartyEmail && `(${transactionRequest.counterpartyEmail})`}</p>
                
                {
                    !transactionRequest.id &&
                    <div className="d-grid" > 
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={() => handleSelectCreditor(true, formData)}>Select creditor</button>
                    </div>
                }
            </div>
        </div>

        <div className="row mb-2">
            <div className="col-3">
                <label htmlFor="amount" className="form-label">Amount</label>
                <input type="number" id="amount" className="form-control" value={formData.amount} onChange={(event) => setFormData({...formData, amount: event.target.value})} disabled={transactionRequest.id}></input>                               
            </div> 
            <div className="col-3">
                <label htmlFor="dueDate" className="form-label">Due date</label>
                <input type="date" id="dueDate" className="form-control" value={formData.dueDate} onChange={(event) => setFormData({...formData, dueDate: event.target.value})} disabled={transactionRequest.id}></input>               
            </div>
            <div className="col-3">
                <label htmlFor="maxDaysBeforeValidityStart" className="form-label">Max days before validity start</label>
                <input type="text" id="maxDaysBeforeValidityStart" className="form-control" 
                    value={formData.maxDaysBeforeValidityStart} 
                    onChange={(event) => setFormData({...formData, maxDaysBeforeValidityStart: event.target.value})}
                    disabled={transactionRequest.id || transactionRequest.unitIssuerAccountId == currentAccount.id}>
                </input>                
            </div>
            <div className="col-3">
                <label htmlFor="minDaysBeforeValidityEnd" className="form-label">Min days before validity end</label>
                <input type="text" id="minDaysBeforeValidityEnd" className="form-control" 
                    value={formData.minDaysBeforeValidityEnd}
                    onChange={(event) => setFormData({...formData, minDaysBeforeValidityEnd: event.target.value})}
                    disabled={transactionRequest.id ||transactionRequest.unitIssuerAccountId == currentAccount.id}></input>                
            </div> 
        </div>
        <div className="row mb-2">
            <div className="col-12">
                <label  htmlFor="message" className="form-label">Message</label>
                <textarea id="message" className="form-control" rows="auto" maxLength="1024" value={formData.message} onChange={(event) => setFormData({ ...formData, message: event.target.value })} disabled={transactionRequest.id}></textarea>  
            </div>
        </div>
        <div className="row mb-2">
            {
                !transactionRequest.transactionId && transactionRequest.debtorAccountId != currentAccount.id && 
                <div className="col-1 d-grid mb-2" > 
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} title={"Perform"} onClick={() => handlePerform({...transactionRequest, ...formData})}>
                        Perform
                    </button>
                </div>
            }
            {
                !transactionRequest.id ?
                <div className="col-1 d-grid mb-2" > 
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await saveTransactionRequest(formData)}>Save</button>
                </div> : <></>
            }
            <div className="col-1 d-grid mb-2" > 
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => handleCancel()}>Cancel</button>
            </div>
        </div>
    </>
}