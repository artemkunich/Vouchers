import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { DomainMembersListFilter } from './domainMembersListFilter.tsx'
import { DomainMembersListElement } from './domainMembersListElement.js'
import { FetchingListItemsInfo } from '../components/fetchingListItemsInfo/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../api/helpers.ts'
import { api } from '../api/api.ts'

export const DomainMembersList = ({openDetail, handleCancel}) => {
    
    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [domainMembersFilter, setDomainMembersFilter] = useState({});
    const [domainAccounts, setDomainAccounts] = useState([]);

    const getDomainAccounts = useCallback(async (query) => await api.getDomainAccounts(token, {...query, domainId: currentAccount.domainId}), [api, token, currentAccount])
    const {isFetching, resetPageIndex} = useLoadOnScroll(domainMembersFilter, getDomainAccounts, setDomainAccounts)

    const putDomainAccount = useCallback(async (domainAccount) => {
        await api.putDomainAccount(token, domainAccount)
        resetPageIndex()
    }, [api, token])

    const handleSearch = useCallback((filter) => {
        setDomainMembersFilter({...domainMembersFilter, ...filter})
        resetPageIndex()
    }, [])

    const openDomainAccountDetail = (domainAccount) => {
        openDetail(domainAccount)
    }

    return <>              
        <DomainMembersListFilter initFilter={domainMembersFilter} handleSearch={handleSearch} handleCancel={handleCancel}/>
        {
            domainAccounts.map((domainAccount, i) => <DomainMembersListElement key={domainAccount.id} currentAccount={currentAccount} domainAccount={domainAccount} putDomainAccount={putDomainAccount} openDomainAccountDetail={openDomainAccountDetail}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>  
}