import { useSelector, useDispatch } from 'react-redux'
import React, { useState, useCallback, useEffect } from 'react';
import { DomainsListFilter } from './domainsListFilter.js'
import { DomainsListElement } from './domainsListElement.js'
import { FetchingListItemsInfo } from '../components/fetchingListItemsInfo/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../api/helpers.ts'
import { api } from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainsList = ({handleCancel}) => {
    
    const token = useSelector(state => state.user.token)

    const [domains, setDomains] = useState([]);
    const [domainsFilter, setDomainsFilter] = useState({});
    
    const getDomains = useCallback(async (query) => await api.getDomains(token, query), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(domainsFilter, getDomains, setDomains)

    const handleSearch = useCallback((filter) => {
        setDomainsFilter({...domainsFilter, ...filter})
        resetPageIndex()
    }, [])

    return (
        <>
            <DomainsListFilter handleSearch={handleSearch} handleCancel={handleCancel}/>   
            {              
                domains.map((domain, i) => <DomainsListElement key={domain.id} domainsItem={domain} postSubscriptionRequest={async (domainId) => await api.postDomainAccount(token, {domainId: domainId})}/>)
            }
            <FetchingListItemsInfo isFetching={isFetching}/>
        </>
    );
}