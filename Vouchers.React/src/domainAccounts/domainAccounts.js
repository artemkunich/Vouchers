import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css";

const defaultFilter = {
    ticker: "",
    issuerName: "", 
}

export const DomainAccounts = ({header, setSelectedDomainAccount, onCancel, api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [filter, setFilter] = useState(defaultFilter)

    const [domainAccounts, setDomainAccounts] = useState([]);

    useEffect(() => { 
        setFilter(defaultFilter)
        setDomainAccounts([])
    }, [currentDomainAccount, token])

    const loadDomainAccountsAsync = useCallback(async (query) => {
        let result = await api.getDomainAccounts(token, {...query, domainId: currentDomainAccount.domainId})  
        
        console.log(api)
        
        if(!result)
            result = []
        
        setDomainAccounts(result)
    }, [api, token, currentDomainAccount])

    return <>
        {
            header && <h2 className="header mt-2 mb-3">{header}</h2>
        }

        <ListElement>
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
                <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={async () => loadDomainAccountsAsync(filter)}>Search</button>
                {
                    onCancel && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} onClick={()=>onCancel()}>Cancel</button>
                }
            </div> 
        </ListElement>     
        {              
            domainAccounts.map((domainAccount, i) => <DomainAccount key={domainAccount.id} domainAccount={domainAccount} setSelectedDomainAccount={setSelectedDomainAccount}/>)
        }
    </>  
}

const DomainAccount = ({domainAccount, setSelectedDomainAccount}) => {

    return <ListElement>
        <div className="container">
            <a onClick={() => setSelectedDomainAccount(domainAccount)}>
                <div className="row mb-2">
                    <div className="col-2 align-middle">
                        { domainAccount.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainAccount.imageId)}></img> }               
                    </div>            
                    <div className="col-4 align-middle">
                        <p className="fs-5">{domainAccount.email}</p>
                        <p className="fs-5">{domainAccount.name}</p>
                    </div>
                    <div className="col-4 align-middle"/> 
                    <div className="col-2 align-middle"/>    
                </div>
            </a> 
        </div>
    </ListElement>
}