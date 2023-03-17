import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { FetchingListItemsInfo } from '../common/fetchingListItemsInfo.tsx'
import { TransactionRequestsListFilter } from './transactionRequestsListFilter.js'
import { OutgoingTransactionRequestsListElement } from './outgoingTransactionRequestsListElement.js'
import { useLoadOnScroll } from '../../api/helpers.ts'
import { api } from '../../api/api.ts'


export const OutgoingTransactionRequestsList = ({selectTransactionRequest}) => {
    
    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionRequestsFilter, setTransactionRequestsFilter] = useState({
        pageIndex: 0,
        pageSize: 10,
        accountId: currentAccount.id,
        includeIncoming: false,
        includeOutgoing: true,
        includePerformed: false,
        includeNotPerformed: true
    });
    const [transactionRequests, setTransactionRequests] = useState([]);

    const getTransactionRequests = useCallback(async (query) => await api.getTransactionRequests(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(transactionRequestsFilter, getTransactionRequests, setTransactionRequests)

    const deleteTransactionRequest = useCallback(async (transactionRequestId) => {
        await api.deleteTransactionRequest(token, transactionRequestId)
        resetPageIndex()
    }, [api, token, transactionRequestsFilter])

    const handleSearch = useCallback((filter) => {
        setTransactionRequestsFilter({...transactionRequestsFilter, ...filter})
        resetPageIndex()
    }, [])

    return <>
        <TransactionRequestsListFilter handleSearch={handleSearch}/>
        {
            transactionRequests.map((transactionRequest, i) => <OutgoingTransactionRequestsListElement key={transactionRequest.id} transactionRequest={transactionRequest} selectTransactionRequest={selectTransactionRequest} deleteTransactionRequest={deleteTransactionRequest}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>
}