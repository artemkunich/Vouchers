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