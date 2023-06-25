import React, {useState, useCallback } from 'react'
import { useSelector, useDispatch} from 'react-redux'
import { FetchingListItemsInfo} from '../common/fetchingListItemsInfo.tsx'
import { HolderTransactionsListFilter } from './listFilter.js'
import { HolderTransactionsListElement } from './listElement.js'
import { useLoadOnScroll} from '../../api/helpers.ts'
import { api } from '../../api/api.ts'

export const HolderTransactionsList = ({openTransactionDetail}) => {
    
    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionsFilter, setTransactionsFilter] = useState({
        pageIndex: 0,
        pageSize: 10,
        accountId: currentAccount.id
    });
    const [transactions, setTransactions] = useState([]);

    const getHolderTransactions = useCallback(async (query) => await api.getHolderTransactions(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(transactionsFilter, getHolderTransactions, setTransactions)

    const handleSearch = useCallback((filter) => {
        setTransactionsFilter({...transactionsFilter, ...filter})
        resetPageIndex()
    }, [])


    return <>
        <HolderTransactionsListFilter handleSearch={handleSearch}/>
        {
            transactions.map((transaction, i) => <HolderTransactionsListElement key={transaction.id} transaction={transaction} currentAccount={currentAccount} openTransactionDetail={openTransactionDetail} />)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>
}