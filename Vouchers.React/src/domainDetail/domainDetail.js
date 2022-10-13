import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { noprofileImageSrc, getImageSrc } from '../imageSources.js';
import ReactCrop from 'react-image-crop'
import "bootstrap-icons/font/bootstrap-icons.css";

import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

export const DomainDetail = ({api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector((state) => state.domainAccount.currentAccount)

    const [detail, setDetail] = useState({
        domainId: currentDomainAccount.domainId,
        domainDetail: {
            name: '',
            description: ''
        }
    })

    
    const [imgSrc, setImgSrc] = useState()
    const [crop, setCrop] = useState()
    const [isCropState, setIsCropState] = useState(false)
    const [isEditState, setIsEditState] = useState(false)

    useEffect(() => {

        const effectAsync = async () => {
            const result = await api.getDomainDetail(token, currentDomainAccount.domainId)

            console.log(JSON.stringify(result))

            if(result) {
                setDetail({
                    ...detail,
                    domainDetail: {
                        name: result.name,
                        description: result.description
                    }
                    
                })
            }

            if(result.imageId) {
                setImgSrc(getImageSrc(result.imageId))
            }
                
                //  `data:image/png;base64,${result.imageBase64}`)

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
    },[currentDomainAccount])

    const onSelectFileAsync = async e => {
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

    const onImageLoadAsync = async (e) => {

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

    const handleSaveCropAsync = async () => {
        setIsCropState(false)
    }

    const handleSubmitAsync = async () => {
        const formData = new FormData();

        console.log(JSON.stringify(detail))

        formData.append('domainId', detail.domainId)

        formData.append('domainDetail.description', detail.domainDetail.description)

        if(detail.image)
            formData.append('image', detail.image)
        if(crop){
            formData.append('domainDetail.cropParameters.x', parseFloat(crop.x))
            formData.append('domainDetail.cropParameters.y', parseFloat(crop.y))
            formData.append('domainDetail.cropParameters.width', parseFloat(crop.width))
            formData.append('domainDetail.cropParameters.height', parseFloat(crop.height))
        }

        await api.putDomainDetail(token, formData)
        
        setIsEditState(false)
    }

    const handleOnCancelAsync = async () => {
        if(isEditState) {
            setIsEditState(false)
            return
        }
    }

    return <div className="container">
            <h1 className="header mt-2 mb-3">Domain detail</h1>
            {
                isEditState && isCropState ?
                <>
                    
                    <div className="row mb-2">
                        <div className="col-12">
                            <ReactCrop crop={crop} onChange={(pixelCrop, percentCrop) => setCrop(percentCrop)} aspect={1}>
                                <img id='croppedImage' style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={imgSrc} onLoad={onImageLoadAsync} />
                            </ReactCrop>
                        </div>
                    </div>
                    <div className="row mb-2">
                        <div className="d-grid col-3">
                            <button className="btn btn-primary" onClick={handleSaveCropAsync}>Save</button>
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
                                <input type="file" name="Image" onChange={onSelectFileAsync}/>
                            </div>
                        </div>
                    }
                </>
                                
            }  
            <div className="row mb-2">
                <div className='col-12'>
                    <label htmlFor="domainName" className="form-label">Name</label>
                    <input type="text" id="domainName" className="form-control" value={detail.domainDetail.name} onChange={async (event) => setDetail({...detail, domainDetail: {...detail.domainDetail, name: event.target.value}})} disabled={!isEditState}></input>                
                </div>
            </div>
            <div className="row mb-2">
                <div className='col-12'>
                    <label  htmlFor="domainDescription" className="form-label">Description</label>
                    <textarea id="domainDescription" className="form-control" rows="auto" value={detail.domainDetail.description} onChange={async (event) => setDetail({...detail, domainDetail: { ...detail.domainDetail, description: event.target.value }})} disabled={!isEditState}></textarea>                         
                </div>
            </div>
            <div className="row mb-2 mt-5">
            {

                isEditState ?               
                    <div className="d-grid col-3" >
                        <button className="btn btn-primary" title="Save" onClick={handleSubmitAsync}>Save</button>
                    </div>
                    : (currentDomainAccount.isAdmin || currentDomainAccount.isOwner) && <div className="d-grid col-3">
                        <button className="btn btn-primary" title="Edit" onClick={() => setIsEditState(true)}>Edit</button>
                    </div>
            }
            {
                isEditState && <div className="d-grid col-3">
                    <button className="btn btn-primary" title="Cancel" onClick={handleOnCancelAsync}>Cancel</button>
                </div>
            }
            </div>
        </div>
}