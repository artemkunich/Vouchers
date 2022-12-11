import { PercentCrop } from 'react-image-crop'

export interface UpdateDomainDetailCommand
{
    domainId: string,

    name?: string,

    description?: string,

    isPublic?: boolean,

    cropParameters?: PercentCrop, 

    image?: any,
}

export interface UpdateDomainAccountCommand
{
    domainAccountId: string,

    isConfirmed?: boolean,

    isIssuer?: boolean,

    isAdmin?: boolean
}


export interface CreateDomainOfferCommand
{
    name: string

    description?: string

    maxMembersCount: number
    amount: number
    currency: string
    invoicePeriod: string
    validFrom: string
    validTo?: string

    maxContractsPerIdentity?: number
}

export interface UpdateDomainOfferCommand
{
    id: string

    terminate?: boolean

    description?: string

    validFrom?: string

    validTo?: string

    maxContractsPerIdentity?: number
}