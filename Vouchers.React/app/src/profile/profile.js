import React, {useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { identityDefined } from '../store/userReducer.js'
import ReactCrop from 'react-image-crop'
import {Buffer} from 'buffer';
import { noprofileImageSrc } from '../noprofile.js';


import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

export const Profile = ({api}) => {

    const token = useSelector((state) => state.user.token)
    const identityId = useSelector((state) => state.user.identityId)

    const dispatch = useDispatch()

    const [detail, setDetail] = useState({
        firstName: "",
        lastName: "",
        email: token?.profile?.name
    })

    const [crop, setCrop] = useState()
    const [imgSrc, setImgSrc] = useState()
    const [isCropState, setIsCropState] = useState(false)

    useDetail(token, identityId, api.getIdentityDetail, setDetail, setImgSrc, setCrop)

    const onSelectFile = async e => {
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

    const handleFirstNameChange = async (event) => {
        setDetail({...detail, firstName: event.target.value})
    }

    const handleLastNameChange = async (event) => {
        setDetail({...detail, lastName: event.target.value})
    }

    const onImageLoad = async (e) => {

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

    const handleSaveCrop = async () => {

        const image = document.getElementById('croppedImage')

        const croppedImg = getCroppedImage(image, crop)

        const blobBin = Buffer.from(croppedImg.split(',')[1], 'base64')
        const croppedImgPng =  new Blob([blobBin], {type: 'image/png'})


        console.log(croppedImgPng)
        setDetail({
            ...detail,
            croppedImage: croppedImgPng,
        })

        setIsCropState(false)
    }

    const handleSubmit = async (event) => {
        const formData = new FormData();
        formData.append('firstName', detail.firstName)
        formData.append('lastName', detail.lastName)
        formData.append('email', detail.email)

        if(detail.image)
            formData.append('image', detail.image)
        if(detail.croppedImage){
            formData.append('cropParameters.X', parseFloat(crop.x))
            formData.append('cropParameters.Y', parseFloat(crop.y))
            formData.append('cropParameters.Width', parseFloat(crop.width))
            formData.append('cropParameters.Height', parseFloat(crop.height))
        }

        const id = await api.postIdentityDetail(token, formData)
        if(id) {
            dispatch(identityDefined(id))
        }
    }

    const header = <h1 className="header mt-2 mb-3">{identityId? 'My profile' : 'Please register'}</h1>

    return (
        //<form onSubmit={handleSubmit}>
        <div className="container">
            {header}
            {
                isCropState ?
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
                    <div className="row mb-2">                          
                        <div className="d-grid col-3">
                            { imgSrc && <button className="btn btn-primary" onClick={() => {setIsCropState(true); setCrop(crop)}}>Crop image</button> }
                        </div>
                        <div className="col-3">
                            <input type="file" name="Image" onChange={onSelectFile}/>
                        </div>
                    </div>
                </>
                              
            }     

            <div className="row mb-2">
                <div className="col-6">
                    <label htmlFor="profilyEmail" className="form-label">Email</label>
                    <input type="text" id="profilyEmail" className="form-control" value={detail.email} disabled={true}></input>               
                </div>  
            </div>
            
            <div className="row mb-5">
                <div className="col-6">
                    <label htmlFor="profilyFirstName" className="form-label">First name</label>
                    <input type="text" id="profilyFirstName" className="form-control" value={detail.firstName} onChange={handleFirstNameChange}></input>              
                </div>
                <div className="col-6">
                    <label htmlFor="profilyLastName" className="form-label">Last name</label>
                    <input type="text" id="profilyLastName" className="form-control" value={detail.lastName} onChange={handleLastNameChange}></input>                
                </div>
            </div>

            <div className="row mb-2">
                <div className="d-grid col-3">
                    <button className="btn btn-primary" onClick={handleSubmit}>Save</button>
                </div>
            </div>
        </div>
        //</form>
    );
}

const useDetail = (token, identityId, getIdentityDetail, setDetail, setImgSrc, setCrop) => {
    useEffect(() => {
        const effect = async () => {
            if(identityId) {
                const result = await getIdentityDetail(token)                              
                setDetail({
                    ...result,
                })
                if(result.imageBase64)
                    setImgSrc(`data:image/png;base64,${result.imageBase64}`)

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

        effect()
    }, []);
}

const getCroppedImage = (image, pixelCrop) => {
 
    const width = pixelCrop.width * image.width / 100
    const height = pixelCrop.height * image.height / 100

    let x = pixelCrop.x * image.naturalWidth / 100
    let y = pixelCrop.y * image.naturalHeight / 100

    const canvas = document.createElement('canvas');
    canvas.width = width
    canvas.height = height
    const ctx = canvas.getContext('2d');
    console.log(JSON.stringify(pixelCrop))

    const scaleX = image.naturalWidth / image.width
    const scaleY = image.naturalHeight / image.height

    const pixelRatio = window.devicePixelRatio

    canvas.width = Math.floor(width * scaleX * pixelRatio)
    canvas.height = Math.floor(height * scaleY * pixelRatio)

    ctx.scale(pixelRatio, pixelRatio)
    ctx.imageSmoothingQuality = 'high'

    const cropX = x * scaleX
    const cropY = y * scaleY

    const centerX = image.naturalWidth / 2
    const centerY = image.naturalHeight / 2
    ctx.save()

    // 5) Move the crop origin to the canvas origin (0,0)
    ctx.translate(-cropX, -cropY)
    // 4) Move the origin to the center of the original position
    ctx.translate(centerX, centerY)
    // 3) Rotate... not used
    // 2) Scale the image
    ctx.scale(1, 1)
    // 1) Move the center of the image to the origin (0,0)
    ctx.translate(-centerX, -centerY)

    ctx.drawImage(
        image,
        0,
        0,
        image.naturalWidth,
        image.naturalHeight,
        0,
        0,
        image.naturalWidth,
        image.naturalHeight,
    )

    ctx.restore()
   
    // As Base64 string
    return canvas.toDataURL('image/png');
}