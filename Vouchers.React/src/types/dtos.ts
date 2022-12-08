export interface DomainAccountDto {

    id: string,

    domainId: string,

    domainName: string,

    email: string,

    name: string,

    isAdmin: boolean,
    isIssuer: boolean,
    isOwner: boolean,
    isConfirmed: boolean,

    imageId?: string
}