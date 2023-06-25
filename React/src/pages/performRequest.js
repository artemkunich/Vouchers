import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { PerformRequestForm } from '../components/holderTransactionRequests/performRequestForm.js'
import { Header } from '../components/common/header.tsx'
import { HolderTransactionDetailContainer } from '../components/holderTransactionDetail/container.js'
import { TransactionRequestDetail } from '../components/holderTransactionRequests/transactionRequestDetail.js'
import { api } from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";


export const PagesPerformRequest = () => {
    
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
    }, [])

    const loadTransactionRequestAsync = useCallback(async (transactionRequestId) => {
        const result = await api.getTransactionRequest(token, transactionRequestId)
        if(result){
            setSelectedTransactionRequestDetail(result)
            return
        }
            
        alert("Transaction request was not found")
    }, [token, api])

    return <>
        <Header>Perform transaction request</Header>
        {
            selectedTransactionRequestDetail ? 
            <TransactionRequestDetail transactionRequest={selectedTransactionRequestDetail} handlePerform={handlePerform} handleCancel={handleCancel}/>
            : selectedTransactionRequest2Perform ? 
            <HolderTransactionDetailContainer transaction={{
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
            :
            <PerformRequestForm loadTransactionRequestAsync={loadTransactionRequestAsync}/>  
        }

    </>
}