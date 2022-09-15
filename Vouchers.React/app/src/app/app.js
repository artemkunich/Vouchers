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

import { createGetIssuerValues, createPostIssuerValue, createPutIssuerValue} from '../infrastructure/issuerValuesAPI.js'
import { createGetIssuerVouchers, createPostIssuerVoucher, createPutIssuerVoucher} from '../infrastructure/issuerVouchersAPI.js'
import { createGetIssuerTransactions, createPostIssuerTransaction } from '../infrastructure/issuerTransactionsAPI.js'
import { createGetValueDetail } from '../infrastructure/valueDetailAPI.js'
import { IssuerBoard } from '../issuerBoard/issuerBoard.js';

import { createGetHolderValues } from '../infrastructure/holderValuesAPI.js'
import { createGetHolderVouchers } from '../infrastructure/holderVouchersAPI.js'
import { createGetHolderTransactions, createPostHolderTransaction } from '../infrastructure/holderTransactionsAPI.js'
import { HolderBoard } from '../holderBoard/holderBoard.js';


import { createGetDomainAccounts, createPostDomainAccount, createPutDomainAccount } from '../infrastructure/domainAccountsAPI.js';
import { createGetIdentityDomainAccounts } from '../infrastructure/identityDomainAccountsAPI.js';
import { Layout } from '../layout/layout.js';

import "./style.css";

export const App = ({apiUrl}) => {
    const token = useSelector((state) => state.user.token)
    const identityId = useSelector((state) => state.user.identityId)
    const identityIdSet = useSelector((state) => state.user.identityIdSet)

    const authHttpRequest = createAuthHttpRequest(apiUrl, login)


    //Identity
    const getIdentityDetail = createGetIdentityDetail(authHttpRequest, serializeToQuery);

    const identityDetailApi = {
        getIdentityDetail: getIdentityDetail,
        postIdentityDetail: createPostIdentityDetail(authHttpRequest)
    }

    //Offers
    const offersApi = {
        getDomainOffers: createGetDomainOffers(authHttpRequest, serializeToQuery),
        postDomainOffer: createPostDomainOffer(authHttpRequest),
        putDomainOffer: createPutDomainOffer(authHttpRequest)
    }

    //New domain
    const newDomainApi = {
        getDomainOffers: createGetIdentityDomainOffers(authHttpRequest, serializeToQuery),
        postDomain: createPostDomain(authHttpRequest)
    }

    //Domains
    const domainsApi = {
        getDomains: createGetDomains(authHttpRequest, serializeToQuery),
        postDomainAccount: createPostDomainAccount(authHttpRequest)
    }

    //Subscriptions
    const layoutApi = {
        getDomainAccounts: createGetIdentityDomainAccounts(authHttpRequest, serializeToQuery)
    }

    //Members
    const getDomainAccounts = createGetDomainAccounts(authHttpRequest, serializeToQuery)

    const domainMembersApi = {
        getDomainAccounts: getDomainAccounts,
        putDomainAccount: createPutDomainAccount(authHttpRequest)
    }

    //Issuer's board
    const issuerBoardApi = {
        getValues: createGetIssuerValues(authHttpRequest, serializeToQuery),
        postValue: createPostIssuerValue(authHttpRequest),
        putValue: createPutIssuerValue(authHttpRequest),
   
        getVouchers: createGetIssuerVouchers(authHttpRequest, serializeToQuery),
        postVoucher: createPostIssuerVoucher(authHttpRequest),
        putVoucher: createPutIssuerVoucher(authHttpRequest),

        postTransaction: createPostIssuerTransaction(authHttpRequest),
        getValueDetail: createGetValueDetail(authHttpRequest)
    }

    //Holder's board
    const holderBoardApi = {
        getValues: createGetHolderValues(authHttpRequest, serializeToQuery),
        getVouchers: createGetHolderVouchers(authHttpRequest, serializeToQuery),
        postTransaction: createPostHolderTransaction(authHttpRequest),
        getDomainAccounts: getDomainAccounts
    }

    useToken(token, identityId, getIdentityDetail)

    if(token && identityIdSet) {
        if(identityId)
        {
            return (
                <div className='App h-100'>
                        <Routes>
                            <Route path="/" element={<Layout api={layoutApi}/>}>
                                <Route index element={<Home/>}/>
                                <Route path="profile" element={<Profile api={identityDetailApi}/>}/>                                
                                <Route path="domainOffers" element={<DomainOffers api={offersApi}/>}/>
                                <Route path="newDomain" element={<NewDomain api={newDomainApi}/>}/>
                                <Route path="domains" element={<Domains api={domainsApi}/>}/>
                                <Route path="domainMembers" element={<DomainMembers api={domainMembersApi}/>}/>
                                <Route path="issuerBoard" element={<IssuerBoard api={issuerBoardApi}/>}/>
                                <Route path="holderBoard" element={<HolderBoard api={holderBoardApi}/>}/>
                            </Route>
                        </Routes>
                </div>
            );
        }

        return <Profile api={identityDetailApi}/>
    }
    return <div>Loading...</div>
}

const useToken = (token, identityId, getIdentityDetail) => {
    const dispatch = useDispatch()

    useEffect(() => {
        const getToken = async () => {
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

        getToken();
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