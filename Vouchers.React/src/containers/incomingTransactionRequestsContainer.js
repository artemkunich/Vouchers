import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { Header } from '../components/header/header.tsx'
import { TransactionRequestDetail } from '../holderTransactionRequests/transactionRequestDetail.js'
import { HolderTransactionDetail } from '../holderTransactionDetail/holderTransactionDetail.js'
import { IncomingTransactionRequestsList } from '../holderTransactionRequests/incomingTransactionRequestsList.js'


export const IncomingTransactionRequestsContainer = () => {
    
    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [selectedTransactionRequestDetail, setSelectedTransactionRequestDetail] = useState();
    const [selectedTransactionRequest2Perform, setSelectedTransactionRequest2Perform] = useState();

    const handlePerform = useCallback((request) => {
        setSelectedTransactionRequestDetail()
        setSelectedTransactionRequest2Perform(request)
    })

    const handleCancel = useCallback(() => {
        setSelectedTransactionRequestDetail()
        setSelectedTransactionRequest2Perform()
    }, [])

    return <>
        <Header>Incoming transaction requests</Header>
        <div style={{display: (selectedTransactionRequestDetail || selectedTransactionRequest2Perform) ? "none" : "block"}}>
            <IncomingTransactionRequestsList selectTransactionRequestDetail={setSelectedTransactionRequestDetail} selectTransactionRequest2Perform={setSelectedTransactionRequest2Perform}/>
        </div>
        {         
            selectedTransactionRequestDetail ? 
            <TransactionRequestDetail transactionRequest={selectedTransactionRequestDetail} handlePerform={handlePerform} handleCancel={handleCancel}/>
            : 
            selectedTransactionRequest2Perform ? 
            <HolderTransactionDetail transaction={{
                creditorAccountId: selectedTransactionRequest2Perform?.creditorAccountId ?? currentAccount.id,
                debtorAccountId: selectedTransactionRequest2Perform.debtorAccountId,
                debtorName: selectedTransactionRequest2Perform.counterpartyName,
                debtorEmail: selectedTransactionRequest2Perform.counterpartyEmail,
                quantity: 0,
                unitTypeId: selectedTransactionRequest2Perform.unitTypeId,
                unitIssuerAccountId: selectedTransactionRequest2Perform.unitIssuerAccountId,
                unitTicker: selectedTransactionRequest2Perform.unitTicker,
                items: [],
                message: ""
            }} transactionRequest={selectedTransactionRequest2Perform} handleCancel={handleCancel}/>
            : <></>
            
        }
    </>
}