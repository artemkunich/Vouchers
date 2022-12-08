import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { DomainValuesListFilter } from './domainValuesListFilter.js'
import { DomainValuesListElement } from './domainValuesListElement.js'
import { FetchingListItemsInfo } from '../components/fetchingListItemsInfo/fetchingListItemsInfo.tsx'
import { useLoadOnScroll } from '../api/helpers.ts'
import { api } from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const DomainValuesList = ({selectValue4Detail, selectValue4Request}) => {

    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [valuesFilter, setValuesFilter] = useState({});
    const [values, setValues] = useState([])

    const getValues = useCallback(async (query) => await api.getDomainValues(token, {...query, domainId: currentAccount.domainId}), [api, token, currentAccount])
    const {isFetching, resetPageIndex} = useLoadOnScroll(valuesFilter, getValues, setValues)

    const handleSearch = useCallback((filter) => {
        setValuesFilter({...valuesFilter, ...filter})
        resetPageIndex()
    }, [])

    return <>
        <DomainValuesListFilter handleSearch={handleSearch}/>
        {
            values.map((value, i) => <DomainValuesListElement key={value.id} valueItem={value} selectValue4Detail={selectValue4Detail} selectValue4Request={selectValue4Request}/>)
        }
        <FetchingListItemsInfo isFetching={isFetching}/>
    </>

}