import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { Routes, Route, Link, useSearchParams } from 'react-router-dom';
import { login, mgr } from '../api/userManager.js'

import { store } from '../store/store.ts'
import { userSigned, identityDefined } from '../store/userReducer.ts'

import { IdentityDetailContainer } from '../containers/identityDetailContainer.js'
import { DomainOffersContainer } from '../containers/DomainOffersContainer.js'
import { DomainsContainer } from '../containers/domainsContainer.js'
import { DomainMembersContainer } from '../containers/domainMembersContainer.js'
import { DomainValuesContainer } from '../containers/domainValuesContainer.js'
import { ValueDetail } from '../valueDetail/valueDetail.js'
import { IssuerValuesContainer } from '../containers/issuerValuesContainer.js'
import { NewDomainContainer } from '../containers/newDomainContainer.js'
import { HolderValuesContainer } from '../containers/HolderValuesContainer.js'
import { HolderTransactionsContainer } from '../containers/holderTransactionsContainer.js'
import { IncomingTransactionRequestsContainer } from '../containers/incomingTransactionRequestsContainer.js'
import { OutgoingTransactionRequestsContainer } from '../containers/outgoingTransactionRequestsContainer.js'
import { PerformRequestContainer } from '../containers/performRequestContainer.js'
import { DomainDetailContainer } from '../containers/domainDetailContainer.js'
import { Layout } from '../layout/layout.js';

import { api } from '../api/api.ts'

import "./style.css";

export const App = () => {
    const token = useSelector((state) => state.user.token)
    const identityId = useSelector((state) => state.user.identityId)
    const identityIdSet = useSelector((state) => state.user.identityIdSet)

    useToken(token, identityId, api.getIdentityDetail)

    if(token && identityIdSet) {
        if(identityId)
        {
            return (
                <div className='App h-100'>
                        <Routes>
                            <Route path="/" element={<Layout/>}>
                                <Route index element={<Home/>}/>
                                <Route path="profile" element={<div className="container"><IdentityDetailContainer/></div>}/>
                                <Route path="profile/:id" element={<div className="container"><IdentityDetailContainer/></div>}/>                                
                                <Route path="domainOffers" element={<div className="container"><DomainOffersContainer/></div>}/>
                                <Route path="newDomain" element={<div className="container"><NewDomainContainer/></div>}/>
                                <Route path="domains" element={<div className="container"><DomainsContainer/></div>}/>
                                <Route path="domainMembers" element={<div className="container"><DomainMembersContainer/></div>}/>
                                <Route path="domainValues" element={<div className="container"><DomainValuesContainer/></div>}/>
                                <Route path="newValue" element={<ValueDetail header={"New value"} valuesItem={{}}/>}/> 
                                <Route path="issuerValues" element={<div className="container"><IssuerValuesContainer/></div>}/>
                                <Route path="holderValues" element={<div className="container"><HolderValuesContainer/></div>}/>
                                <Route path="holderTransactions" element={<div className="container"><HolderTransactionsContainer/></div>}/>                             
                                <Route path="domainDetail" element={<div className="container"><DomainDetailContainer/></div>}/>
                                <Route path="incomingRequests" element={<div className="container"><IncomingTransactionRequestsContainer/></div>}/>
                                <Route path="outgoingRequests" element={<div className="container"><OutgoingTransactionRequestsContainer/></div>}/>
                                <Route path="performRequest" element={<div className="container"><PerformRequestContainer/></div>}/>
                            </Route>
                        </Routes>
                </div>
            );
        }

        return <IdentityDetailContainer/>
    }
    return <div>Loading...</div>
}

const useToken = (token, identityId, getIdentityDetail) => {
    const dispatch = useDispatch()

    useEffect(() => {
        const getTokenAsync = async () => {
            console.log(`Store: ${JSON.stringify(store.getState())}`)

            if(!token) {
                try {
                    const t = await mgr.signinRedirectCallback()
                    console.log(`Use effect: ${t}`)
                    dispatch(userSigned(t))

                    if(!identityId) {
                        try {
                            const identityDetail = await getIdentityDetail(t)              
                            dispatch(identityDefined(identityDetail))
                        } catch (error) {
                            console.log(error)
                        }
                    }

                } catch (error) {
                    console.log(error)
                    login()
                }
            }
        }

        getTokenAsync();
    },[]);
}

const Home = () => {

    return (
        <>
          <div>
            <h2 className='display-1'>Welcome to the VOU!</h2>
            <p>Create and exchange vouchers</p>
          </div>
        </>
    )
}