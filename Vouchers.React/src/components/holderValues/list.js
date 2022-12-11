import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { HolderValuesListFilter } from './listFilter.js'
import { HolderValuesListElement } from './listElement.js'
import { FetchingListItemsInfo } from '../common/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../../api/helpers.ts'
import { api } from '../../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const HolderValuesList = ({handleNewTransaction, handleCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [valuesFilter, setValuesFilter] = useState({});
    const [values, setValues] = useState([]);

    const getHolderValues = useCallback(async (query) => await api.getHolderValues(token, {...query, holderId: currentDomainAccount.id}), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(valuesFilter, getHolderValues, setValues)
    
    const handleSearch = useCallback((filter) => {
        setValuesFilter({...valuesFilter, ...filter})
        resetPageIndex()
    }, [])

    return (
        <>
            <HolderValuesListFilter handleSearch={handleSearch}/>
            {
                values.map((value, i) => <HolderValuesListElement key={`${value.id} ${value.balance}`} valueItem={value} handleNewTransaction={handleNewTransaction}/>)
            }
            <FetchingListItemsInfo isFetching={isFetching}/>
        </>
    );
}

