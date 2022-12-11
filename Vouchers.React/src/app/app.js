import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { Routes, Route, Link, useSearchParams } from 'react-router-dom'
import { login, mgr } from '../api/userManager.ts'

import { store } from '../store/store.ts'
import { userSigned, identityDefined } from '../store/userReducer.ts'

import { PageIdentityDetail } from '../pages/identityDetail.js'
import { PagesDomainOffers } from '../pages/domainOffers.js'
import { PagesDomains } from '../pages/domains.js'
import { PagesDomainMembers } from '../pages/domainMembers.js'
import { PagesDomainValues } from '../pages/domainValues.js'
import { ValueDetailContainer } from '../components/valueDetail/container.js'
import { PagesIssuerValues } from '../pages/issuerValues.js'
import { PagesNewDomain } from '../pages/newDomain.js'
import { PagesHolderValues } from '../pages/holderValues.js'
import { PagesHolderTransactions } from '../pages/holderTransactions.js'
import { PagesIncomingTransactionRequests } from '../pages/incomingTransactionRequests.js'
import { PagesOutgoingTransactionRequests } from '../pages/outgoingTransactionRequests.js'
import { PagesPerformRequest } from '../pages/performRequest.js'
import { PagesDomainDetail } from '../pages/domainDetail.js'
import { Layout } from '../layout/layout.js'

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
                                <Route path="profile" element={<div className="container"><PageIdentityDetail/></div>}/>
                                <Route path="profile/:id" element={<div className="container"><PageIdentityDetail/></div>}/>                                
                                <Route path="domainOffers" element={<div className="container"><PagesDomainOffers/></div>}/>
                                <Route path="newDomain" element={<div className="container"><PagesNewDomain/></div>}/>
                                <Route path="domains" element={<div className="container"><PagesDomains/></div>}/>
                                <Route path="domainMembers" element={<div className="container"><PagesDomainMembers/></div>}/>
                                <Route path="domainValues" element={<div className="container"><PagesDomainValues/></div>}/>
                                <Route path="newValue" element={<ValueDetailContainer header={"New value"} valuesItem={{}}/>}/> 
                                <Route path="issuerValues" element={<div className="container"><PagesIssuerValues/></div>}/>
                                <Route path="holderValues" element={<div className="container"><PagesHolderValues/></div>}/>
                                <Route path="holderTransactions" element={<div className="container"><PagesHolderTransactions/></div>}/>                             
                                <Route path="domainDetail" element={<div className="container"><PagesDomainDetail/></div>}/>
                                <Route path="incomingRequests" element={<div className="container"><PagesIncomingTransactionRequests/></div>}/>
                                <Route path="outgoingRequests" element={<div className="container"><PagesOutgoingTransactionRequests/></div>}/>
                                <Route path="performRequest" element={<div className="container"><PagesPerformRequest/></div>}/>
                            </Route>
                        </Routes>
                </div>
            );
        }

        return <PageIdentityDetail/>
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