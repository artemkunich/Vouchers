import { PercentCrop } from 'react-image-crop'

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

export interface DomainDetailDto
    {
        id: string,

        name: string,

        description?: string,

        isPublic: boolean,

        membersCount: number

        imageId?: string

        cropParameters?: PercentCrop
    }