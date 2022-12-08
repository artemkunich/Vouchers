import React from 'react'
import {ListElement} from '../components/list/list.tsx'
import {getImageSrc} from '../imageSources.ts'

export const OutgoingTransactionRequestsListElement = ({transactionRequest, selectTransactionRequest, deleteTransactionRequest}) => {
    return <ListElement>
        <div className="col-1 align-middle">
            { transactionRequest.unitImageId && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(transactionRequest.unitImageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="mb-0">Ticker: {transactionRequest.unitTicker}</p>
            <p className='mb-0'>Issuer: {transactionRequest.unitIssuerName} ({transactionRequest.unitIssuerEmail})</p>
        </div> 
        <div className="col-1 align-middle">
            { transactionRequest.counterpartyImageId && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(transactionRequest.counterpartyImageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="mb-0">{transactionRequest.dueDate.substring(0,10).replaceAll('T', ' ')}</p>
            {
                transactionRequest.counterpartyImageId && <p className="mb-0">Counterparty: {`${transactionRequest.counterpartyName} (${transactionRequest.counterpartyEmail})`}</p>
            }
            <p className='mb-0'>{`Amount: `} 
                <b style={{color: "#4CBB17"}}>
                    {transactionRequest.amount}
                </b>
            </p>
        </div>
        <div className=" col-1"/> 
        <div className=" col-1">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, padding: 2, width: 90}} title={"Perform"} onClick={() => selectTransactionRequest(transactionRequest)}>
                Detail
            </button>
            {
                !transactionRequest.transactionId &&
                <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, padding: 2, width: 90}} title={"Perform"} onClick={() => deleteTransactionRequest(transactionRequest.id)}>
                    Remove
                </button>
            }
        </div>

    </ListElement>
}