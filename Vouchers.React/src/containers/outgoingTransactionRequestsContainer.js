import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { Header } from '../components/header/header.tsx'
import { TransactionRequestDetail } from '../holderTransactionRequests/transactionRequestDetail.js'
import { OutgoingTransactionRequestsList } from '../holderTransactionRequests/outgoingTransactionRequestsList.js'



export const OutgoingTransactionRequestsContainer = () => {
    
    const [selectedTransactionRequest, setSelectedTransactionRequest] = useState();

    const handleCancel = useCallback(() => {
        setSelectedTransactionRequest()
    }, [])

    return <>
        <Header>Outgoing transaction requests</Header>
        <div style={{display: selectedTransactionRequest ? "none" : "block"}}>
            <OutgoingTransactionRequestsList selectTransactionRequest={setSelectedTransactionRequest}/>
        </div>
        {
            selectedTransactionRequest ? 
            <TransactionRequestDetail transactionRequest={selectedTransactionRequest} handleCancel={handleCancel}/>
            : 
            <></>
        }

    </>
}