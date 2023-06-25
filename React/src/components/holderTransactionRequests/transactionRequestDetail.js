import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { TransactionRequestDetailForm } from './transactionRequestDetailForm.js'
import { DomainAccountsList } from '../domainAccounts/list.tsx'
import { api } from '../../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const TransactionRequestDetail = ({transactionRequest, handlePerform, handleCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [isSelectCreditorState, setIsSelectCreditorState] = useState(false)

    const [transactionRequestDetail, setTransactionRequestDetail] = useState({
        ...transactionRequest,
        dueDate: transactionRequest.dueDate?.substring(0,10)
    })

    useEffect(() => {
        if(!transactionRequest.debtorAccountId) {
            transactionRequestDetail.debtorAccountId = currentAccount.id
        }
    }, [token, currentAccount])

    const setSelectedCreditor = useCallback((creditorAccount) => {
        setIsSelectCreditorState(false)

        setTransactionRequestDetail({
            ...transactionRequestDetail,
            creditorAccountId: creditorAccount?.id,
            counterpartyName: creditorAccount?.name,
            counterpartyEmail: creditorAccount?.email,
            counterpartyImageId: creditorAccount?.imageId           
        })
    }, [transactionRequestDetail])

    const setSelectCreditorState = (state, formData) => {
        setTransactionRequestDetail({...transactionRequestDetail, ...formData})
        setIsSelectCreditorState(state)
    }

    const saveTransactionRequest = useCallback(async (formData) => {
        setTransactionRequestDetail({...transactionRequestDetail, ...formData})

        const request = transactionRequestDetail
        const transactionRequestId = await api.postTransactionRequest(token, request)
        if(transactionRequestId) {
            setTransactionRequestDetail({
                ...transactionRequestDetail,
                id: transactionRequestId
            })
        }
    }, [token, api, transactionRequestDetail])

    return <> {
        isSelectCreditorState ?
            <DomainAccountsList selectDomainAccount={setSelectedCreditor} handleCancel={() => setIsSelectCreditorState(false)}/> 
        :
        <TransactionRequestDetailForm
            transactionRequest = {transactionRequestDetail}
            currentAccount = {currentAccount}
            handleSelectCreditor = {setSelectCreditorState}
            handlePerform = {handlePerform}
            saveTransactionRequest = {saveTransactionRequest}
            handleCancel = {handleCancel}
        />
    }
    </>
}