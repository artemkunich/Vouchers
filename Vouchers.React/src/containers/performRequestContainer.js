import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { PerformRequestForm } from '../holderTransactionRequests/performRequestForm.js'
import { Header } from '../components/header/header.tsx'
import { TransactionRequestDetail } from '../holderTransactionRequests/transactionRequestDetail.js'
import { api } from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";


export const PerformRequestContainer = () => {
    
    const token = useSelector(state => state.user.token)

    const [selectedTransactionRequestDetail, setSelectedTransactionRequestDetail] = useState();

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
            <TransactionRequestDetail transactionRequest={selectedTransactionRequestDetail} handleCancel={handleCancel}/>
            :
            <PerformRequestForm loadTransactionRequestAsync={loadTransactionRequestAsync}/>  
        }

    </>
}