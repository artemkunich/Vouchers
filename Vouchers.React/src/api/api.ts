import { User } from 'oidc-client'
import { DomainAccountDto, DomainDetailDto } from '../types/dtos'
import { DomainAccountsQuery } from '../types/queries'
import { getById, getByQuery, postJson, putJson, postForm, putForm, deleteById, serializeToQuery } from './apiUtils'

import { UpdateDomainAccountCommand, UpdateDomainDetailCommand } from '../types/commands'

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
      
    getDomainDetail: (token: User, id: string): Promise<DomainDetailDto | undefined> => getById("DomainDetail", id, token.access_token) as Promise<DomainDetailDto | undefined>,
    putDomainDetail: (token: User, command: UpdateDomainDetailCommand) => putForm("DomainDetail", converUpdateDomainDetailCommandToForm(command), token.access_token),
    
    getIdentityDomainAccounts: (token: User, query: any) => getByQuery("IdentityDomainAccounts", serializeToQuery(query), token.access_token),

    getDomainAccounts: (token: User, query: DomainAccountsQuery): Promise<DomainAccountDto[]> => getByQuery("DomainAccounts", serializeToQuery(query), token.access_token) as Promise<DomainAccountDto[]>,
    postDomainAccount: (token: User, domainAccount: any) => postJson("DomainAccounts", domainAccount, token.access_token),
    putDomainAccount: (token: User, command: UpdateDomainAccountCommand): Promise<boolean | undefined> => putJson("DomainAccounts", command, token.access_token),

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

    postHolderTransaction: (token: User, transaction: any) => postJson("HolderTransactions", transaction, token.access_token),
    getHolderTransactions: (token: User, query: any) => getByQuery("HolderTransactions", serializeToQuery(query), token.access_token),
    
    getTransactionRequests: (token: User, query: any) => getByQuery("HolderTransactionRequests", serializeToQuery(query), token.access_token),
    getDomainValues: (token: User, query: any) => getByQuery("DomainValues", serializeToQuery(query), token.access_token),
    
    postTransactionRequest: (token: User, request: any) => postJson("HolderTransactionRequests", request, token.access_token),
    deleteTransactionRequest: (token: User, id: string) => deleteById("HolderTransactionRequests", id, token.access_token),
    getTransactionRequest: (token: User, id: string) => getById("HolderTransactionRequest", id, token.access_token),
}

const converUpdateDomainDetailCommandToForm = (command: UpdateDomainDetailCommand): FormData => {
    const formData = new FormData();

    formData.append('domainId', command.domainId)

    if(command.description){
        formData.append('description', command.description)
    }
        
    if(command.image){
        formData.append('image', command.image)
    }

    if(command.cropParameters){
        formData.append('cropParameters.x', command.cropParameters.x.toString())
        formData.append('cropParameters.y', command.cropParameters.y.toString())
        formData.append('cropParameters.width', command.cropParameters.width.toString())
        formData.append('cropParameters.height', command.cropParameters.height.toString())
    }

    return formData
}