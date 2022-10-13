import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import {CollapsedListElement, ListElement} from '../list/listElement.js'
import {getImageSrc} from '../imageSources.js'
import {Profile} from '../profile/profile.js'
import "bootstrap-icons/font/bootstrap-icons.css";

const domainAccountActions = {
    openDetail: "openDetail"
}

export const DomainMembers = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [domainAccountsFilter, setDomainAccountsFilter] = useState({
        email: "",
        name: "",
        includeConfirmed: true,
        includeNotConfirmed: false,
    });
    const [domainAccounts, setDomainAccounts] = useState([]);
    const [selectedDomainAccount, setSelectedDomainAccount] = useState();

    const openDomainAccountDetail = useCallback((domainAccount) => {
        setSelectedDomainAccount({
            domainAccount,
            action: domainAccountActions.openDetail
        })
    }, [])

    const onCancel = useCallback(() => {
        setSelectedDomainAccount()
    }, [])

    const getDomainAccountsAsync = useCallback(async (query) => {
        let result = await api.getDomainAccounts(token, {...query, domainId: currentDomainAccount.domainId})  
        if(!result)
            result = []
        
        setDomainAccounts(result)
    }, [api, token, currentDomainAccount])

    const putDomainAccountAsync = useCallback(async (domainAccount) => {
        await api.putDomainAccount(token, domainAccount)
        await getDomainAccountsAsync(domainAccountsFilter)
    }, [api, token, getDomainAccountsAsync, domainAccountsFilter])

    useEffect(() => {
        getDomainAccountsAsync(domainAccountsFilter)
    }, [token, currentDomainAccount])

    

    return <div className="container">
        { !selectedDomainAccount && <h1 className="header mt-2 mb-3">Domain's members</h1> }
        
        {
            selectedDomainAccount && selectedDomainAccount.action == domainAccountActions.openDetail  ?        
                <Profile api={api} accountId={selectedDomainAccount.domainAccount.id} onCancel={onCancel}/>
            :
            <>
                <ListElement>
                    <div className="col-3 align-middle">
                        <label htmlFor="domainOfferNameFilter" className="form-label">Email</label>
                        <input type="text" id="domainOfferNameFilter" className="form-control" value={domainAccountsFilter.email} onChange={(event) => setDomainAccountsFilter({...domainAccountsFilter, email: event.target.value})}></input> 
                    </div>
                    <div className="col-3 align-middle">
                        <label htmlFor="domainOfferNameFilter" className="form-label">Name</label>
                        <input type="text" id="domainOfferNameFilter" className="form-control" value={domainAccountsFilter.name} onChange={(event) => setDomainAccountsFilter({...domainAccountsFilter, name: event.target.value})}></input> 
                    </div>
                    <div className="col-4 align-middle form-check">
                        <label htmlFor={"showRequestsFilter"} className="form-check-label">Get requests</label>
                        <input type="checkbox" id={"showRequestsFilter"} className="form-check-input" checked={domainAccountsFilter.includeNotConfirmed} onChange={() => setDomainAccountsFilter({...domainAccountsFilter, includeConfirmed: !domainAccountsFilter.includeConfirmed, includeNotConfirmed: !domainAccountsFilter.includeNotConfirmed})}></input> 
                    </div>
                    <div className="col-2 mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32, marginLeft: -15, width: 125}} onClick={async () => await getDomainAccountsAsync(domainAccountsFilter)}>Search</button>
                    </div>
                </ListElement>     
                {              
                    domainAccounts.map((domainAccount, i) => <DomainAccount key={domainAccount.id} domainAccount={domainAccount} putDomainAccount={putDomainAccountAsync} openDomainAccountDetail={openDomainAccountDetail} onCancel={onCancel}/>)
                }
            </>
        }
    </div>  
}

const DomainAccount = ({domainAccount, putDomainAccount, openDomainAccountDetail, onCancel}) => {

    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    return <ListElement>
        <div className="container">
            <a title={domainAccount.name}>
                <div className="row mb-2"> 
                    <div className="col-2 align-middle">
                        { domainAccount.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainAccount.imageId)}></img> }               
                    </div>  
                    <div className="col-8 align-middle">
                        <p className="fs-5">{domainAccount.email}</p>
                        <p className="fs-5">{domainAccount.name}</p>
                    </div>              
                    <div className=" col-2">
                        <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={async () => openDomainAccountDetail(domainAccount)}>
                            Detail
                        </button>
                        {
                            (currentDomainAccount.isAdmin || currentDomainAccount.isOwner) &&
                            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={domainAccount.isConfirmed ? "Remove" : "Confirm"} onClick={async () => await putDomainAccount({domainAccountId: domainAccount.id, isConfirmed: !domainAccount.isConfirmed})}>
                                {domainAccount.isConfirmed ? "Remove" : "Confirm"}
                            </button>
                        }
                        
                    </div>        
                </div>
            </a> 
        </div>
    </ListElement>
}