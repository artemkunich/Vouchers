import React, { useState, useEffect, useRef, useCallback, useMemo} from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { ListElement, CollapsedListElement} from '../components/list/list.tsx'
import { FetchingListItemsInfo } from '../components/fetchingListItemsInfo/fetchingListItemsInfo.tsx'
import { getImageSrc } from '../imageSources.ts'
import { useLoadOnScroll } from '../api/helpers.ts'
import { api } from '../api/api.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const IssuerValuesList = ({openValueDetail, openVouchers}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [values, setValues] = useState([]);
    const [valuesFilter, setValuesFilter] = useState({
        ticker: "",
    });

    const getValues = useCallback(async (query) => await api.getValues(token, {...query, issuerAccountId: currentDomainAccount.id}), [api, token])
    const {isFetching, resetPageIndex} = useLoadOnScroll(valuesFilter, getValues, setValues)

    const handleSearch = useCallback((filter) => {
        setValuesFilter({
            ...valuesFilter,
            ...filter
        })
        resetPageIndex()
    })

    return (
        <>           
            <IssuerValuesListFilter handleSearch={handleSearch}/>     
            {          
                values.map((value, i) => <IssuerValuesListElement key={value.id} valuesItem={value} openValueDetail={openValueDetail} openVouchers={openVouchers}/>)
            }
            <FetchingListItemsInfo isFetching={isFetching}/>
        </>
    )
}

const IssuerValuesListFilter = ({handleSearch}) => {
    const [filter, setFilter] = useState({
        ticker: "",
    });

    return (
        <ListElement>
            <div className="col-3 align-middle">
                <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                <input type="text" id="valueTickerFilter" className="form-control" value={filter.ticker} onChange={(event) => setFilter({...filter, ticker: event.target.value})}></input> 
            </div>  
            <div className="col-1 mb-2" > 
                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => handleSearch(filter)}>Search</button>
            </div> 
        </ListElement>
    )
}

const IssuerValuesListElement = ({valuesItem, openValueDetail, openVouchers}) => {

    return <ListElement>
        <div className="col-2 align-middle">
            { valuesItem.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(valuesItem.imageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">{valuesItem.ticker}</p>
            <p>{valuesItem.description}</p>
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">Supply: {valuesItem.supply}</p>
        </div> 
        <div className=" col-2">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => openValueDetail(valuesItem)}>
                Detail
            </button>
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Vouchers"} onClick={() => openVouchers(valuesItem)}>
                Vouchers
            </button>
        </div>       
    </ListElement>
}