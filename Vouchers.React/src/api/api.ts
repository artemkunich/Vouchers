import { User } from 'oidc-client';
import { DomainAccountDto } from '../types/dtos';
import { DomainAccountsQuery } from '../types/queries';
import { getById, getByQuery, postJson, putJson, postForm, putForm, deleteById, serializeToQuery } from './apiUtils';

export const api = {
    getIdentityDetail: (token: User, query: any) => getByQuery("IdentityDetail", serializeToQuery(query), token.access_token),
    postIdentityDetail: (token: User, form: FormData) => postForm("IdentityDetail", form, token.access_token),
    putIdentityDetail: (token: User, form: FormData) => putForm("IdentityDetail", form, token.access_token),

    getDomainOffers: (token: User, query: any) => getByQuery("DomainOffers", serializeToQuery(query), token.access_token),
    postDomainOffer: (token: User, offer: any) => postJson("DomainOffers", offer, token.access_token),
    putDomainOffer: (token: User, offer: any) => putJson("DomainOffers", offer, token.access_token),
    
    getIdentityDomainOffers: (token: User, query: any) => getByQuery("IdentityDomainOffers", serializeToQuery(query), token.access_token),
    
    getDomains: (token: User, query: any) => getByQuery("Domains", serializeToQuery(query), token.access_token),
    postDomain: (token: User, domain: any) => postJson("Domains", domain, token.access_token),
      
    getDomainDetail: (token: User, id: string) => getById("DomainDetail", id, token.access_token),
    putDomainDetail: (token: User, form: FormData) => putForm("DomainDetail", form, token.access_token),
    
    getIdentityDomainAccounts: (token: User, query: any) => getByQuery("IdentityDomainAccounts", serializeToQuery(query), token.access_token),

    getDomainAccounts: (token: User, query: DomainAccountsQuery) => getByQuery("DomainAccounts", serializeToQuery(query), token.access_token),
    postDomainAccount: (token: User, domainAccount: any) => postJson("DomainAccounts", domainAccount, token.access_token),
    putDomainAccount: (token: User, domainAccount: any) =>putJson("DomainAccounts", domainAccount, token.access_token),

    getValues: (token: User, query: any) => getByQuery("IssuerValues", serializeToQuery(query), token.access_token),
    postValue: (token: User, form: FormData) => postForm("IssuerValues", form, token.access_token),
    putValue: (token: User, form: FormData) => putForm("IssuerValues", form, token.access_token),

    getIssuerVouchers: (token: User, query: any) => getByQuery("IssuerVouchers", serializeToQuery(query), token.access_token),
    postIssuerVoucher: (token: User, issuerVoucher: any) => postJson("IssuerVouchers", issuerVoucher, token.access_token),
    putIssuerVoucher: (token: User, issuerVoucher: any) => putJson("IssuerVouchers", issuerVoucher, token.access_token),

    postIssuerTransaction: (token: User, transaction: any) => postJson("IssuerTransactions", transaction, token.access_token),
    getValueDetail: (token: User, id: string) => getById("ValueDetail", id, token.access_token),

    getHolderValues: (token: User, query: any) => getByQuery("HolderValues", serializeToQuery(query), token.access_token),
    getHolderVouchers: (token: User, query: any) => getByQuery("HolderVouchers", serializeToQuery(query), token.access_token),

    getHolderTransactions: (token: User, query: any) => getByQuery("HolderTransactions", serializeToQuery(query), token.access_token),
    
    getTransactionRequests: (token: User, query: any) => getByQuery("HolderTransactionRequests", serializeToQuery(query), token.access_token),
    getDomainValues: (token: User, query: any) => getByQuery("DomainValues", serializeToQuery(query), token.access_token),
    
    postTransactionRequest: (token: User, request: any) => postJson("HolderTransactionRequests", request, token.access_token),
    deleteTransactionRequest: (token: User, id: string) => deleteById("HolderTransactionRequests", id, token.access_token),
    getTransactionRequest: (token: User, id: string) => getById("HolderTransactionRequest", id, token.access_token),
}