import React, {useState, useEffect, useRef, useCallback, useMemo } from 'react'
import {useSelector } from 'react-redux'
import { noprofileImageSrc, getImageSrc } from '../imageSources.js';
import ReactCrop from 'react-image-crop'
import "bootstrap-icons/font/bootstrap-icons.css";

import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

export const ValueDetail = ({header, valuesItem, api, onCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [detail, setDetail] = useState({
        voucherValueId: valuesItem.id,
        issuerAccountId: valuesItem.issuerAccountId ?? currentDomainAccount.id,
        voucherValueDetail: {
            ticker: valuesItem.ticker,
            description: valuesItem.description
        }
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
                voucherValueDetail: {
                    ticker: result.ticker,
                    description: result.description
                }
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
        console.log(JSON.stringify(detail.voucherValueDetail))

        formData.append('voucherValueDetail.ticker', detail.voucherValueDetail.ticker)
        formData.append('voucherValueDetail.description', detail.voucherValueDetail.description)

        if(detail.image)
            formData.append('image', detail.image)
        if(crop){
            formData.append('voucherValueDetail.cropParameters.x', parseFloat(crop.x))
            formData.append('voucherValueDetail.cropParameters.y', parseFloat(crop.y))
            formData.append('voucherValueDetail.cropParameters.width', parseFloat(crop.width))
            formData.append('voucherValueDetail.cropParameters.height', parseFloat(crop.height))
        }



        if(detail.voucherValueId ){
            formData.append('voucherValueId', detail.voucherValueId)

            await api.putValue(token, formData)           
        } else {
            if(currentDomainAccount.id) {
                formData.append('issuerDomainAccountId', currentDomainAccount.id)

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
                        <img style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={`${imgSrc ?? noprofileImageSrc}`}></img>
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
                <input type="text" id="valueTicker" className="form-control" value={detail.voucherValueDetail.ticker} onChange={async (event) => setDetail({...detail, voucherValueDetail: {...detail.voucherValueDetail, ticker: event.target.value}})} disabled={!isEditState}></input>                
            </div>
        </div>
        <div className="row mb-2">
            <div className="col-12">
                <label  htmlFor="valueDescription" className="form-label">Description</label>
                <textarea id="valueDescription" className="form-control" rows="auto" value={detail.voucherValueDetail.description} onChange={async (event) => setDetail({...detail, voucherValueDetail: { ...detail.voucherValueDetail, description: event.target.value }})} disabled={!isEditState}></textarea>                         
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