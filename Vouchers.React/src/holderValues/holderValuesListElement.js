import React, { useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { ListElement } from '../components/list/list.tsx'
import { api } from '../api/api.ts'
import { getImageSrc } from '../imageSources.ts'
import "bootstrap-icons/font/bootstrap-icons.css";

export const HolderValuesListElement = ({valueItem, handleNewTransaction}) => {

    const token = useSelector(state => state.user.token)

    const [value, setValue] = useState(valueItem)
    const [vouchers, setVouchers] = useState([])

    const loadHolderVouchers = async (query) => {

        let result = await api.getHolderVouchers(token, query)  
        if(!result)
            result = []

        setVouchers(result)
    }

    useEffect(() => {
        const effectAsync = async () => await loadHolderVouchers({valueId: value.id})

        effectAsync()
    },[])

    return <ListElement>

        <div className="col-2 align-middle">
            { value.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(value.imageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">{value.ticker}</p>
            <p className='mb-0'>{value.description}</p>
            <p className='mb-0'>Issuer: {value.issuerName} ({value.issuerEmail})</p>
        </div>
        <div className="col-4 align-middle">
            <p className="fs-5">Balance: {value.balance}</p>
        </div> 
        <div className=" col-2">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"New transfer"} onClick={() => handleNewTransaction(value)}>
                New transfer
            </button>
        </div>

    </ListElement>
}