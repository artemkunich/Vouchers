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