interface PagedQuery {
    pageIndex?: number,
    pageSize?: number,
}

export interface DomainAccountsQuery extends PagedQuery {
    domainId: string,
    email?: string,
    name?: string,
    includeConfirmed?: boolean,
    includeNotConfirmed?: boolean,
}

export interface DomainOffersQuery extends PagedQuery
    {
        name?: string,

        minMaxSubscribersCount?: number
        maxMaxSubscribersCount?: number

        invoicePeriod?: number
        currency?: number
        maxAmount?: number
        minAmount?: number

        recipientId?: string

        includeExpired?: boolean
        includePlanned?: boolean
    }