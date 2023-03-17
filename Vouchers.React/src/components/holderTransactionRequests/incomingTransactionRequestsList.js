import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { FetchingListItemsInfo } from '../common/fetchingListItemsInfo.tsx'
import { TransactionRequestsListFilter } from './transactionRequestsListFilter.js'
import { IncomingTransactionRequestsListElement } from './incomingTransactionRequestsListElement.js'
import { useLoadOnScroll } from '../../api/helpers.ts'
import { api } from '../../api/api.ts'


export const IncomingTransactionRequestsList = ({selectTransactionRequestDetail, selectTransactionRequest2Perform, handleCancel}) => {
    
    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionRequestsFilter, setTransactionRequestsFilter] = useState({
        accountId: currentAccount.id,
        includeIncoming: true,
        includeOutgoing: false,
        includePerformed: false,
        includeNotPerformed: true,
        pageSize: 10
    });
    const [transactionRequests, setTransactionRequests] = useState([]);

    const getTransactionRequests = useCallback(async (query) => await api.getTransactionRequests(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(transactionRequestsFilter, getTransactionRequests, setTransactionRequests)

    const handleSearch = useCallback((filter) => {
        setTransactionRequestsFilter({...transactionRequestsFilter, ...filter})
        resetPageIndex()
    }, [])

    return <>
        <TransactionRequestsListFilter handleSearch={handleSearch}/>
        {
            transactionRequests.map((transactionRequest, i) => <IncomingTransactionRequestsListElement key={transactionRequest.id} transactionRequest={transactionRequest} selectTransactionRequestDetail={selectTransactionRequestDetail} selectTransactionRequest2Perform={selectTransactionRequest2Perform}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>
}