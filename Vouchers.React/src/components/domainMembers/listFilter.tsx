import * as React from 'react'
import { ListElement } from '../common/list/list'
import { DomainAccountsQuery } from '../../types/queries'

interface DomainMembersListFilterProps {
    handleSearch: (filter: DomainMembersListFilterData) => void,
    handleCancel: VoidFunction,
}

export interface DomainMembersListFilterData {
    email: string,
    name: string,
    includeConfirmed: boolean,
    includeNotConfirmed: boolean,
}

export const DomainMembersListFilter = ({handleSearch, handleCancel}: DomainMembersListFilterProps) => {

    const [filter, setFilter] = React.useState<DomainMembersListFilterData>({
        email: "",
        name: "",
        includeConfirmed: true,
        includeNotConfirmed: false,
    })

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="domainOfferNameFilter" className="form-label">Email</label>
            <input type="text" id="domainOfferNameFilter" className="form-control" value={filter.email} onChange={(event) => setFilter({...filter, email: event.target.value})}></input> 
        </div>
        <div className="col-3 align-middle">
            <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
            <input type="text" id="domainOfferNameFilter" className="form-control" value={filter.name} onChange={(event) => setFilter({...filter, name: event.target.value})}></input> 
        </div>
        <div className="col-4 align-middle form-check">
            <label htmlFor={"showRequestsFilter"} className="form-check-label">Get requests</label>
            <input type="checkbox" id={"showRequestsFilter"} className="form-check-input" checked={filter.includeNotConfirmed} onChange={() => setFilter({...filter, includeConfirmed: !filter.includeConfirmed, includeNotConfirmed: !filter.includeNotConfirmed})}></input> 
        </div>
        <div className="col-2 mb-2" >
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={() => handleSearch(filter)}>Search</button>
            {
                handleCancel && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={()=>handleCancel()}>Cancel</button>
            }
        </div>
    </ListElement>
}