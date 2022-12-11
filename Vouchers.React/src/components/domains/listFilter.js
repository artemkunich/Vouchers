import React, { useState } from 'react'
import { ListElement } from '../common/list/list.tsx'

export const DomainsListFilter = ({handleSearch, handleCancel}) => {

    const [filter, setFilter] = useState({
        name: "",
        ownerName: ""
    });

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="nameFilter" className="form-label">Name</label>
            <input type="text" id="nameFilter" className="form-control" value={filter.name} onChange={(event) => setFilter({...filter, name: event.target.value})}></input>
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="ownerNameFilter" className="form-label">Owner name</label>
            <input type="text" id="ownerNameFilter" className="form-control" value={filter.ownerName} onChange={(event) => setFilter({...filter, ownerName: event.target.value})}></input>
        </div>
        <div className="col-4 align-middle"/>
        <div className="col-2 align-middle" >
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={handleSearch}>Search</button>
            {
                handleCancel && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={()=>handleCancel()}>Cancel</button>
            }
        </div>
    </ListElement>
}