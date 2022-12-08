import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { DomainDetailForm } from './domainDetailForm.js'
import { CroppableImage } from '../components/croppableImage/croppableImage'
import { getImageSrc } from '../imageSources.ts';
import { api } from '../api/api.ts'


export const DomainDetail = () => {

    const token = useSelector(state => state.user.token)
    const currentAccount = useSelector((state) => state.domainAccount.currentAccount)

    const [detail, setDetail] = useState({
        domainId: currentAccount.domainId,
        name: '',
        description: ''
    })

    const [imgSrc, setImgSrc] = useState()
    const [crop, setCrop] = useState()
    const [isEditState, setIsEditState] = useState(false)

    useEffect(() => {

        const effectAsync = async () => {
            const result = await api.getDomainDetail(token, currentAccount.domainId)

            console.log(JSON.stringify(result))

            if(result) {
                setDetail({
                    ...detail,
                    name: result.name,
                    description: result.description                   
                })
            }

            if(result.imageId) {
                setImgSrc(getImageSrc(result.imageId))
            }

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

        effectAsync()
    },[currentAccount])

    const setImage = (image) => {
        setDetail({
            ...detail,
            image: image,
        })
    }

    const handleSubmit = async (data) => {
        const newDetail = {...detail, ...data}
        setDetail(newDetail)

        const formData = new FormData();

        formData.append('domainId', newDetail.domainId)

        formData.append('description', newDetail.description)

        if(newDetail.image)
            formData.append('image', newDetail.image)
        if(crop){
            formData.append('cropParameters.x', parseFloat(crop.x))
            formData.append('cropParameters.y', parseFloat(crop.y))
            formData.append('cropParameters.width', parseFloat(crop.width))
            formData.append('cropParameters.height', parseFloat(crop.height))
        }

        await api.putDomainDetail(token, formData)
        
        setIsEditState(false)
    }

    const handleCancelEdit = () => {
        if(isEditState) {
            setIsEditState(false)
            return
        }
    }

    return <>
        <CroppableImage
            isEditState={isEditState}
            initImgSrc={imgSrc} 
            setImage={setImage}
            crop={crop} 
            setCrop={setCrop}
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