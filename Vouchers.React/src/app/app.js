import React, { useState, useEffect, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux'
import { Routes, Route, Link, useSearchParams } from 'react-router-dom';
import { login, mgr } from '../infrastructure/userManager.js'

import { store } from '../store/store.js';
import { userSigned, identityDefined } from '../store/userReducer.js';
import { createAuthHttpRequest, serializeToQuery } from '../infrastructure/APIUtils.js';

import { createGetIdentityDetail, createPostIdentityDetail } from '../infrastructure/profileAPI.js';
import { Profile } from '../profile/profile.js';

import { createGetDomainOffers, createPostDomainOffer, createPutDomainOffer } from '../infrastructure/domainOffersAPI.js';
import { DomainOffers } from '../domainOffers/domainOffers.js';

import { createGetIdentityDomainOffers } from '../infrastructure/identityDomainOffersAPI.js';
import { NewDomain } from '../newDomain/newDomain.js'

import { createGetDomains, createPostDomain } from '../infrastructure/domainsAPI.js';
import { Domains } from '../domains/domains.js';

import { DomainMembers } from '../domainMembers/domainMembers.js';

import { DomainValues } from '../domainValues/domainValues.js';

import { ValueDetail } from '../valueDetail/valueDetail.js';

import { createGetIssuerValues, createPostIssuerValue, createPutIssuerValue} from '../infrastructure/issuerValuesAPI.js'
import { createGetIssuerVouchers, createPostIssuerVoucher, createPutIssuerVoucher} from '../infrastructure/issuerVouchersAPI.js'
import { createGetIssuerTransactions, createPostIssuerTransaction } from '../infrastructure/issuerTransactionsAPI.js'
import { createGetValueDetail } from '../infrastructure/valueDetailAPI.js'
import { IssuerValues } from '../issuerValues/issuerValues.js';

import { createGetHolderValues } from '../infrastructure/holderValuesAPI.js'
import { createGetHolderVouchers } from '../infrastructure/holderVouchersAPI.js'
import { createGetHolderTransactions, createPostHolderTransaction } from '../infrastructure/holderTransactionsAPI.js'
import { HolderValues } from '../holderValues/holderValues.js';
import { HolderTransactions } from '../holderTransactions/holderTransactions.js';


import { createGetTransactionRequests, createGetTransactionRequest, createGetDomainValues, createPostTransactionRequest, createDeleteTransactionRequest } from '../infrastructure/transactionRequestsAPI.js'
import { IncomingRequests } from '../holderTransactionRequests/incomingRequests.js'
import { OutgoingRequests } from '../holderTransactionRequests/outgoingRequests.js'
import { PerformRequest } from '../holderTransactionRequests/performRequest.js'

import { createGetDomainDetail } from '../infrastructure/domainDetailAPI.js'
import { createPutDomainDetail } from '../infrastructure/domainDetailAPI.js'
import { DomainDetail } from '../domainDetail/domainDetail.js'

import { createGetDomainAccounts, createPostDomainAccount, createPutDomainAccount } from '../infrastructure/domainAccountsAPI.js';
import { createGetIdentityDomainAccounts } from '../infrastructure/identityDomainAccountsAPI.js';
import { Layout } from '../layout/layout.js';

import "./style.css";

export const App = ({apiUrl}) => {
    const token = useSelector((state) => state.user.token)
    const identityId = useSelector((state) => state.user.identityId)
    const identityIdSet = useSelector((state) => state.user.identityIdSet)

    const authHttpRequest = createAuthHttpRequest(apiUrl, login)

    const getIdentityDetail = createGetIdentityDetail(authHttpRequest, serializeToQuery);

    const api = {
        getIdentityDetail: getIdentityDetail,
        postIdentityDetail: createPostIdentityDetail(authHttpRequest),

        getDomainOffers: createGetDomainOffers(authHttpRequest, serializeToQuery),
        postDomainOffer: createPostDomainOffer(authHttpRequest),
        putDomainOffer: createPutDomainOffer(authHttpRequest),

        getIdentityDomainOffers: createGetIdentityDomainOffers(authHttpRequest, serializeToQuery),
        postDomain: createPostDomain(authHttpRequest),

        getDomains: createGetDomains(authHttpRequest, serializeToQuery),
        postDomainAccount: createPostDomainAccount(authHttpRequest),

        getDomainDetail: createGetDomainDetail(authHttpRequest, serializeToQuery),
        putDomainDetail: createPutDomainDetail(authHttpRequest),

        getIdentityDomainAccounts: createGetIdentityDomainAccounts(authHttpRequest, serializeToQuery),

        getDomainAccounts: createGetDomainAccounts(authHttpRequest, serializeToQuery),
        putDomainAccount: createPutDomainAccount(authHttpRequest),

        getValues: createGetIssuerValues(authHttpRequest, serializeToQuery),
        postValue: createPostIssuerValue(authHttpRequest),
        putValue: createPutIssuerValue(authHttpRequest),
   
        getIssuerVouchers: createGetIssuerVouchers(authHttpRequest, serializeToQuery),
        postIssuerVoucher: createPostIssuerVoucher(authHttpRequest),
        putIssuerVoucher: createPutIssuerVoucher(authHttpRequest),

        postIssuerTransaction: createPostIssuerTransaction(authHttpRequest),
        getValueDetail: createGetValueDetail(authHttpRequest),

        getHolderValues: createGetHolderValues(authHttpRequest, serializeToQuery),
        getHolderVouchers: createGetHolderVouchers(authHttpRequest, serializeToQuery),
        postHolderTransaction: createPostHolderTransaction(authHttpRequest),
        getHolderTransactions: createGetHolderTransactions(authHttpRequest, serializeToQuery),

        getTransactionRequests: createGetTransactionRequests(authHttpRequest, serializeToQuery),
        getDomainValues: createGetDomainValues(authHttpRequest, serializeToQuery),
        postTransactionRequest: createPostTransactionRequest(authHttpRequest),
        deleteTransactionRequest: createDeleteTransactionRequest(authHttpRequest),
        getTransactionRequest: createGetTransactionRequest(authHttpRequest)
    }

    useToken(token, identityId, getIdentityDetail)

    if(token && identityIdSet) {
        if(identityId)
        {
            return (
                <div className='App h-100'>
                        <Routes>
                            <Route path="/" element={<Layout api={api}/>}>
                                <Route index element={<Home/>}/>
                                <Route path="profile" element={<Profile api={api}/>}/>
                                <Route path="profile/:id" element={<Profile api={api}/>}/>                                
                                <Route path="domainOffers" element={<DomainOffers api={api}/>}/>
                                <Route path="newDomain" element={<NewDomain api={api}/>}/>
                                <Route path="domains" element={<Domains api={api}/>}/>
                                <Route path="domainMembers" element={<DomainMembers api={api}/>}/>
                                <Route path="domainValues" element={<DomainValues api={api}/>}/>
                                <Route path="newValue" element={<ValueDetail header={"New value"} valuesItem={{}} api={api}/>}/> 
                                <Route path="issuerValues" element={<IssuerValues api={api}/>}/>
                                <Route path="holderValues" element={<HolderValues api={api}/>}/>
                                <Route path="holderTransactions" element={<HolderTransactions api={api}/>}/>                             
                                <Route path="domainDetail" element={<DomainDetail api={api}/>}/>
                                <Route path="incomingRequests" element={<IncomingRequests api={api}/>}/>
                                <Route path="outgoingRequests" element={<OutgoingRequests api={api}/>}/>
                                <Route path="performRequest" element={<PerformRequest api={api}/>}/>
                            </Route>
                        </Routes>
                </div>
            );
        }

        return <Profile api={api}/>
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
            <p>Create and exchange your own vouchers</p>
          </div>
        </>
    )
}