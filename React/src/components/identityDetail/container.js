import React, { useState, useEffect, useCallback} from 'react'
import { useSelector, useDispatch} from 'react-redux'
import { identityDefined } from '../../store/userReducer.ts'
import { CroppableImage } from '../common/croppableImage'
import { IdentityDetailForm } from './form.js'
import { getImageSrc } from '../../api/imageSources.ts';
import { api } from '../../api/api.ts'

export const IdentityDetailContainer = ({accountId, handleCancel}) => {

    const token = useSelector((state) => state.user.token)
    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const dispatch = useDispatch()

    const [detail, setDetail] = useState({
        firstName: "",
        lastName: "",
        email: token?.profile?.name
    })

    const [imgSrc, setImgSrc] = useState()
    const [crop, setCrop] = useState()
    const [isEditState, setIsEditState] = useState(false)

    useDetail(token, accountId ?? currentAccount?.id, api.getIdentityDetail, setDetail, setImgSrc, setCrop)

    const handleSubmit = async (data) => {
        const newDetail = {...detail, ...data}
        setDetail(newDetail)

        const formData = new FormData();
        formData.append('firstName', newDetail.firstName)
        formData.append('lastName', newDetail.lastName)
        formData.append('email', newDetail.email)

        if(newDetail.image)
            formData.append('image', newDetail.image)

        if(crop) {
            formData.append('cropParameters.X', parseFloat(crop.x))
            formData.append('cropParameters.Y', parseFloat(crop.y))
            formData.append('cropParameters.Width', parseFloat(crop.width))
            formData.append('cropParameters.Height', parseFloat(crop.height))
        }

        if(newDetail.identityId)
            await api.putIdentityDetail(token, formData)
        else
            await api.postIdentityDetail(token, formData) 
        
        var idDetail = await api.getIdentityDetail(token)

        if(idDetail) {
            dispatch(identityDefined(idDetail))
        }
    }

    const setImage = (image) => {
        setDetail({
            ...detail,
            image: image,
        })
    }

    const handleSetRole = async (role) => await api.putDomainAccount(token, role)

    const handleCancelEdit = () => {
        if(isEditState) {
            setIsEditState(false)
            return
        }
            
        handleCancel && handleCancel() 
    }

    return (
        <>
            <CroppableImage
                isEditState={isEditState}
                initImgSrc={imgSrc} 
                setImage={setImage}
                crop={crop} 
                setCrop={setCrop}
            />   

            <IdentityDetailForm
                detail={detail}
                currentAccount={currentAccount}
                accountId={accountId} 
                isEditState={isEditState} 
                setIsEditState={setIsEditState}
                handleSetRole={handleSetRole}
                handleSubmit={handleSubmit}
                handleCancel={handleCancelEdit}
            />
        </>
    );
}



const useDetail = (token, accountId, getIdentityDetail, setDetail, setImgSrc, setCrop) => {

    const identityId = useSelector((state) => state.user.identityId)

    useEffect(() => {
        const effectAsync = async () => {
            if(identityId) {
                const result = await getIdentityDetail(token, accountId)
                setDetail({
                    ...result,
                })
                if(result.imageId)
                    setImgSrc(getImageSrc(result.imageId))

                console.log('cropParams: ' + JSON.stringify(result.cropParameters))

                if(result.cropParameters)
                    setCrop({
                        unit: '%',
                        x: result.cropParameters.x,
                        y: result.cropParameters.y,
                        keepSelection : true,
                        width: result.cropParameters.width,
                        height: result.cropParameters.height,
                    })
            }
        }

        effectAsync()
    }, []);
}