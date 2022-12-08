import React, { useState } from 'react'
import { ListElement } from '../components/list/list.tsx'
import { getImageSrc } from '../imageSources.ts'

export const DomainValuesListElement = ({valueItem, selectValue4Detail, selectValue4Request}) => {

    const [value, setValue] = useState(valueItem)

    return <ListElement>
        <div className="col-2 align-middle">
            { value.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(value.imageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{value.ticker}</p>
            <p className='mb-0'>{value.description}</p>
            <p className='mb-0'>Issuer: {value.issuerName} ({value.issuerEmail})</p>
        </div>
        <div className="col-2 align-middle">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => selectValue4Detail(value)}>
                Detail
            </button>
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => selectValue4Request(value)}>
                New request
            </button>
        </div>
    </ListElement>
}