import * as React from 'react'
import { ListElement } from '../components/list/list'

interface DomainAccountsListFilterProps {
    handleSearch: (filter: Filter) => void,
    handleCancel: () => {}
}

export interface Filter {
    email: string,
    name: string,
}

export const DomainAccountsListFilter = ({handleSearch, handleCancel} : DomainAccountsListFilterProps) => {

    const [filter, setFilter] = React.useState<Filter>({
        email: "",
        name: "",
    })

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="emailFilter" className="form-label">Email</label>
            <input type="text" id="emailFilter" className="form-control" value={filter.email} onChange={(event) => setFilter({...filter, email: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="nameFilter" className="form-label">Name</label>
            <input type="text" id="nameFilter" className="form-control" value={filter.name} onChange={(event) => setFilter({...filter, name: event.target.value})}></input> 
        </div>
        <div className="col-4 align-middle"/>
        <div className="col-2 align-middle" >
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={() => handleSearch(filter)}>Search</button>
            {
                handleCancel && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={()=>handleCancel()}>Cancel</button>
            }
        </div>
    </ListElement>
}