import * as React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { DomainDetailForm } from './domainDetailForm'
import { CroppableImage } from '../components/croppableImage/croppableImage'
import { getImageSrc } from '../imageSources';
import { RootState } from '../store/store'
import { User } from 'oidc-client'
import { api } from '../api/api'
import { DomainAccountDto, DomainDetailDto } from '../types/dtos';
import { UpdateDomainDetailCommand } from '../types/commands'
import { DomainDetailFormData } from './domainDetailForm'
import { PercentCrop } from 'react-image-crop';


export const DomainDetail = () => {

    const token = useSelector<RootState, User | undefined>(state => state.user.token)
    const currentAccount = useSelector<RootState, DomainAccountDto | undefined>((state) => state.domainAccount.currentAccount)

    const [detail, setDetail] = React.useState<DomainDetailDto | undefined>()

    const [imgSrc, setImgSrc] = React.useState<string>()
    const [crop, setCrop] = React.useState<PercentCrop>()

    const [isEditState, setIsEditState] = React.useState(false)
    const [newImage, setNewImage] = React.useState<File>()
    const [newCrop, setNewCrop] = React.useState<PercentCrop>()

    React.useEffect(() => {

        const effectAsync = async () => {
            
            if(!token || !currentAccount)
                return
            
            const result = await api.getDomainDetail(token, currentAccount.domainId)

            console.log(JSON.stringify(result))

            if(!result || currentAccount.domainId !== result.id)
                return 

            setDetail(result)

            if(result.imageId) {
                setImgSrc(getImageSrc(result.imageId))
            }
            
            console.log('cropParams: ' + JSON.stringify(result.cropParameters))

            if(result.cropParameters)
                setCrop({
                    unit: '%',
                    x: result.cropParameters.x,
                    y: result.cropParameters.y,
                    //keepSelection : true,
                    width: result.cropParameters.width,
                    height: result.cropParameters.height,
                })
        }

        effectAsync()
    },[currentAccount])

    const handleSubmit = async (data: DomainDetailFormData) => {

        if(!token || !detail)
            return

        let isAnyChange = false 
            
        const updateDomainDetailCommand: UpdateDomainDetailCommand = {
            domainId: detail.id,   
        }

        if(data.description != detail.description){
            updateDomainDetailCommand.description = data.description
            detail.description = data.description
            isAnyChange = true
        }

        if(newImage){
            updateDomainDetailCommand.image = newImage
            isAnyChange = true
        }

        if(newCrop){
            updateDomainDetailCommand.cropParameters = newCrop
            isAnyChange = true
        }

        if(isAnyChange){
            await api.putDomainDetail(token, updateDomainDetailCommand)
        }
        setIsEditState(false)
    }

    const handleCancelEdit = () => {
        if(isEditState) {
            setIsEditState(false)
            return
        }
    }

    return detail && currentAccount &&
    <>
        <CroppableImage
            isEditState={isEditState}
            initImgSrc={imgSrc ?? ""}
            setImage={setNewImage}
            crop={crop}
            setCrop={setNewCrop}
        />
        <DomainDetailForm
            detail={detail}
            currentAccount={currentAccount}
            isEditState={isEditState}
            setIsEditState={setIsEditState}
            handleSubmit={handleSubmit}
            handleCancel={handleCancelEdit}
        />
    </>
}