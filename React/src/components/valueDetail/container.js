import React, {useState, useEffect, useRef, useCallback, useMemo } from 'react'
import {useSelector } from 'react-redux'
import { noImageFoundSrc, getImageSrc } from '../../api/imageSources.ts';
import {api} from '../../api/api.ts'
import ReactCrop from 'react-image-crop'
import "bootstrap-icons/font/bootstrap-icons.css";

import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

export const ValueDetailContainer = ({header, valuesItem, onCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [detail, setDetail] = useState({
        id: valuesItem.id,
        issuerAccountId: valuesItem.issuerAccountId ?? currentDomainAccount.id,
        ticker: valuesItem.ticker,
        description: valuesItem.description
    })

    const [crop, setCrop] = useState()
    const [imgSrc, setImgSrc] = useState()
    const [isCropState, setIsCropState] = useState(false)
    const [isEditState, setIsEditState] = useState(valuesItem.id ? false : true)

    useEffect(() => {

        const effectAsync = async () => {

            if(!valuesItem.id)
                return

            const result = await api.getValueDetail(token, valuesItem.id)

            console.log(JSON.stringify(result))

            setDetail({
                ...detail,
                ticker: result.ticker,
                description: result.description
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

        effectAsync()
    },[api, token, currentDomainAccount])

    const onSelectFile = e => {
        if (e.target.files && e.target.files.length > 0) {
            setDetail({
                ...detail,
                image: e.target.files[0],
            })
            setCrop(undefined) // Makes crop preview update between images.
            const reader = new FileReader()
            reader.addEventListener('load', () => {
                setImgSrc(reader.result.toString() || '')
                setIsCropState(true)
            })
            reader.readAsDataURL(e.target.files[0])
        }
    }

    const onImageLoad = (e) => {

        if(crop){
            return
        }

        const { width, height } = e.currentTarget

        const side = Math.min(width, height)

        const cropRelation = 66

        const percentWidth = cropRelation * side / width
        const percentHeight = cropRelation * side / height

        const percentX = (100 - percentWidth) / 2
        const percentY = (100 - percentHeight) / 2

        setCrop({
            unit: '%', // Can be 'px' or '%'
            x: percentX,
            y: percentY,
            keepSelection : true,
            width: percentWidth,
            height: percentHeight,
        })

    }

    const handleSaveCrop = () => {
        setIsCropState(false)
    }

    const handleSubmitAsync = async (event) => {
        const formData = new FormData();

        console.log(JSON.stringify(detail))

        formData.append('ticker', detail.ticker)
        formData.append('description', detail.description)

        if(detail.image)
            formData.append('image', detail.image)
        if(crop){
            formData.append('cropParameters.x', parseFloat(crop.x))
            formData.append('cropParameters.y', parseFloat(crop.y))
            formData.append('cropParameters.width', parseFloat(crop.width))
            formData.append('cropParameters.height', parseFloat(crop.height))
        }



        if(detail.id ){
            formData.append('id', detail.id)

            await api.putValue(token, formData)           
        } else {
            if(currentDomainAccount.id) {
                formData.append('issuerAccountId', currentDomainAccount.id)

                const newValueId = await api.postValue(token, formData)
                setDetail({
                    ...detail,
                    voucherValueId: newValueId
                })
            }
        }

        setIsEditState(false)
    }

    const handleOnCancel = () => {
        if(isEditState) {
            setIsEditState(false)
            return
        }
            
        onCancel && onCancel() 
    }


    return <div className="container">
        {
            header && <h1 className="header mt-2 mb-3">{header}</h1>
        }     
        {
            isEditState && isCropState ?
            <>
                  
                <div className="row mb-2">
                    <div className="col-12">
                        <ReactCrop crop={crop} onChange={(pixelCrop, percentCrop) => setCrop(percentCrop)} aspect={1}>
                            <img id='croppedImage' style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={imgSrc} onLoad={onImageLoad} />
                        </ReactCrop>
                    </div>
                </div>
                <div className="row mb-2">
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" onClick={handleSaveCrop}>Save</button>
                    </div>
                </div>
            </>
            :
            <>
                <div className="row mb-2">
                    <div className="col-6">
                        <img style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={`${imgSrc ?? noImageFoundSrc}`}></img>
                    </div>
                </div>
                { 
                    isEditState &&
                    <div className="row mb-2">                          
                        <div className="d-grid col-3">
                            { imgSrc && <button className="btn btn-primary" onClick={() => {setIsCropState(true); setCrop(crop)}}>Crop image</button> }
                        </div>
                        <div className="col-3">
                            <input type="file" name="Image" onChange={onSelectFile}/>
                        </div>
                    </div>
                }
            </>
                            
        }  
        <div className="row mb-2" >
            <div className="col-12">
                <label htmlFor="valueTicker" className="form-label">Ticker</label>
                <input type="text" id="valueTicker" className="form-control" value={detail.ticker} onChange={async (event) => setDetail({...detail, ticker: event.target.value})} disabled={!isEditState}></input>                
            </div>
        </div>
        <div className="row mb-2">
            <div className="col-12">
                <label  htmlFor="valueDescription" className="form-label">Description</label>
                <textarea id="valueDescription" className="form-control" rows="auto" value={detail.description} onChange={async (event) => setDetail({...detail, description: event.target.value })} disabled={!isEditState}></textarea>                         
            </div>
        </div>
        <div className="row mb-2 mt-5">
            {
                isEditState ?
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" title="Save" onClick={handleSubmitAsync}>Save</button>
                    </div>
                : 
                    detail.issuerAccountId == currentDomainAccount.id &&
                    <div className="d-grid col-3">
                        <button className="btn btn-primary" title="Edit" onClick={() => setIsEditState(true)}>Edit</button>
                    </div>   
            }
            {
                (isEditState || onCancel) &&
                <div className="d-grid col-3" >
                    <button className="btn btn-primary" title="Cancel" onClick={handleOnCancel}>Cancel</button>
                </div> 
            }
        </div>
    </div>
}