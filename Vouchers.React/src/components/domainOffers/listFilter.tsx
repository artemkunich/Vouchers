import * as React from 'react'
import { ListElement } from '../common/list/list'

interface DomainOffersListFilterProps {
    handleSearch: (filterData: DomainOffersListFilterData) => void,
    handleCancel: VoidFunction
}

interface DomainOffersListFilterData {
    name: string,
    includeExpired: boolean,
    includePlanned: boolean,
}

export const DomainOffersListFilter = ({handleSearch, handleCancel}: DomainOffersListFilterProps) => {

    const [filter, setFilter] = React.useState<DomainOffersListFilterData>({
        name: "",
        includeExpired: false,
        includePlanned: false,
    });

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
            <input type="text" id="domainOfferNameFilter" className="form-control" value={filter.name} onChange={(event) => setFilter({...filter, name: event.target.value})}></input> 
        </div>  
        <div className="col-3 align-middle form-check">
            <div>
                <label htmlFor="domainIncludePlannedFilter" className="form-check-label">Include planned</label>
                <input type="checkbox" id="domainIncludePlannedFilter" className="form-check-input" defaultChecked={filter.includePlanned} onClick={() => setFilter({...filter, includePlanned: !filter.includePlanned})}></input> 
            </div>
            <div>
                <label htmlFor="domainIncludeExpiredFilter" className="form-check-label">Include expired</label>
                <input type="checkbox" id="domainIncludeExpiredFilter" className="form-check-input" defaultChecked={filter.includeExpired} onClick={() => setFilter({...filter, includeExpired: !filter.includeExpired})}></input> 
            </div>              
        </div>
        <div className="col-6 align-middle" >
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={() => handleSearch(filter)}>Search</button>
            {
                handleCancel && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={()=>handleCancel()}>Cancel</button>
            }
        </div>        
    </ListElement>
}